using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class LuoyingNativeDiscardChecks
{
    private const string CaoXian = "ol:cao-xian", Lingxi = "ol:lingxi", Zhifou = "ol:zhifou";
    private const string CaoZhi = "classic:cao-zhi", Luoying = "classic:luoying";
    private const string Xunshi = "anniversary:xunshi", Peer = "fixture:luoying-native-discard-peer";
    private const string Deck = "fixture:luoying-native-discard-deck";
    private const string Discard = "skill-program.ol:lingxi-or-zhifou.discard";

    public static void ZhifouOffersOnlyTheActualClubDiscard()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(373, 1, "luoying-zhifou", CaoXian, 3, 0, 0, 0, 0,
            [Xunshi], [new(CardKind.Dodge, Suit.Heart, 1), new(CardKind.Dodge, Suit.Heart, 2),
                new(CardKind.DrawTwo, Suit.Spade, 3), new(CardKind.IronChain, Suit.Club, 4), new(CardKind.Duel, Suit.Club, 5)],
            [], [new(CaoZhi, Hp: 3) { StartingHand = [] }, new(Peer, Hp: 6) { StartingHand = [] },
                new(Peer, Hp: 6) { StartingHand = [] }], [], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);

        Accept(game, new StartGameCommand());
        Reach(game, p => p.SkillPrompt?.SkillId == Lingxi && p.Choices.Any(c => Action(c, "activate")));
        var hand = game.CreateSnapshot(0).Players[0].Hand;
        var chain = hand.Single(c => c.Kind == CardKind.IronChain);
        var duel = hand.Single(c => c.Kind == CardKind.Duel);
        var wings = hand.Where(c => c.Kind == CardKind.Dodge && c.Suit == Suit.Heart).Select(c => c.Id).ToArray();
        Require(chain is { IdentityKind: CardKind.Slash, Suit: Suit.None } && duel.Suit == Suit.Club && wings.Length == 2,
            "The native Xunshi identity changes the hand Iron Chain to a suitless Slash and leaves Duel a Club.");

        Answer(game, c => Action(c, "activate"));
        ReachStage(game, CaoXianStage.StoreCards);
        foreach (var wing in wings) Answer(game, c => c.Cards.SequenceEqual([wing]));
        Answer(game, c => c.Parameters.GetValueOrDefault("branch") == "finish");
        Settle(game);
        var draw = game.GetHumanLegalActions().Single(a => a.Kind == LegalActionKind.DrawTwo && a.ConversionSource is null);
        Accept(game, new PlayCardCommand(0, draw.CardId!.Value, draw.TargetSeats, game.Revision, P(game)!.PromptId));
        Reach(game, p => p.SkillPrompt?.SkillId == Zhifou && p.Choices.Any(c => Action(c, "activate")));
        Answer(game, c => Action(c, "activate"));
        ReachStage(game, CaoXianStage.RemoveCards);
        Answer(game, c => c.Cards.SequenceEqual([wings[0]]));
        Answer(game, c => c.Parameters.GetValueOrDefault("branch") == "finish");
        ReachStage(game, CaoXianStage.AdjustCards);
        var required = game.CreateSnapshot(0).PendingDecision!.RequiredCardCount;
        Require(required > 0, "Lingxi's projected adjustment must publish the unpaid discard count.");
        for (var remaining = required; remaining > 0; remaining--)
            Answer(game, c => c.Cards is [var id] && id != chain.Id && id != duel.Id);
        ReachStage(game, CaoXianStage.OptionTarget);
        Answer(game, c => c.Parameters.GetValueOrDefault("option") == "2" && c.Targets.SequenceEqual([0]));
        Reach(game, _ => game.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()?.CaoXian is
            { Stage: CaoXianStage.EffectCards, Option: 2 });
        Answer(game, c => c.Cards.SequenceEqual([chain.Id]));

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(restored.CreateSnapshot(0).PendingDecision is { PlayerSeat: 0, SkillPrompt.SkillId: Zhifou, RequiredCardCount: 1 } &&
                !restored.CardMovements.Any(m => m.Reason.Value == Discard && (m.CardId == chain.Id || m.CardId == duel.Id)),
            "Cold replay before the last choice must leave both selected materials unpaid.");
        var originalResolution = FinishZhifouAndResolveLuoying(game, chain.Id, duel.Id);
        var restoredResolution = FinishZhifouAndResolveLuoying(restored, chain.Id, duel.Id);
        Require(originalResolution == restoredResolution && game.CardMovements.SequenceEqual(restored.CardMovements) &&
                Enumerable.Range(0, game.PlayerCount).All(seat =>
                    SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Resuming from before the final Zhifou payment must reproduce the same AI Luoying resolution, movements and views.");
    }

    private static ProgramBindingResolvedEvent FinishZhifouAndResolveLuoying(GameEngine game, int chain, int duel)
    {
        Answer(game, c => c.Cards.SequenceEqual([duel]));
        var chainDiscard = DiscardOf(game, chain);
        var duelDiscard = DiscardOf(game, duel);
        var material = Events<NativeLogicalDiscardMaterialCompletedEvent>(game).Single(e => e.CompletionSequence == duelDiscard.Sequence);
        var chainMaterial = Events<NativeLogicalDiscardMaterialCompletedEvent>(game).Single(e => e.CompletionSequence == chainDiscard.Sequence);
        var invoice = Events<NativeLogicalDiscardInvocationRecordedEvent>(game).Single(e => e.InvocationId == material.InvocationId);
        Require(invoice is { InitiatorSeat: 0, ExecutorSeat: 0, ActualCount: 2 } && chainMaterial.InvocationId == invoice.InvocationId &&
                material.CardId == duel && chainMaterial.CardId == chain &&
                chainDiscard.BatchId > 0 && chainDiscard.BatchId == duelDiscard.BatchId &&
                new[] { chainDiscard, duelDiscard }.All(m => m.PrintedSuit == Suit.Club && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile),
            "Zhifou's real second option must discard both printed Clubs in one original logical and physical batch.");

        Settle(game);
        var origins = Events<ProgramDiscardedEntityOriginEvent>(game)
            .Where(e => e.MovementSequence == chainDiscard.Sequence || e.MovementSequence == duelDiscard.Sequence).ToArray();
        Require(origins.Length == 2 &&
                origins.Contains(new ProgramDiscardedEntityOriginEvent(chainDiscard.Sequence, 0, Suit.None, null)) &&
                origins.Contains(new ProgramDiscardedEntityOriginEvent(duelDiscard.Sequence, 0, Suit.Club, null)),
            "The two original Zhifou entries must retain the source and distinct effective suits of Iron Chain and Duel.");
        var resolutions = Events<ProgramBindingResolvedEvent>(game).Where(e => e.SkillId == Luoying).ToArray();
        Require(resolutions is [{ OwnerSeat: 1, Window: SkillProgramTriggerWindow.DiscardPileReceived }] &&
                game.CreateCardZoneDiagnostics().Single(c => c.CardId == chain).Location == CardLocation.DiscardPile,
            "The AI must resolve the one original Luoying opportunity and leave the suitless entity in the discard pile.");
        var resolution = resolutions[0];
        var claim = Events<ProgramDiscardedEntityClaimedEvent>(game).SingleOrDefault();
        Require(resolution.Activated
            ? claim is { Origin: { OwnerSeat: 1, SkillId: Luoying } } && claim.CardId == duel &&
                claim.FrameId == resolution.FrameId && claim.DiscardMovementSequence == duelDiscard.Sequence
            : claim is null && resolution.FrameId == duelDiscard.BatchId,
            "Accepting must claim exactly the original Duel entry, while declining must settle that same batch's opportunity.");
        return resolution;
    }

    private static void ReachStage(GameEngine game, CaoXianStage stage) => Reach(game, _ =>
        game.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()?.CaoXian?.Stage == stage);

    private static bool Action(PromptChoice choice, string action) => choice.Parameters.GetValueOrDefault("program-action") == action;

    private static T[] Events<T>(GameEngine game) where T : IGameEvent =>
        game.Events.Select(e => e.Payload).OfType<T>().ToArray();

    private static CardMovementRecord DiscardOf(GameEngine game, int cardId) =>
        game.CardMovements.Single(m => m.CardId == cardId && m.Reason.Value == Discard);

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-luoying-native-discard", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, CaoXian, CaoZhi, "anniversary:shen-zhang-fei");
            builder.AddGeneral(new(Peer, "测试同桌", "", "standard:none", "qun", 6));
            builder.AddDeck(new(Deck, "固定非梅花摸牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 10)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "曹宪知否与落英", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [CaoXian, CaoZhi, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
