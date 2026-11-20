using CardGame.Content.Standard;
using CardGame.Core;

internal static class LeCaiWenJiShuangjiaDistanceOwnerChecks
{
    private const string Hero = "le:cai-wen-ji", Mark = "shuangjia";
    private const string Mode = "identity:shuangjia-distance-owner", Deck = "fixture:shuangjia-distance-deck";
    private static string Peer(int index) => $"fixture:shuangjia-distance-peer-{index}";

    internal static void MarkedEquipmentContributesOnlyWhileOwnedCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, ConfirmOpeningHand = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 5
        }, registry);
        Submit(game, new StartGameCommand());
        var play = ReachPlay(game, selectGeneral: true);

        var opening = Player(game).Hand;
        var originalMarks = Marks(game);
        Require(opening.Count == 4 && opening.All(card => card.Kind == CardKind.Crossbow && IsMarked(card)) &&
            originalMarks.Length == 4 && originalMarks.Select(mark => mark.CardId).ToHashSet().SetEquals(opening.Select(card => card.Id)) &&
            game.GetCombatDistance(1, 0) == 5 && game.GetCombatDistance(0, 1) == 1,
            "The real opening marks four physical hand entities and adds four only to incoming distance.");

        var installed = opening[0].Id;
        Equip(game, play, installed);
        play = ReachPlay(game);
        Require(Player(game).Equipment is [var weapon] && weapon.Id == installed && IsMarked(weapon) && Player(game).Hand.Count == 3 &&
            game.GetCombatDistance(1, 0) == 5 && game.GetCombatDistance(0, 1) == 1,
            "An original marked entity installed through a real card use still contributes to owned incoming distance.");

        var before = game;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(before.CreateCheckpoint())), registry);
        Require(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(before.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(before.CardMovements) && Marks(game).SequenceEqual(originalMarks) &&
            game.GetCombatDistance(1, 0) == 5 && game.GetCombatDistance(0, 1) == 1,
            "Cold restore preserves the marked equipment's native installation, the original marks and direction.");

        var replacement = opening[1].Id;
        play = game.CreateSnapshot(0).PendingDecision!;
        Equip(game, play, replacement);
        ReachPlay(game);
        Require(Player(game).Equipment is [var newWeapon] && newWeapon.Id == replacement && IsMarked(newWeapon) &&
            Player(game).Hand.Count(card => IsMarked(card)) == 2 && Marks(game).SequenceEqual(originalMarks) &&
            game.CardMovements.Count(move => move.CardId == installed && move.From == CardLocation.Equipment(0) &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.EquipmentReplace) == 1 &&
            game.GetCombatDistance(1, 0) == 4 && game.GetCombatDistance(0, 1) == 1,
            "A native replacement moves the original marked weapon to discard and reduces only incoming distance by one.");
    }

    private static void Equip(GameEngine game, PendingDecision play, int cardId)
    {
        var action = game.GetLegalActions(0).Single(move => move.CardId == cardId && move.Kind == LegalActionKind.Equip && move.ConversionSource is null);
        Submit(game, new PlayCardCommand(0, cardId, action.TargetSeats, game.Revision, play.PromptId));
        Require(game.CardMovements.Count(move => move.CardId == cardId && move.From == CardLocation.Hand(0) &&
            move.To == CardLocation.Processing && move.Reason == CardMoveReasons.EquipmentUse) == 1,
            "The public equipment command pays exactly its original hand entity.");
    }

    private static PlayerSnapshot Player(GameEngine game) => game.CreateSnapshot(0).Players[0];
    private static bool IsMarked(CardSnapshot card) => card.SkillMarkLabel?.Contains("胡笳", StringComparison.Ordinal) == true;
    private static CardEntityMarkedEvent[] Marks(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<CardEntityMarkedEvent>()
        .Where(mark => mark.InitialOwnerSeat == 0 && mark.Mark == Mark).ToArray();

    private static PendingDecision ReachPlay(GameEngine game, bool selectGeneral = false)
    {
        for (var step = 0; step < 100; step++)
        {
            var prompt = game.CreateSnapshot(0).PendingDecision;
            if (prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && game.ResolutionStack.Count == 0)
                return prompt;
            if (selectGeneral && prompt is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral })
            {
                Submit(game, new SelectGeneralCommand(0, Hero, game.Revision, prompt.PromptId));
                continue;
            }

            Require(prompt is null && game.State.Winner == Winner.None, $"Unexpected Shuangjia fixture boundary: {prompt?.Kind}.");
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The Shuangjia fixture did not reach its original Play phase.");
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The Shuangjia fixture command failed.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:le-cai-wen-ji-shuangjia-distance", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            for (var index = 1; index < 4; index++)
                builder.AddGeneral(new(Peer(index), "同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "双笳固定原装备", 4, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, 1), 32).ToArray()
            });
            builder.AddMode(new(Mode, "双笳实际方向距离", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Hero, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
