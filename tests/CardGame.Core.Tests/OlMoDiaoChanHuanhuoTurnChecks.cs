using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;
using static BoundaryLiDianChecks;

internal static class OlMoDiaoChanHuanhuoTurnChecks
{
    private const string Huanhuo = "ol:mo-huanhuo", Hero = "ol:mo-diao-chan";
    private const string Giver = "fixture:huanhuo-giver", Peer = "fixture:huanhuo-peer", Deck = "fixture:huanhuo-deck";

    internal static void PaidPromiseWaitsForRecipientActualPlayCold()
    {
        const int recipient = 0, giver = 2;
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "mo-diao-chan-huanhuo", Peer, 4, 0, 0, 0, 0, [],
            [new(CardKind.DrawTwo, Suit.Club, 7), new(CardKind.Dodge, Suit.Club, 7),
                new(CardKind.Dodge, Suit.Spade, 7), new(CardKind.Dodge, Suit.Club, 7)], [],
            [new(Giver) { StartingHand = [new(CardKind.Dodge, Suit.Club, 7), new(CardKind.Dodge, Suit.Spade, 7)] },
                new(Peer) { StartingHand = [] }], [new(Peer) { StartingHand = [] }], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = recipient, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12
        }, content);

        Accept(game, new StartGameCommand());
        Settle(game);
        var drawTwo = game.CreateSnapshot(recipient).Players[recipient].Hand.Single(card => card.Kind == CardKind.DrawTwo);
        var promise = Facts<MoDiaoChanPromiseEvent>().Single(e => e.TargetSeat == recipient);
        var issuingStarts = Facts<TurnStartedEvent>();
        var costs = Facts<MoDiaoChanCostEvent>().Where(e => e.FrameId == promise.FrameId).ToArray();
        Require(promise.Source is { OwnerSeat: giver, SkillId: Huanhuo, BindingId: "round-draw-and-select" } &&
            promise.AfterTurnStartOrdinal == issuingStarts.Length && issuingStarts.Last().ActorSeat == recipient &&
            costs.Length == 2 && costs.Select(e => e.CardId).Distinct().Count() == 2 && costs.All(e => Paid(e, giver)) &&
            DueTurn() is null && P(game)!.ForcedHandSelection is null,
            "The real round-start payer discards two physical cards and issues the promise after the recipient's current actual turn has already started.");

        Accept(game, new EndPlayPhaseCommand(recipient, game.Revision, P(game)!.PromptId));
        for (var step = 0; step < 100 && Facts<TurnStartedEvent>().Length == issuingStarts.Length; step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        var foreignStarts = Facts<TurnStartedEvent>().Skip(promise.AfterTurnStartOrdinal).ToArray();
        Require(foreignStarts.Length > 0 && foreignStarts.All(turn => turn.ActorSeat != recipient) && DueTurn() is null &&
            Facts<MoDiaoChanLockEvent>().All(e => e.ActorSeat != recipient),
            "A real foreign TurnStarted leaves the original recipient's paid promise waiting.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Facts<MoDiaoChanPromiseEvent>().Single(e => e.PromiseId == promise.PromiseId) == promise && DueTurn() is null,
            "Cold restore keeps the same pending promise through a foreign actual turn.");
        Settle(game);
        var recipientStart = Facts<TurnStartedEvent>().Skip(promise.AfterTurnStartOrdinal).First(turn => turn.ActorSeat == recipient);
        var selection = P(game)!.ForcedHandSelection;
        Require(recipientStart.TurnNumber == game.State.TurnNumber && DueTurn() == recipientStart.TurnNumber &&
            selection is { SkillId: Huanhuo, RemainingUses: 2 } && selection.CardId == drawTwo.Id,
            "The first subsequent actual recipient Play locks the same original physical DrawTwo.");

        var action = game.GetHumanLegalActions().Single(entry => entry.CardId == drawTwo.Id);
        Accept(game, new PlayCardCommand(recipient, drawTwo.Id, action.TargetSeats, game.Revision, P(game)!.PromptId,
            action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource });
        Settle(game);
        var claim = Facts<MoDiaoChanUseClaimedEvent>().Single(e => e.Claim.PromiseId == promise.PromiseId).Claim;
        var settled = Facts<MoDiaoChanUseSettledEvent>().Single(e => e.PromiseId == promise.PromiseId);
        var discarded = Facts<MoDiaoChanRandomDiscardEvent>().Single(e => e.FrameId == settled.FrameId);
        var discardCost = Facts<MoDiaoChanCostEvent>().Single(e => e.FrameId == settled.FrameId);
        Require(claim is { ActorSeat: recipient, ProviderSeat: recipient, Ordinal: 1, Kind: CardKind.DrawTwo } &&
            claim.ActualTurn == recipientStart.TurnNumber && claim.ActionId == settled.ActionId &&
            Facts<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == claim.ActionId && e.Action.PhysicalCards.Any(c => c.CardId == drawTwo.Id)) &&
            discarded.ActorSeat == recipient && discarded.CardId == discardCost.CardId && Paid(discardCost, recipient),
            "The original promise settles the actual physical card use once and pays its random discard once on the matching actual turn.");

        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        int? DueTurn() => game.GetRuntimeModule<CharacterDiaoChanOlMagicRuntime>().MoDcDueTurn(promise);
        bool Paid(MoDiaoChanCostEvent cost, int seat) => cost.From == CardLocation.Hand(seat) && cost.To == CardLocation.DiscardPile &&
            game.CardMovements.Count(move => move.Sequence == cost.Sequence && move.CardId == cost.CardId &&
                move.From == cost.From && move.To == cost.To && move.Reason.Value == "skill-program.ol:mo-huanhuo.discard") == 1;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-mo-diao-chan-huanhuo-turn", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Giver, "幻惑授予者", "", Huanhuo, "wei", 8));
            builder.AddGeneral(new(Peer, "固定同桌", "", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "幻惑固定牌堆", 0, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "幻惑实际回合验证", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Giver, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
