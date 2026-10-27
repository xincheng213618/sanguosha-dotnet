using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceZhangYanStageChecks
{
    private const string Driver = "fixture:source-zhangyan-stage-driver";
    private const string Deck = "fixture:source-zhangyan-stage-deck";
    private const int BossSeat = 1;

    public static void Source2653MediumRouteLethalTransitionStartsTheRealZhangYanTurn()
    {
        foreach (var difficulty in new[] { 6, 10 })
        {
            var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            var region = StandardShanheSourceRouteCatalog.Season5.ByRegionId[6];
            var node = source.Nodes.Single(candidate => candidate.Id == "source-city-5068");
            var encounter = node.Encounters!.Single(candidate => candidate.SourceBattleId == 2653);
            Need(region.MinimumSourceDifficultyId == 6 && region.CityIds.Contains(5068) &&
                source.Nodes.Single(candidate => candidate.Id == "source-region-6:camp").NextNodeIds.Contains(node.Id) &&
                node is { ChapterNumber: 2, Kind: ShanheNodeKind.BossBattle } && encounter.SourceCityId == 5068 &&
                encounter.Enemies.Count == 1 && encounter.Enemies[0].GeneralId == StandardShanheSourceZhangYanPackage.ZhangniujiaoMediumId &&
                encounter.BossTransition is { Hp: 4, MaxHp: 4 },
                "The actual medium source map offers group2653 at region6/city5068, including its complete two-form encounter.");

            var setup = new ShanheBattleSetup(71, 1, "fixture:source-zhangyan-" + difficulty,
                "classic:guan-yu", 34, 30, 0, 0, 0, [Driver],
                [new(CardKind.Slash, Suit.Heart, 10), new(CardKind.Dodge, Suit.Club, 2)], [],
                encounter.Enemies.Select(member => member with
                {
                    // Only make the ordinary initial four deterministic. Keep
                    // the actual source body, tier rows and phase recipe.
                    StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Slash, Suit.Spade, 7), 4).ToArray()
                }).ToArray(), [], ShanheCampaignContent.GetContentHash(source))
            { SourceDifficultyId = difficulty, BossTransition = encounter.BossTransition };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] })
                .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var scene = new Scene(setup, registry);
            scene.Submit(new StartGameCommand());
            scene.Reach(() => scene.Prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && scene.Game.ResolutionStack.Count == 0);
            Need(scene.Boss is { Hp: 3, MaxHp: 3, HandCount: 4 } && scene.Boss.Skills!.Select(skill => skill.ContentId)
                    .SequenceEqual([StandardShanheS5XuncanCaoshiPackage.LeijieId]) &&
                !scene.Events<ShanheBossStageStartedEvent>().Any(),
                "The actual first form begins at three/three with only the reused sourceLeijie, four physical opening cards and no premature second form.");

            var round = scene.Events<RoundStartedEvent>().Last().RoundNumber;
            var turn = scene.Game.State.TurnNumber;
            scene.Submit(new UseProgramSkillCommand(0, Driver, "stage", [], [BossSeat], scene.Game.Revision, scene.Prompt!.PromptId));
            scene.Reach(() => scene.Events<ShanheBossStageStartedEvent>().Length == 1 &&
                scene.Game.State.CurrentSeat == BossSeat && scene.Game.State.Phase == TurnPhase.Play && scene.Game.ResolutionStack.Count == 0);
            Need(scene.Events<DamageAppliedEvent>().Any(fact => fact.SourceSeat == 0 && fact.TargetSeat == BossSeat && fact.Amount == 20) &&
                scene.Events<ShanheBossStageStartedEvent>() is [{ BossSeat: BossSeat, Hp: 4, MaxHp: 4 } stage] &&
                stage.PreviousGeneralId == StandardShanheSourceZhangYanPackage.ZhangniujiaoMediumId &&
                stage.GeneralId == StandardShanheSourceZhangYanPackage.ZhangYanMediumId &&
                scene.Boss is { Hp: 4, MaxHp: 4, IsAlive: true } && scene.Boss.GeneralId == stage.GeneralId &&
                scene.Boss.Skills!.Select(skill => skill.ContentId).Order().SequenceEqual(new[]
                    { StandardShanheSourceZhangYanPackage.YanyuSkillId, StandardShanheSourceZhangYanPackage.BiriSkillId }.Order()),
                "A genuine lethal damage attempt enters the exact living four/four ZhangYan once, with Yanyu+Biri and no ordinaryZhangYan or first-form skills.");
            Need(scene.Events<ShanheBossTurnInterruptedEvent>() is [{ PlayerSeat: 0, BossSeat: BossSeat }] &&
                scene.Events<TurnStartedEvent>().Last().ActorSeat == BossSeat && scene.Game.State.TurnNumber > turn &&
                scene.Events<RoundStartedEvent>().Last().RoundNumber == round,
                "The native phase contract retires the original human turn and starts the boss's natural turn within the same actual round.");

            scene.Reach(() => scene.BossSlashes() is [var use] &&
                scene.Events<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == use.ResolutionId) && scene.Game.ResolutionStack.Count == 0);
            var slash = scene.BossSlashes().Single();
            Need(slash.CardId > 0 && scene.Game.CardMovements.Count(move => move.CardId == slash.CardId &&
                    move.From == CardLocation.Hand(BossSeat) && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
                scene.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == slash.ResolutionId) == 1 &&
                scene.Events<ShanheBossStageStartedEvent>().Length == 1 && scene.Game.State.Winner == Winner.None,
                "The transformed boss really acts with a physical paid Slash, finishes it once, and remains in a live encounter after its sole transition.");
        }
    }

    private sealed class Scene(ShanheBattleSetup setup, ContentRegistry registry)
    {
        public GameEngine Game { get; } = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 1 + setup.EnemyRoster.Count, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);
        public PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot Boss => Game.CreateSnapshot(BossSeat).Players[BossSeat];
        public T[] Events<T>() => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public CardUseDeclaredEvent[] BossSlashes() => Events<CardUseDeclaredEvent>()
            .Where(fact => fact.SourceSeat == BossSeat && fact.CardKind == CardKind.Slash).ToArray();

        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 240; step++)
            {
                if (done()) return;
                Need(Game.State.Winner == Winner.None, "The bounded ZhangYan fixture ended before the required native boundary.");
                if (Prompt is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.Kind == DecisionKind.PlayCard && Game.ResolutionStack.Count != 0)
                    { Submit(new AdvanceOneStepCommand(Game.Revision)); continue; }
                    var pass = prompt.Choices.FirstOrDefault(choice => choice.Cards.Count == 0 &&
                        choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                    Need(pass is not null, $"Unexpected ZhangYan stage boundary: {prompt.Kind}/{prompt.SkillPrompt?.SkillId}.");
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, pass!.Id, Game.Revision));
                    continue;
                }
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException($"ZhangYan stage flow exceeded its bound: {Prompt?.Kind}/{Game.State.CurrentSeat}/{Game.State.Phase}.");
        }

        public void Submit(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native ZhangYan fixture command."); }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-zhangyan-stage", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"stage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,
                     "effects":[{"op":"damage","target":"selectedTarget","amount":20}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "阶段攻击", description = "真实致死伤害触发来源阶段。" } } }));
            builder.AddSkill(new(Driver, "阶段攻击", "真实致死伤害触发来源阶段。")
            { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddDeck(new(Deck, "固定原生杀", 0, 2, [new("standard:slash", 100)]));
        }
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
