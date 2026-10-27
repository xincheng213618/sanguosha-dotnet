using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceStageEquipmentChecks
{
    private const string Driver = "fixture:source-stage-equipment-driver", Deck = "fixture:source-stage-equipment-deck";
    private const string Wushuang = "shanhe:shengui-wushuang", Shenqu = "shanhe:shengui-shenqu";
    private const int BossSeat = 1;

    public static void SourceThresholdsResolveExactFormsAndCumulativeStageEquipment()
    {
        foreach (var difficulty in new[] { 3, 6, 11, 16, 21, 25 })
        {
            var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            foreach (var battleId in new[] { 2644, 2645, 2671 })
            {
                var encounter = Encounter(source, battleId);
                var stage = encounter.BossTransition!;
                var setup = RawSetup(source, encounter, difficulty, battleId, withDriver: false);
                var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
                var expectedId = battleId switch
                {
                    2645 => difficulty >= 21 ? StandardShanheBossPackage.ShenguiLvbuId : difficulty >= 16
                        ? StandardShanheBossPackage.SourceShenguiNightmareId : "classic:lu-bu",
                    2671 => difficulty >= 16 ? StandardShanheS5BossPackage.DongMinCuanchaoId : StandardShanheS5BossPackage.DongMinBaseId,
                    _ => difficulty >= 21 ? StandardShanheFengxianBossPackage.SourceRageHellId : difficulty >= 11
                        ? StandardShanheFengxianBossPackage.SourceRageHardId : StandardShanheFengxianBossPackage.RageId
                };
                Need(stage.GeneralId == expectedId && stage.Combatant?.GeneralId == expectedId &&
                    stage.Hp == (battleId == 2645 ? 3 : 5) && stage.MaxHp == stage.Hp && stage.DrawCards == 4,
                    "The real source transition selects its tier's form while retaining source three/three or five/five and the existing phase-card recipe, rather than copying the printed body's health.");
                Need((stage.StartingEquipment ?? []).Select(Face).SequenceEqual(ExpectedEquipment(battleId, difficulty).Select(Face)) &&
                    (stage.StartingEquipment ?? []).All(card => registry.Cards.Values.Any(definition => definition.LegacyKind == card.Kind)),
                    "Every stage's cumulative equipment thresholds must resolve to the actual registered physical card faces.");
                var general = registry.Generals[expectedId];
                var skills = new[] { general.SkillId }.OfType<string>().Concat(general.AdditionalSkillIds ?? []).Order().ToArray();
                if (battleId == 2645)
                    Need(skills.SequenceEqual((difficulty >= 21 ? new[] { Wushuang, Shenqu } : difficulty >= 16 ? new[] { Wushuang } : ["classic:wushuang"]).Order()) &&
                        (difficulty < 21 || general.BaseHp == 6 && stage.MaxHp == 3),
                        "The source 264501 panel has classic Wushuang through fifteen, only dynamic Wushuang at sixteen, and adds Shenqu at twenty-one without adopting printed six HP.");
                if (battleId == 2671)
                    Need(skills.SequenceEqual((difficulty >= 16 ? new[] { StandardShanheS5BossPackage.DungongId, StandardShanheS5BossPackage.CuanchaoId }
                            : [StandardShanheS5BossPackage.DungongId]).Order()),
                        "The Dong Min phase retains its real shield skill and adds Cuanchao only at sixteen.");
                if (battleId is 2644 or 2645)
                    Need((encounter.Enemies[0].StartingHand?.Count ?? 4) == 4 && encounter.Enemies[0].InitialHandBonus == 0 &&
                        (encounter.Enemies[0].StartingCards ?? []).Count(card => card.Kind == CardKind.SilverLion) == (difficulty >= 11 ? 1 : 0) &&
                        !(encounter.Enemies[0].StartingEquipment ?? []).Any(card => card.Kind == CardKind.SilverLion),
                        "The extra hard-tier Silver Lion is added to the first form's hand recipe, preserving its four-card base and independent source-growth attributes.");
            }
        }
    }

    public static void Source264501LethalStageInstallsRealGearAndUsesItsNewNativeSkills()
    {
        foreach (var difficulty in new[] { 16, 21 })
        {
            var scene = Scene.Start(2645, difficulty);
            var firstHand = scene.Boss.Hand;
            Need(firstHand.Count(card => card.Kind == CardKind.SilverLion) == 1 && firstHand.Count >= 5,
                "The real first form actually holds its extra Silver Lion together with the four-card fixture opening, before lethal damage.");
            scene.EnterSecondFormPlay();
            Need(scene.Boss is { Hp: 3, MaxHp: 3 } && scene.Boss.GeneralId == scene.Setup.BossTransition!.GeneralId &&
                scene.Boss.Skills!.Any(skill => skill.ContentId == Wushuang) && !scene.Boss.Skills.Any(skill => skill.ContentId == "classic:wushuang") &&
                scene.Boss.Skills.Any(skill => skill.ContentId == Shenqu) == (difficulty >= 21),
                "Actual lethal damage replaces the first form with the exact three-HP source panel and its correctly gated skills.");
            scene.AssertInstalledStageEquipment();
            scene.FinishBossSlashes(1);
            var uses = scene.BossSlashUses().Take(1).ToArray();
            Need(uses.Length == 1 && scene.RequiredDodges.GetValueOrDefault(uses[0].ResolutionId) == 1,
                "The transformed form can act in its new natural turn and reaches the real first-Slash native response window.");
            foreach (var use in uses)
                Need(scene.Game.CardMovements.Count(move => move.CardId == use.CardId && move.From == CardLocation.Hand(BossSeat) &&
                        move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
                    scene.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == use.ResolutionId) == 1,
                    "Every demonstrated Slash pays its own actual entity and completes once.");
            Need(scene.Events<ShanheBossStageStartedEvent>().Length == 1 && scene.Boss.IsAlive && scene.Game.State.Winner == Winner.None,
                "The real boss stays actionable after its single transition and skill/equipment flows.");
        }
    }

    public static void Source267101StageUsesCrownDrawCapAndTierGatedCuanchao()
    {
        foreach (var difficulty in new[] { 11, 16 })
        {
            var scene = Scene.Start(2671, difficulty); scene.EnterSecondFormPlay();
            Need(scene.Boss is { Hp: 5, MaxHp: 5, IncomingDamageCap: 3 } &&
                scene.Boss.Skills!.Any(skill => skill.ContentId == StandardShanheS5BossPackage.CuanchaoId) == (difficulty >= 16),
                "Native transformation starts the source five/five shield form, with the actual incoming cap and the correct Cuanchao threshold.");
            scene.AssertInstalledStageEquipment();
            var turn = scene.Events<TurnStartedEvent>().Last(fact => fact.ActorSeat == BossSeat).TurnNumber;
            Need(scene.Game.CardMovements.Count(move => move.TurnNumber == turn && move.To == CardLocation.Hand(BossSeat) &&
                    move.Reason == CardMoveReasons.Draw) == 4 && scene.Boss.HandLimit == 4,
                "The physically installed Delusion Crown adds two actual normal draw cards and reduces this five-HP phase's hand limit to four.");
            scene.FinishBossSlashes(1);
            var use = scene.BossSlashUses().Single();
            Need(use.CardId > 0 && scene.Game.CardMovements.Count(move => move.CardId == use.CardId &&
                    move.From == CardLocation.Hand(BossSeat) && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
                scene.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == use.ResolutionId) == 1 &&
                scene.Events<ShanheBossStageStartedEvent>().Length == 1 && scene.Boss.IsAlive,
                "The equipped source body continues into a real paid native Slash and completes it once after its sole transformation.");
            scene.PlayerPlay();
            Need(scene.Game.CreateSnapshot(0).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 },
                "The new source panel and installed crown return naturally to the next actionable human turn.");
        }
    }

    private static ShanheEncounterDefinition Encounter(ShanheCampaignDefinition source, int id) =>
        source.Nodes.SelectMany(node => node.Encounters ?? []).Single(encounter => encounter.SourceBattleId == id);
    private static ShanheStartingCard[] ExpectedEquipment(int battle, int difficulty)
    {
        if (battle == 2671) return difficulty >= 11 ? [new(CardKind.DelusionCrown, Suit.Diamond, 4)] : [];
        var result = new List<ShanheStartingCard> { new(battle == 2645 ? CardKind.ShuraInfernoHalberd : CardKind.WushuangHalberd, Suit.Heart, 12) };
        if (difficulty >= 6) result.Add(new(CardKind.PurpleGoldCrown, Suit.Heart, 1));
        if (difficulty >= 11) result.Add(battle == 2645 ? new(CardKind.LionBelt, Suit.Club, 2) : new(CardKind.RedCottonRobe, Suit.Diamond, 1));
        return result.ToArray();
    }
    private static (CardKind Kind, Suit Suit, int Rank) Face(ShanheStartingCard card) => (card.Kind, card.Suit, card.Rank);
    private static bool IsSlash(CardKind kind) => kind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash;
    private static ShanheBattleSetup RawSetup(ShanheCampaignDefinition source, ShanheEncounterDefinition encounter, int difficulty, int battleId, bool withDriver) =>
        new(71, 1, "fixture:source-stage-equipment-" + battleId + "-" + difficulty, "classic:guan-yu", 34, 30, 0, 0, 0,
            withDriver ? [Driver] : [], [new(CardKind.Slash, Suit.Heart, 10), new(CardKind.Dodge, Suit.Club, 2)], [],
            encounter.Enemies.Select(member => member with
            {
                // Keep the actual body, source fixed cards and every stage input.
                // Only the ordinary opening four become fixed Slashes here.
                StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Slash, Suit.Spade, 7), 4).ToArray()
            }).ToArray(), [], ShanheCampaignContent.GetContentHash(source))
        { SourceDifficultyId = difficulty, BossTransition = encounter.BossTransition };

    private sealed class Scene(ShanheBattleSetup setup, ContentRegistry registry)
    {
        public ShanheBattleSetup Setup { get; } = setup;
        public GameEngine Game { get; } = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 1 + setup.EnemyRoster.Count, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12
        }, registry);
        public Dictionary<long, int> RequiredDodges { get; } = [];
        public PlayerSnapshot Boss => Game.CreateSnapshot(BossSeat).Players[BossSeat];
        private PendingDecision? P => Game.CreateSnapshot(0).PendingDecision;
        private int _beforeStageMove;
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public CardUseDeclaredEvent[] BossSlashUses() => Events<CardUseDeclaredEvent>().Where(fact => fact.SourceSeat == BossSeat && IsSlash(fact.CardKind)).ToArray();
        public static Scene Start(int battleId, int difficulty)
        {
            var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            var setup = RawSetup(source, Encounter(source, battleId), difficulty, battleId, withDriver: true);
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] })
                .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var scene = new Scene(setup, registry); scene.Submit(new StartGameCommand()); scene.PlayerPlay(); return scene;
        }
        public void EnterSecondFormPlay()
        {
            _beforeStageMove = Game.CardMovements.Select(move => move.Sequence).DefaultIfEmpty(0).Max();
            Submit(new UseProgramSkillCommand(0, Driver, "stage", [], [BossSeat], Game.Revision, P!.PromptId));
            Reach(() => Events<ShanheBossStageStartedEvent>().Length == 1 && Game.State.CurrentSeat == BossSeat &&
                Game.State.Phase == TurnPhase.Play && Game.ResolutionStack.Count == 0, endOwnPlay: true);
            Need(Events<ShanheBossTurnInterruptedEvent>() is [{ PlayerSeat: 0 }] &&
                Events<TurnStartedEvent>().Last().ActorSeat == BossSeat && Game.State.TurnNumber > 1,
                "The lethal native attempt interrupts the old actor and starts a new natural boss turn exactly once.");
        }
        public void AssertInstalledStageEquipment()
        {
            var expected = Setup.BossTransition!.StartingEquipment!;
            Need(Boss.Equipment.Count == expected.Count && Boss.Equipment.Select(card => (card.Kind, card.Suit, card.Rank)).Order()
                    .SequenceEqual(expected.Select(Face).Order()) &&
                Boss.Equipment.All(card => Game.CardMovements.Count(move => move.Sequence > _beforeStageMove && move.CardId == card.Id &&
                    move.From == CardLocation.OutsideGame && move.To == CardLocation.Equipment(BossSeat)) == 1),
                "The actual transition installs every exact equipment entity directly into its native slot once, rather than leaving it in hand or using a drawn replacement.");
        }
        public void FinishBossSlashes(int count) => Reach(() => BossSlashUses().Take(count).ToArray() is { } uses && uses.Length == count &&
            uses.All(use => Events<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == use.ResolutionId)) && Game.ResolutionStack.Count == 0, endOwnPlay: true);
        public void PlayerPlay() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Game.ResolutionStack.Count == 0);
        private void Reach(Func<bool> done, bool endOwnPlay = false)
        {
            for (var step = 0; step < 480; step++)
            {
                if (done()) return;
                Need(Game.State.Winner == Winner.None, "The bounded stage fixture ended before its native assertion boundary.");
                if (P is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.Kind == DecisionKind.PlayCard && Game.ResolutionStack.Count != 0)
                    { Submit(new AdvanceOneStepCommand(Game.Revision)); continue; }
                    if (prompt.Kind == DecisionKind.PlayCard && endOwnPlay)
                    { Submit(new EndPlayPhaseCommand(0, Game.Revision, prompt.PromptId)); continue; }
                    if (prompt.Kind == DecisionKind.RespondDodge)
                    {
                        var use = Game.ResolutionStack.OfType<CardUseFrame>().Last(frame => frame.SourceSeat == BossSeat && IsSlash(frame.CardKind));
                        Need(use.CardAttack is { Active: true }, "The real human response must retain its native attacking use.");
                        RequiredDodges[use.Id] = use.CardAttack!.RequiredDodgeResponses;
                    }
                    var choice = prompt.Choices.FirstOrDefault(value => value.Cards.Count == 0 &&
                        value.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
                    Need(choice is not null, $"Unexpected source stage boundary {prompt.Kind}/{prompt.SkillPrompt?.SkillId}.");
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, Game.Revision)); continue;
                }
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException($"Stage equipment flow exceeded its native bound: {P?.Kind}/{Game.State.CurrentSeat}/{Game.State.Phase}.");
        }
        private void Submit(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native stage-equipment command."); }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-stage-equipment", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"stage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,
                     "effects":[{"op":"damage","target":"selectedTarget","amount":20}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "原生阶段攻击", description = "真实致命伤害触发来源第二形态。" } } }));
            builder.AddSkill(new(Driver, "原生阶段攻击", "真实致命伤害触发来源第二形态。")
            { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddDeck(new(Deck, "固定实体杀", 0, 2, [new("standard:slash", 180)]));
        }
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
