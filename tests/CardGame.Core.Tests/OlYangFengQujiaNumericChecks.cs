using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class OlYangFengQujiaNumericChecks
{
    private const string Hero = "ol:yang-feng", Qujia = "ol:qujia";
    private const string Peer = "fixture:qujia-numeric-peer", Deck = "fixture:qujia-numeric-deck";
    private const string Gain = "skill-program.yang-feng.qujia.gain";

    internal static void CompletedTurnGrantPaysTwoNativeSlashesCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        ShanheStartingCard[] opening = [new(CardKind.Slash, Suit.Spade, 3), new(CardKind.Slash, Suit.Heart, 4)];
        var setup = new ShanheBattleSetup(39, 1, "qujia-numeric", Hero, 4, 0, 0, 0, 0,
            [], opening, [], [new(Peer) { StartingHand = [] }], [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, content);
        Accept(game, new StartGameCommand());
        Settle(game);

        var firstTurn = Facts<TurnStartedEvent>(game).Single().TurnNumber;
        var hand = game.CreateSnapshot(0).Players[0].Hand;
        var original = hand.Where(card => card.Kind == CardKind.Slash).Select(card => card.Id).ToArray();
        var deckSlash = game.CreateCardZoneDiagnostics().Single(card => card.Location == CardLocation.DrawPile && card.CardKind == CardKind.Slash).CardId;
        Require(hand.Count == 2 && original.Length == 2, "Yang Feng starts with two ordinary Slashes and the only remaining Slash is in the draw pile.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Qujia } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, IsPrivate: true, SkillPrompt.SkillId: Qujia } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "quota"));
        var ended = game.ResolutionStack.OfType<DeferredTurnEndFrame>().Single();
        var pending = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Qujia);
        var started = Facts<YangFengStartedEvent>(game).Single(fact => fact.Op == SkillProgramEffectOp.ResolveQujiaLimitedTurnEnd);
        Require(ended is { OwnerSeat: 0 } && ended.TurnNumber == firstTurn && started.ParentId == ended.Id && started.FrameId == pending.Id &&
            pending.WindowContext?.Facts is { OwnerKilledThisTurn: false } && Facts<TurnEndedEvent>(game).Count(fact =>
                fact.ActorSeat == 0 && fact.TurnNumber == firstTurn) == 1 && !Facts<QujiaChoiceIssuedEvent>(game).Any(),
            "The unpaid choice is still owned by the original completed no-kill turn.");

        var cold = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        RequireSameReplay(game, cold);
        game = cold;
        Answer(game, choice => choice.Parameters.GetValueOrDefault("branch") == "quota");
        Settle(game);
        var nextTurn = Facts<TurnStartedEvent>(game).Single(fact => fact.ActorSeat == 0 && fact.TurnNumber > firstTurn).TurnNumber;
        var choice = new QujiaChoiceIssuedEvent(started.FrameId, started.Source, started.GameplayHash, firstTurn, ended.Id, "quota");
        var grant = new QujiaSlashBonusGrantedEvent(started.FrameId, started.Source, started.GameplayHash, firstTurn);
        var gain = game.CardMovements.Single(move => move.Reason.Value == Gain);
        Require(game.State.TurnNumber == nextTurn && gain.CardId == deckSlash && gain.CardKind == CardKind.Slash &&
            gain.From == CardLocation.DrawPile && gain.To == CardLocation.Hand(0) &&
            Facts<QujiaChoiceIssuedEvent>(game).SequenceEqual([choice]) && Facts<QujiaSlashBonusGrantedEvent>(game).SequenceEqual([grant]),
            "The original quota choice gains the one physical deck Slash and issues one permanent allowance before the next actual Play.");

        foreach (var id in new[] { deckSlash, original[0] })
        {
            var action = game.GetLegalActions(0).Single(item => item.CardId == id && item.Kind == LegalActionKind.Slash &&
                item.ConversionSource is null && item.TargetSeats.SequenceEqual([1]));
            Accept(game, new PlayCardCommand(0, id, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
            Settle(game);
            var use = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.CardId == id && fact.SourceSeat == 0);
            Require(game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use && move.TurnNumber == nextTurn) == 1 &&
                Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == use.ResolutionId && fact.CardId == id && fact.CardKind == CardKind.Slash) == 1,
                "The original gained Slash and the ordinary hand Slash each pay and finish once in the later Play.");
        }

        var third = original[1];
        var revision = game.Revision;
        var movementCount = game.CardMovements.Count;
        var rejected = game.Submit(new PlayCardCommand(0, third, [1], revision, P(game)!.PromptId, CardKind.Slash));
        Require(!rejected.Accepted && game.Revision == revision && game.CardMovements.Count == movementCount &&
            game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == third) && !game.GetLegalActions(0).Any(action => action.CardId == third) &&
            Facts<CardUseDebitRecordedEvent>(game).Count(fact => fact.Debit is { ActorSeat: 0, Query: SkillRuleQuery.SlashLimit, Phase: TurnPhase.Play } && fact.Debit.TurnNumber == nextTurn) == 2 &&
            Facts<QujiaChoiceIssuedEvent>(game).SequenceEqual([choice]) && Facts<QujiaSlashBonusGrantedEvent>(game).SequenceEqual([grant]) &&
            game.CardMovements.Count(move => move.Reason.Value == Gain) == 1,
            "Qujia admits exactly two ordinary debits. The third Slash stays in hand, and the original choice, gain and allowance remain single.");
        RequireSameReplay(game, GameReplay.Replay(game.CreateCheckpoint().Options, game.AcceptedCommands, content));
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static void RequireSameReplay(GameEngine game, GameEngine replay) =>
        Require(replay.CardMovements.SequenceEqual(game.CardMovements) && Facts<QujiaChoiceIssuedEvent>(replay).SequenceEqual(Facts<QujiaChoiceIssuedEvent>(game)) &&
            Facts<QujiaSlashBonusGrantedEvent>(replay).SequenceEqual(Facts<QujiaSlashBonusGrantedEvent>(game)) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(replay.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Replay preserves the original quota choice, issued allowance and paid physical cards.");

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-yang-feng-qujia-numeric", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Peer, "同桌", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "驱驾固定实体", 0, 0, []) { PhysicalCards =
                [new("standard:slash", Suit.Club, 7), .. Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 8), 23)] });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "驱驾次数", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
