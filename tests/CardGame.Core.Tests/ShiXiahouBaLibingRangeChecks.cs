using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ShiXiahouBaLibingRangeChecks
{
    private const string Hero = "mobile:shi-xiahou-ba", Libing = "mobile:shi-libing";
    private const string Mode = "identity:classic-libing-range", Deck = "fixture:libing-range-deck";
    private static readonly string[] Generals = [Hero, "fixture:libing-neighbor", "fixture:libing-target", "fixture:libing-peer"];

    internal static void LibingEnablesAnOrdinaryDistantSlashUntilItCompletesCold()
    {
        var stock = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2), 36).ToArray();
        var probe = Start(ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock)));
        var opening = probe.CreateSnapshot(0).Players[0].Hand;
        var study = opening[0].Id;
        var slash = opening[1].Id;
        stock[study - 1] = new("standard:draw_two", Suit.Heart, 7);
        stock[slash - 1] = new("standard:slash", Suit.Spade, 5);
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock));
        var game = Start(content);

        Require(game.GetAttackRange(0) == 1 && game.GetCombatDistance(0, 2) == 2 &&
            SlashActions(game, slash).Any(action => action.TargetSeats.SequenceEqual([1])) &&
            SlashActions(game, slash).All(action => !action.TargetSeats.Contains(2)),
            "The original ordinary hand Slash is playable against a neighbor, but the opposite target is outside base range.");
        var draw = game.GetLegalActions(0).Single(action => action.CardId == study && action.Kind == LegalActionKind.DrawTwo);
        Play(game, draw);
        Settle(game);
        var turn = game.State.TurnNumber;
        var drawUse = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.CardId == study);
        var increase = Facts<ShiXbLibbingAdvancedEvent>(game).Single();
        Require(increase is { Option: 1, Count1: 1, Count2: 0, Count3: 0, Source: { OwnerSeat: 0, SkillId: Libing } } &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == drawUse.ResolutionId && fact.CardKind == CardKind.DrawTwo) == 1 &&
            game.CardMovements.Count(move => move.CardId == study && move.Reason == CardMoveReasons.Use &&
                move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
            game.GetAttackRange(0) == 2 && game.GetCombatDistance(0, 2) == 2,
            "The real non-damage use pays and settles once, and the first Libing option adds one finite range without changing distance.");

        game = Cold(game, content);
        Play(game, SlashActions(game, slash).Single(action => action.TargetSeats.SequenceEqual([2])));
        Reach(game, prompt => prompt is { Kind: DecisionKind.RespondDodge, PlayerSeat: 2 });
        var attack = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == slash);
        Require(attack is { SourceSeat: 0, CardKind: CardKind.Slash,
                Action: { Type: CardActionType.Use, ActorSeat: 0, EffectiveKind: CardKind.Slash } action } &&
            action.ConversionChain.Count == 0 && action.PhysicalCards.Single().CardId == slash &&
            attack.TargetSeats.SequenceEqual([2]) && game.GetAttackRange(0) == 2 &&
            Facts<ShiXbLibbingResetEvent>(game).Length == 0,
            "The opposite target receives the ordinary paid Slash, and its pending defense does not reset Libing early.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "take-damage");
        Settle(game);
        var reset = Facts<ShiXbLibbingResetEvent>(game).Single();
        Require(reset.Source == increase.Source && reset.CardActionId == attack.Action!.ActionId && reset.UsedKind == nameof(CardKind.Slash) &&
            game.State.TurnNumber == turn && game.GetAttackRange(0) == 1 && game.GetCombatDistance(0, 2) == 2 &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 2, Amount: 1 } damage &&
            damage.SourceCardUseResolutionId == attack.Id &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == attack.Id && fact.CardId == slash) == 1 &&
            Facts<ShiXbPoxiNativeIssuedEvent>(game).Length == 0,
            "Completing the same ordinary damage-card use resets the range within that Play phase, with no Poxi permission.");
        Require(Cold(game, content).GetAttackRange(0) == 1, "The reset range also survives cold command replay.");
    }

    private static IEnumerable<LegalAction> SlashActions(GameEngine game, int cardId) =>
        game.GetLegalActions(0).Where(action => action.CardId == cardId && action.Kind == LegalActionKind.Slash && action.ConversionSource is null);

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static void Play(GameEngine game, LegalAction action) =>
        Accept(game, new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            game.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType())).SequenceEqual(
                restored.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType()))) &&
            Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the actual range-producing card, its effect and the available native targets.");
        return restored;
    }

    private static GameEngine Start(ContentRegistry content)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
            HumanRole = Role.Lord, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The fixed identity setup gives Xiahou Ba the first actual Play.");
        return game;
    }

    private sealed class Fixture(IReadOnlyList<ContentDeckPhysicalCard> stock) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-xiahou-ba-libing-range", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            foreach (var peer in Generals.Skip(1))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定厉兵实体牌", 4, 2, []) { PhysicalCards = stock });
            builder.AddMode(new(Mode, "厉兵实际攻击范围", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
