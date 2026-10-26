using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceBossTierChecks
{
    private const string Driver = "fixture:source-boss-tier-driver";
    private const string Deck = "fixture:source-boss-tier-deck";
    private const int BossSeat = 3;

    public static void Source2644NativeFormsCountPublicEnemiesAndRestrictShenjiToOwnPlay()
    {
        foreach (var difficulty in new[] { 11, 21 })
        {
            var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            var encounter = source.Nodes.SelectMany(node => node.Encounters ?? [])
                .Single(candidate => candidate.SourceBattleId == 2644);
            // Keep the real source bodies, second-form recipe and fixed equipment.
            // Extra friendly HP and a fixed deck isolate the stage skills from growth and draw randomness.
            var setup = new ShanheBattleSetup(71, 1, "fixture:source-2644-tier-" + difficulty,
                "classic:guan-yu", 34, 30, 0, 0, 0, [Driver], [], [],
                encounter.Enemies.Select(enemy => enemy with
                {
                    StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Slash, Suit.Spade, 7), 4).ToArray(),
                    StartingCards = []
                }).ToArray(),
                [new("classic:ma-chao", MaxHpBonus: 30, Hp: 34) { StartingHand = [] },
                 new("classic:zhang-fei", MaxHpBonus: 30, Hp: 34) { StartingHand = [] }],
                ShanheCampaignContent.GetContentHash(source))
            { SourceDifficultyId = difficulty, BossTransition = encounter.BossTransition };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] })
                .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId
                    ? mode with { DeckId = Deck } : mode);
            var scene = new Scene(setup, registry);
            scene.EnterSecondFormPlay();

            var view = scene.Game.CreateSnapshot(BossSeat);
            var boss = view.Players[BossSeat];
            var expectedForm = difficulty == 11 ? StandardShanheFengxianBossPackage.SourceRageHardId :
                StandardShanheFengxianBossPackage.SourceRageHellId;
            Need(scene.Events<ShanheBossStageStartedEvent>().Single().GeneralId == expectedForm &&
                boss is { Hp: 5, MaxHp: 5 } && boss.GeneralId == expectedForm,
                "Real lethal damage must enter the selected source 2644 second form before its natural turn.");
            var publicEnemies = view.Players.Count(player => player.IsAlive && player.IsTeamRevealed &&
                player.TeamId is not null && player.TeamId != boss.TeamId);
            var bossTurn = scene.Events<TurnStartedEvent>().Last(fact => fact.ActorSeat == BossSeat).TurnNumber;
            var normalDraws = scene.Game.CardMovements.Count(move => move.TurnNumber == bossTurn &&
                move.To == CardLocation.Hand(BossSeat) && move.Reason == CardMoveReasons.Draw);
            Need(publicEnemies == 3 && normalDraws == 2 + publicEnemies && boss.HandLimit == boss.Hp + publicEnemies,
                "The transformed boss's actual normal draw and visible hand limit add the three living public enemies, excluding itself.");

            if (difficulty != 21) continue;
            CheckShenjiPhaseQueries(registry, boss);
            scene.FinishFourPhysicalSlashes();
            var uses = scene.Events<CardUseDeclaredEvent>().Where(fact => fact.SourceSeat == BossSeat &&
                fact.CardKind == CardKind.Slash && fact.CardId > 0).Take(4).ToArray();
            Need(uses.Length == 4 && uses.Select(use => use.CardId).Distinct().Count() == 4 &&
                scene.Game.State.CurrentSeat == BossSeat && scene.Game.State.Phase == TurnPhase.Play,
                "Hell Shenji must really use four distinct physical Slashes in the same own Play phase without a Crossbow.");
            foreach (var use in uses)
            {
                Need(scene.Events<TargetsConfirmedEvent>().Single(fact => fact.ResolutionId == use.ResolutionId)
                        .TargetSeats.Order().SequenceEqual([0, 1, 2]) &&
                    scene.Events<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == use.ResolutionId) &&
                    scene.Game.CardMovements.Any(move => move.CardId == use.CardId && move.TurnNumber == bossTurn &&
                        move.From == CardLocation.Hand(BossSeat) && move.Reason == CardMoveReasons.Use),
                    "Each accepted physical Slash must pay its own real card, finish, and legally confirm all three public enemies.");
            }
            Need(scene.Game.CreateSnapshot(0).Players.Where(player => player.Seat != BossSeat).All(player => player.IsAlive),
                "All three native targets survive the four uses, so later Slashes do not rely on removed seats.");
        }
    }

    private static void CheckShenjiPhaseQueries(ContentRegistry registry, PlayerSnapshot boss)
    {
        var program = registry.GetSkill(StandardShanheFengxianBossPackage.ShenjiId).Program!;
        SkillProgramRuleSource[] sources = [new(program.Id, "fixture:current-shenji", program)];
        foreach (var phase in Enum.GetValues<TurnPhase>())
        foreach (var isOwnTurn in new[] { false, true })
        foreach (var kind in new[] { CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash })
        {
            var context = new SkillProgramRuleContext(new(boss.Seat, boss.Hp, boss.MaxHp, boss.HandCount,
                phase, IsOwnTurn: isOwnTurn), 1, EffectiveCardKind: kind);
            var quota = RuleQueryService.Evaluate(SkillRuleQuery.SlashLimit, new(0, int.MaxValue),
                [new("ordinary-slash", 1)], SkillProgramRules.CollectContributions(SkillRuleQuery.SlashLimit, context, sources));
            var targets = RuleQueryService.Evaluate(SkillRuleQuery.CardTargetCount, new(0, int.MaxValue),
                [new("ordinary-target", 1)], SkillProgramRules.CollectContributions(SkillRuleQuery.CardTargetCount, context, sources));
            var ownPlay = isOwnTurn && phase == TurnPhase.Play;
            Need(quota.IsUnlimited == ownPlay && (ownPlay || quota.Value is FiniteRuleQueryValue { Value: 1 }) &&
                targets.Value is FiniteRuleQueryValue finite && finite.Value == (ownPlay ? 3 : 1),
                $"Actual Shenji query contributions must allow unlimited three-target {kind} only in own Play; phase={phase}, own={isOwnTurn}.");
        }
    }

    private sealed class Scene
    {
        public GameEngine Game { get; }
        public Scene(ShanheBattleSetup setup, ContentRegistry registry)
        {
            Game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, registry);
            Submit(new StartGameCommand());
        }

        public T[] Events<T>() where T : IGameEvent => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public void EnterSecondFormPlay()
        {
            Reach(() => Game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && Game.ResolutionStack.Count == 0);
            Submit(new UseProgramSkillCommand(0, Driver, "stage", [], [BossSeat], Game.Revision, Game.PendingDecision!.PromptId));
            Reach(() => Events<ShanheBossStageStartedEvent>().Length == 1 && Game.State.CurrentSeat == BossSeat &&
                Game.State.Phase == TurnPhase.Play && Game.ResolutionStack.Count == 0);
        }

        public void FinishFourPhysicalSlashes() => Reach(() =>
            Events<CardUseDeclaredEvent>().Where(fact => fact.SourceSeat == BossSeat && fact.CardKind == CardKind.Slash && fact.CardId > 0)
                .Take(4).ToArray() is { Length: 4 } uses &&
            uses.All(use => Events<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == use.ResolutionId)) &&
            Game.ResolutionStack.Count == 0);

        private void Reach(Func<bool> stop)
        {
            for (var step = 0; step < 480; step++)
            {
                if (stop()) return;
                if (Game.PendingDecision is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.Kind == DecisionKind.PlayCard && Game.ResolutionStack.Count != 0)
                    { Submit(new AdvanceOneStepCommand(Game.Revision)); continue; }
                    var choice = prompt.Choices.FirstOrDefault(candidate => candidate.Cards.Count == 0 &&
                        candidate.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
                    if (choice is not null)
                    { Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision)); continue; }
                    throw new InvalidOperationException("Unexpected source boss tier human boundary: " + JsonSerializer.Serialize(prompt));
                }
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Source boss tier check exceeded its bounded native steps: " + JsonSerializer.Serialize(Game.PendingDecision));
        }
        private void Submit(GameCommand command)
        {
            var result = Game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "Source boss tier native command rejected.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-boss-tier", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[{"id":"stage","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,
                    "effects":[{"op":"damage","target":"selectedTarget","amount":20}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "阶段检查", description = "原生致命伤害触发来源第二形态。" } } }));
            builder.AddSkill(new(Driver, "阶段检查", "原生致命伤害触发来源第二形态。")
            { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddDeck(new(Deck, "固定杀牌堆", 0, 2, [new("standard:slash", 180)]));
        }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }
}
