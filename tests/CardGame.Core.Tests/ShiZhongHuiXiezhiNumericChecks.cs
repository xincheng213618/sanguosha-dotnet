using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class ShiZhongHuiXiezhiNumericChecks
{
    private const string Hero = "mobile:shi-zhong-hui", Driver = "fixture:xiezhi-health";
    private const string Peer = "fixture:xiezhi-peer", Deck = "fixture:xiezhi-deck";

    internal static void OneOverflowFundsTwoSlashesAndARealHandLimitCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        ShanheStartingCard[] opening = [new(CardKind.Slash, Suit.Spade, 3), new(CardKind.Slash, Suit.Club, 4),
            new(CardKind.Slash, Suit.Heart, 5), new(CardKind.Dodge, Suit.Heart, 6), new(CardKind.Dodge, Suit.Club, 7)];
        var setup = new ShanheBattleSetup(617, 1, "xiezhi-overflow-numeric", Hero, 4, 0, 0, 0, 0,
            [Driver], opening, [], [new(Peer) { StartingHand = [] }], [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 3
        }, content);
        Accept(game, new StartGameCommand());
        Settle(game);
        var turn = Facts<TurnStartedEvent>(game).Single().TurnNumber;
        var slashes = game.CreateSnapshot(0).Players[0].Hand.Where(card => card.Kind == CardKind.Slash).Select(card => card.Id).ToArray();
        Require(slashes.Length == 3 && game.CreateSnapshot(0).Players[0] is { GeneralId: Hero, Hp: 4, HandCount: 7 },
            "The real Zhong Hui starts Play at four HP with seven hand cards, including three physical Slashes.");

        UseHealth("lose");
        Require(Facts<ProgramSkillHpLostEvent>(game).Single() is { SkillId: Driver, TargetSeat: 0, Amount: 3, RemainingHp: 1 } &&
            !Facts<ShiZhHuiXiezhiOverflowEvent>(game).Any(), "The original loss of three HP fills only three charge points and issues no bonus.");
        UseHealth("recover");
        var overflow = Facts<ShiZhHuiXiezhiOverflowEvent>(game).Single();
        Require(Facts<RecoveryAppliedEvent>(game).Single() == new RecoveryAppliedEvent(0, 0, 3, 4) &&
            overflow is { OwnerSeat: 0, Excess: 2, Total: 1 } &&
            Facts<PlayerMarkerChangedEvent>(game).Where(fact => fact is { PlayerSeat: 0, Marker: PlayerMarkerKind.ShiZhHuiCharge })
                .Select(fact => fact.Count).SequenceEqual([3, 4]),
            "Recovering three real HP overflows by two points and grants just one Xiezhi unit.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        RequireSameReplay(game, restored);
        game = restored;
        foreach (var id in slashes.Take(2))
        {
            Require(game.GetLegalActions(0).Any(action => action.CardId == id && action.Kind == LegalActionKind.Slash &&
                action.ConversionSource is null && action.TargetSeats.SequenceEqual([1])), "The next physical Slash has an ordinary legal action.");
            Accept(game, new PlayCardCommand(0, id, [1], game.Revision, P(game)!.PromptId, CardKind.Slash));
            Settle(game);
            var use = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.SourceSeat == 0 && fact.CardId == id);
            Require(game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
                Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == use.ResolutionId && fact.CardId == id && fact.CardKind == CardKind.Slash) == 1,
                "Each of the two physical Slashes pays its original entity and completes in the native use.");
        }

        var held = game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).Order().ToArray();
        var revision = game.Revision;
        var movementCount = game.CardMovements.Count;
        var rejected = game.Submit(new PlayCardCommand(0, slashes[2], [1], revision, P(game)!.PromptId, CardKind.Slash));
        Require(!rejected.Accepted && game.Revision == revision && game.CardMovements.Count == movementCount &&
            held.Length == 5 && held.Contains(slashes[2]) && !game.GetLegalActions(0).Any(action => action.CardId == slashes[2]) &&
            Facts<CardUseDebitRecordedEvent>(game).Count(fact => fact.Debit is { ActorSeat: 0, Query: SkillRuleQuery.SlashLimit, Phase: TurnPhase.Play } &&
                fact.Debit.TurnNumber == turn) == 2, "One overflow admits two ordinary debits and rejects the third Slash without paying it.");

        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        bool TurnEnded() => Facts<TurnEndedEvent>(game).Contains(new(turn, 0));
        for (var step = 0; step < 35 && !TurnEnded() && P(game) is not { PlayerSeat: 0, Kind: DecisionKind.DiscardCards }; step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        var hand = game.CreateSnapshot(0).Players[0];
        Require(TurnEnded() && Facts<PhaseChangedEvent>(game).Any(fact => fact is { ActorSeat: 0, Phase: TurnPhase.Discard }) &&
            hand.Hp == 4 && hand.Hand.Select(card => card.Id).Order().SequenceEqual(held) &&
            !game.CardMovements.Skip(movementCount).Any(move => move.From == CardLocation.Hand(0) && move.Reason == CardMoveReasons.HandLimitDiscard) &&
            Facts<ShiZhHuiXiezhiOverflowEvent>(game).SequenceEqual([overflow]) && !Facts<ShiZhHuiSiziActivatedEvent>(game).Any(),
            "The actual Discard keeps the same five hand cards at four HP using only the original Xiezhi unit.");

        RequireSameReplay(game, GameReplay.Replay(game.CreateCheckpoint().Options, game.AcceptedCommands, content));

        void UseHealth(string activation)
        {
            Accept(game, new UseProgramSkillCommand(0, Driver, activation, [], [], game.Revision, P(game)!.PromptId));
            Settle(game);
        }
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static void RequireSameReplay(GameEngine game, GameEngine replay) =>
        Require(game.CardMovements.SequenceEqual(replay.CardMovements) && Facts<ShiZhHuiXiezhiOverflowEvent>(game).SequenceEqual(Facts<ShiZhHuiXiezhiOverflowEvent>(replay)) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(replay.CreateSnapshot(seat))),
            "Replay reproduces the original overflow, its physical effects and each seat's state.");

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-zhong-hui-xiezhi-numeric", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,"activations":[
                  {"id":"lose","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,
                   "effects":[{"op":"loseHp","target":"owner","amount":3}]},
                  {"id":"recover","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,
                   "effects":[{"op":"recover","target":"owner","amount":3}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "实变体力", description = "失去或回复3点体力。" } } }));
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Peer, "无技能同桌", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "挟志固定实体", 0, 2, [])
            { PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7), 24).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "挟志溢出", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
