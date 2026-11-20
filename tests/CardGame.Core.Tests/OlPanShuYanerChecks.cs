using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlPanShuYanerChecks
{
    private const string PanShu = "ol:pan-shu", Yaner = "ol:yaner", Zhiren = "ol:zhiren";
    private const string Mode = "identity:classic-ol-yaner-pairs", Deck = "fixture:ol-yaner-pairs-deck";
    private const string Draw = "skill-program.ol:yaner.yaner-draw";
    private static readonly string[] Generals = ["fixture:yaner-lord", "fixture:yaner-loser", PanShu, "fixture:yaner-peer"];

    internal static void YanerGrantsEachRealDrawPairItsOwnBenefitCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        AssertBenefits(content, 61, [CardKind.Slash, CardKind.Duel], [CardKind.Duel, CardKind.Duel], relaxZhiren: false, recover: true);
        AssertBenefits(content, 84, [CardKind.Duel, CardKind.Duel], [CardKind.Duel, CardKind.Slash], relaxZhiren: true, recover: false);
        AssertBenefits(content, 57, [CardKind.Slash, CardKind.Slash], [CardKind.Duel, CardKind.Duel], relaxZhiren: true, recover: true);
    }

    private static void AssertBenefits(ContentRegistry content, int seed, CardKind[] expectedOwn, CardKind[] expectedOther,
        bool relaxZhiren, bool recover)
    {
        var game = Start(content, seed);
        var full = game.CreateSnapshot(1).Players[1].Hp;
        var firstTurn = game.State.TurnNumber;
        Slash(game, 0, target: 1);
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Yaner });
        Require(game.State.TurnNumber == firstTurn && game.CreateSnapshot(0).Players[0].HandCount == 0,
            "The first Yaner offer belongs to the lord's own preceding last-hand movement.");
        Skip(game);
        Settle(game);
        Require(game.CreateSnapshot(1).Players[1].Hp == full - 1 &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 1 },
            "The last-hand player starts with real Slash damage from the previous play phase.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        var openingCard = game.CreateSnapshot(1).Players[1].Hand.Single();
        var turn = game.State.TurnNumber;

        Slash(game, 1, target: 0);
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Yaner });
        Require(game.CreateSnapshot(1).Players[1].HandCount == 0 && turn > firstTurn && game.State.TurnNumber == turn &&
            game.CardMovements.Any(move => move.CardId == openingCard.Id && move.From == CardLocation.Hand(1) &&
                move.To == CardLocation.Processing && move.TurnNumber == turn),
            "The real Slash paid the other player's last hand card in their actual play phase.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the original last-hand movement and Yaner prompt on every seat's projection.");
        Activate(restored);
        Reach(restored, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });

        var gains = restored.CardMovements.Where(move => move.Reason.Value == Draw).ToArray();
        Require(gains.Length == 4 && gains.All(move => move.From == CardLocation.DrawPile && move.TurnNumber == turn) &&
            gains.Where(move => move.To == CardLocation.Hand(2)).Select(move => move.CardKind).SequenceEqual(expectedOwn) &&
            gains.Where(move => move.To == CardLocation.Hand(1)).Select(move => move.CardKind).SequenceEqual(expectedOther),
            $"Seed {seed} must yield both actual Yaner draw pairs: {string.Join(", ", gains.Select(move => $"{move.To}:{move.CardKind}"))}.");

        var relaxations = Facts<ProgramBooleanStateChangedEvent>(restored).Count(entry =>
            entry is { OwnerSeat: 2, SkillId: Zhiren, StateId: "zhiren-unrestricted", Value: true });
        var recoveries = Facts<RecoveryAppliedEvent>(restored).Count(entry => entry is { SourceSeat: 2, TargetSeat: 1, Amount: 1 });
        Require(relaxations == (relaxZhiren ? 1 : 0) && recoveries == (recover ? 1 : 0) &&
            restored.CreateSnapshot(1).Players[1].Hp == full - (recover ? 0 : 1),
            $"Seed {seed}: each player's own two cards determine their Yaner benefit. Zhiren changes: {relaxations}, actual recoveries: {recoveries}.");
    }

    private static void Slash(GameEngine game, int seat, int target)
    {
        var action = game.GetLegalActions(seat).First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([target]));
        Accept(game, new PlayCardCommand(seat, action.CardId!.Value, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Start(ContentRegistry content, int seed)
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = seed, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }

        Require(game.CreateSnapshot(0).Players[0].Hand.Single().Kind == CardKind.Slash &&
            game.CreateSnapshot(1).Players[1].Hand.Single().Kind == CardKind.Slash,
            $"Seed {seed} needs an actual opening Slash on the first two seats.");
        return game;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-yaner-pairs", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, PanShu);
            foreach (var general in Generals.Where(id => id != PanShu))
                builder.AddGeneral(new(general, "同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "燕尔基本与锦囊", 1, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7), 8)
                    .Concat(Enumerable.Repeat(new ContentDeckPhysicalCard("standard:duel", Suit.Club, 1), 8)).ToArray()
            });
            builder.AddMode(new(Mode, "燕尔成对摸牌", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
