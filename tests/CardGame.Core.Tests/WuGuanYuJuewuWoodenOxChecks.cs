using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class WuGuanYuJuewuWoodenOxChecks
{
    private const string Hero = "wumiao:guan-yu", Juewu = "wumiao:juewu", WoodenOx = "classic:wooden-ox";
    private const string Mode = "identity:classic-juewu-wooden-ox", Deck = "fixture:juewu-wooden-ox-deck";
    private static readonly string[] Generals = [Hero, "fixture:juewu-ox-peer-1", "fixture:juewu-ox-peer-2", "fixture:juewu-ox-peer-3"];

    internal static void JuewuUsesTheOriginalRankTwoGrainCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        ReachOpening();
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            ReachOpening();
        }

        var opening = game.CreateSnapshot(0).Players[0];
        Require(opening.GeneralId == Hero && opening.Hand.Count(card => card.Rank == 2) == 1,
            "The official Wu Guan Yu starts Play with one printed rank-two card.");
        var ox = opening.Hand.Single(card => card.Kind == CardKind.WoodenOx).Id;
        var grain = opening.Hand.Single(card => card is { Kind: CardKind.Dodge, Rank: 2, Suit: Suit.Heart }).Id;
        Accept(game, new PlayCardCommand(0, ox, [], game.Revision, P(game)!.PromptId));
        Settle(game);
        Accept(game, new UseEquipmentEffectCommand(0, CardKind.WoodenOx, [grain], [], game.Revision, P(game)!.PromptId));
        Settle(game);

        var stored = game.CardMovements.Single(move => move.CardId == grain && move.Reason == CardMoveReasons.WoodenOxStore);
        Require(stored.From == CardLocation.Hand(0) && stored.To == CardLocation.WoodenOxGrain(0),
            $"The native storage moves the original Dodge into the owner's grain. Actual move {JsonSerializer.Serialize(stored)}");
        var oxMoves = game.CardMovements.Where(move => move.CardId == ox).ToArray();
        Require(oxMoves.Any(move => move.From == CardLocation.Processing && move.To == CardLocation.Equipment(0) &&
            move.Reason == CardMoveReasons.EquipmentEnter),
            $"The original Ox enters its owner's equipment through native use. Actual moves {JsonSerializer.Serialize(oxMoves)}");
        var views = Enumerable.Range(0, game.PlayerCount).Select(seat => game.CreateSnapshot(seat).Players[0]).ToArray();
        Require(views[0].WoodenOxGrainCount == 1 && views[0].WoodenOxGrain is [var visible] && visible.Id == grain,
            $"The owner sees the exact face-down grain. Actual views {GrainViews(views)}");
        Require(views.Skip(1).All(view => view.WoodenOxGrainCount == 1 && view.WoodenOxGrain is { Count: 0 }),
            $"Each other viewer sees the face-down grain count alone. Actual views {GrainViews(views)}");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        RequireSameReplay(game, restored);
        game = restored;
        var slashes = game.GetLegalActions(0).Where(action => action is { Kind: LegalActionKind.Slash, PlayedCardKind: CardKind.Slash,
            ConversionSource: { SkillId: Juewu, BindingId: "rank-two-slash", OwnerSeat: 0 } }).ToArray();
        Require(slashes.Select(action => action.CardId).Distinct().SequenceEqual([grain]),
            "Only the original rank-two grain can pay the published Juewu Slash after cold restoration.");
        var slash = slashes.Single(action => action.TargetSeats.SequenceEqual([1]));
        Accept(game, new PlayCardCommand(0, grain, [1], game.Revision, P(game)!.PromptId, CardKind.Slash)
            { ConversionSource = slash.ConversionSource });
        Reach(game, prompt => prompt is { Kind: DecisionKind.RespondDodge, PlayerSeat: 1 });
        Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "take-damage");
        Settle(game);

        var issued = Facts<JuewuUseIssuedEvent>(game).Single();
        var paid = Facts<JuewuMaterialPaidEvent>(game).Single();
        var moves = game.CardMovements.Where(move => move.CardId == grain && move.Sequence > stored.Sequence).ToArray();
        Require(issued.CardId == grain && issued.Source == slash.ConversionSource && issued.PrintedKind == CardKind.Dodge &&
            issued.PrintedRank == 2 && issued.OutputKind == CardKind.Slash && issued.From == stored.To,
            $"The original Juewu receipt keeps the printed rank-two Dodge and effective Slash. Actual receipt {JsonSerializer.Serialize(issued)}");
        Require(moves is [var payment, var cleanup] && payment.From == stored.To && payment.To == CardLocation.Processing && payment.Reason == CardMoveReasons.Use &&
            cleanup.From == payment.To && cleanup.To == CardLocation.DiscardPile && cleanup.Reason == CardMoveReasons.UseFinished,
            $"The original grain pays and retires through the native Slash use. Actual moves {JsonSerializer.Serialize(moves)}");
        Require(paid == new JuewuMaterialPaidEvent(issued.UseId, issued.ActionId, grain, moves[0].From, moves[0].To, moves[0].Reason, moves[0].Sequence) &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == issued.UseId && fact.CardId == grain && fact.CardKind == CardKind.Slash) == 1 &&
            game.CreateSnapshot(0).Players[0] is { WoodenOxGrainCount: 0 } holder && holder.Equipment.Any(card => card.Id == ox),
            "The accepted Slash pays and retires that one original grain, while the Wooden Ox stays equipped.");

        RequireSameReplay(game, GameReplay.Replay(game.CreateCheckpoint().Options, game.AcceptedCommands, content));

        void ReachOpening() => Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral ||
            prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static string GrainViews(IReadOnlyList<PlayerSnapshot> views) => JsonSerializer.Serialize(views.Select((view, seat) =>
        new { ViewerSeat = seat, view.WoodenOxGrainCount, VisibleCardIds = view.WoodenOxGrain?.Select(card => card.Id) }));

    private static void RequireSameReplay(GameEngine game, GameEngine replay)
    {
        Require(replay.CardMovements.SequenceEqual(game.CardMovements) && Facts<JuewuMaterialPaidEvent>(replay).SequenceEqual(Facts<JuewuMaterialPaidEvent>(game)) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(replay.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Replay preserves the original private grain, public choices and exact native payment.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wu-guan-yu-juewu-wooden-ox", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            new StandardClassicGeneralPackage().Register(new TreasureOnlyBuilder(builder));
            foreach (var peer in Generals.Skip(1)) builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "绝武和木牛流马", 3, 0, []) { PhysicalCards = Enumerable.Range(1, 12).Select(index => index switch
            {
                6 => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2),
                11 => new ContentDeckPhysicalCard(WoodenOx, Suit.Club, 5),
                _ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)
            }).ToArray() });
            builder.AddMode(new(Mode, "绝武使用木牛流马里的实体", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class TreasureOnlyBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
    {
        public void AddCard(ContentCardDefinition card) { if (card.Id == WoodenOx) inner.AddCard(card); }
        public void AddDeck(ContentDeckRecipe deck) { }
        public void AddGeneral(ContentGeneralDefinition general) { }
        public void AddMode(ContentModeDefinition mode) { }
        public void AddSkill(ContentSkillDefinition skill) { }
    }
}
