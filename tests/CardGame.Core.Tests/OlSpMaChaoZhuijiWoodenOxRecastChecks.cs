using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlSpMaChaoZhuijiWoodenOxRecastChecks
{
    private const string Hero = "ol:sp-ma-chao", Zhuiji = "ol:zhuiji", WoodenOx = "classic:wooden-ox";
    private const string Mode = "identity:classic-ol-zhuiji-wooden-ox", Deck = "fixture:zhuiji-wooden-ox-deck";
    private static readonly string[] Generals = ["fixture:zhuiji-ox-target", Hero, "fixture:zhuiji-ox-peer-2", "fixture:zhuiji-ox-peer-3"];

    public static void ZhuijiRecastsTheLoadedTreasureAndReturnsItsOriginalSlashCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        var hand = game.CreateSnapshot(0).Players[0].Hand;
        var ox = hand.First(card => card.Kind == CardKind.WoodenOx).Id;
        var grain = hand.First(card => card.Kind == CardKind.Duel).Id;
        Accept(game, new PlayCardCommand(0, ox, [], game.Revision, P(game)!.PromptId));
        Settle(game);

        CompleteRecast(Cold(game, content), content, ox, null);
        Accept(game, new UseEquipmentEffectCommand(0, CardKind.WoodenOx, [grain], [], game.Revision, P(game)!.PromptId));
        Settle(game);
        Require(game.CreateSnapshot(0).Players[0].WoodenOxGrain!.Single().Id == grain &&
            game.CardMovements.Single(move => move.CardId == grain && move.Reason == CardMoveReasons.WoodenOxStore).To == CardLocation.WoodenOxGrain(0),
            "The loaded branch used the public Wooden Ox command to store a real hand card before the other player's turn.");
        CompleteRecast(game, content, ox, grain);
    }

    private static void CompleteRecast(GameEngine game, ContentRegistry content, int ox, int? grain)
    {
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        var turn = game.State.TurnNumber;
        var slash = game.GetLegalActions(1).First(action => action.Kind == LegalActionKind.Slash && action.CardId is > 0 &&
            action.ConversionSource is null && action.TargetSeats.SequenceEqual([0]));
        Accept(game, new PlayCardCommand(1, slash.CardId!.Value, slash.TargetSeats, game.Revision, P(game)!.PromptId, slash.PlayedCardKind));
        Reach(game, prompt => prompt is { PlayerSeat: 0, IsPrivate: true, SkillPrompt.SkillId: Zhuiji } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("penalty-option") == "recast"));
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == slash.CardId);
        var offered = Facts<SlashTargetPenaltyOfferedEvent>(game).Single().Identity;
        var target = game.CreateSnapshot(0).Players[0];
        Require(use is { SourceSeat: 1, CardKind: CardKind.Slash } && use.TargetSeats.SequenceEqual([0]) &&
            offered is { ActorSeat: 1, TargetSeat: 0, FrozenDistance: 1 } && offered.CardUseFrameId == use.Id && offered.ActualTurnNumber == turn &&
            target.Equipment.Single().Id == ox && (target.WoodenOxGrain ?? []).Select(card => card.Id).SequenceEqual(grain is { } id ? [id] : []) &&
            !Facts<SlashTargetPenaltyPaidEvent>(game).Any(),
            "An original adjacent physical Slash offers the target a real one-equipment recast while its original ox and grain are still in place.");

        game = Cold(game, content);
        var before = game.CardMovements.Count;
        Answer(game, choice => choice.Parameters.GetValueOrDefault("penalty-option") == "recast");
        for (var step = 0; step < 80 && !Facts<CardUseFinishedEvent>(game).Any(entry => entry.ResolutionId == use.Id); step++)
        {
            if (P(game) is { Kind: DecisionKind.RespondDodge })
                Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
            else
                Accept(game, new AdvanceOneStepCommand(game.Revision));
        }

        var moves = game.CardMovements.Skip(before).ToArray();
        var paid = Facts<SlashTargetPenaltyPaidEvent>(game).Single();
        var cost = moves.Single(move => move.Reason == CardMoveReasons.RecastDiscard);
        Require(paid is { ActorSeat: 1, TargetSeat: 0, Recast: true } && paid.CardUseFrameId == use.Id && paid.CardIds.SequenceEqual([ox]) &&
            cost.CardId == ox && cost.BatchId is > 0 && cost.From == CardLocation.Equipment(0) && cost.To == CardLocation.DiscardPile,
            "The target pays exactly one real equipment card as the original Slash's recast cost.");
        var drops = moves.Where(move => move.Reason == CardMoveReasons.WoodenOxGrainDiscard).ToArray();
        Require(grain is { } stored ? drops is [{ CardId: var card }] && card == stored && drops[0].BatchId is > 0 &&
            drops[0].BatchId != cost.BatchId && drops[0].Sequence > cost.Sequence &&
            drops[0].From == CardLocation.WoodenOxGrain(0) && drops[0].To == CardLocation.DiscardPile : drops.Length == 0,
            "Only a loaded treasure issues a separate true grain departure, with no extra recast material.");
        Require(moves.Single(move => move.Reason == CardMoveReasons.RecastDraw).To == CardLocation.Hand(0) &&
            Facts<SlashTargetPenaltyDrawIssuedEvent>(game).Single() is { RequestedCount: 1, ActualCount: 1, TargetSeat: 0 } &&
            Facts<CardRecastEvent>(game).Single() is { ActorSeat: 0, DrawCount: 1, CardKind: CardKind.WoodenOx } recast && recast.CardId == ox &&
            Facts<SlashTargetPenaltyFinishedEvent>(game).Single() is { Completed: true, Recast: true, ActualDrawCount: 1 } finished && finished.CardUseFrameId == use.Id &&
            Facts<CardUseFinishedEvent>(game).Count(entry => entry.ResolutionId == use.Id) == 1,
            $"The original equipment pays for one actual draw and the same native Slash completes once. Stored card: {grain}.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold recovery preserves the equipment and private target-owned payment on the original use.");
        return restored;
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Start(ContentRegistry content)
    {
        for (var seed = 47; seed < 55; seed++)
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
            var target = game.CreateSnapshot(0).Players[0].Hand;
            if (target.Any(card => card.Kind == CardKind.WoodenOx) && target.Any(card => card.Kind == CardKind.Duel) &&
                game.CreateSnapshot(1).Players[1].Hand.Any(card => card.Kind == CardKind.Slash))
                return game;
        }
        throw new InvalidOperationException("The Zhuiji fixture needs an initial Wooden Ox and Duel at seat zero and a physical Slash at seat one.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-zhuiji-wooden-ox", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            new StandardClassicGeneralPackage().Register(new TreasureOnlyBuilder(builder));
            foreach (var peer in Generals.Where(id => id != Hero))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "真木牛、决斗与杀", 7, 0, []) { PhysicalCards = Enumerable.Range(0, 63)
                .Select(index => (index % 3) switch
                {
                    0 => new ContentDeckPhysicalCard(WoodenOx, Suit.Diamond, 5),
                    1 => new ContentDeckPhysicalCard("standard:duel", Suit.Spade, 1),
                    _ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)
                }).ToArray() });
            builder.AddMode(new(Mode, "追击真实装备重铸", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class TreasureOnlyBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
    {
        public void AddCard(ContentCardDefinition card)
        {
            if (card.Id == WoodenOx) inner.AddCard(card);
        }

        public void AddGeneral(ContentGeneralDefinition general) { }
        public void AddSkill(ContentSkillDefinition skill) { }
        public void AddDeck(ContentDeckRecipe deck) { }
        public void AddMode(ContentModeDefinition mode) { }
    }
}
