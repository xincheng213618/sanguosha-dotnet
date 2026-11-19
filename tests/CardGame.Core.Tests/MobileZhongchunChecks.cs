using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class MobileZhongchunChecks
{
    private const string Hero = "mou:guo-you-zhi-mobile", Bique = "mou-mobile:bique", Zhongchun = "mou-mobile:zhongchun";
    private const string Peer = "fixture:zhongchun-peer", Deck = "fixture:zhongchun-deck";
    private const string Discard = "skill-program.mou-mobile:bique.discard", Obtain = "skill-program.mou-mobile:zhongchun.obtain";

    internal static void BiqueDiscardMaterialsQualifyAtEndingAndReplay()
    {
        var (game, content) = Start();
        var turn = game.State.TurnNumber;
        var hand = game.CreateSnapshot(0).Players[0].Hand;
        var drawTwo = hand.Single(card => card.Kind == CardKind.DrawTwo).Id;
        Accept(game, new UseProgramSkillCommand(0, Bique, "inspect-and-give", [], [1], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => HasBranch(prompt, Bique, "gift-confirm"));
        Answer(game, choice => IsBranch(choice, "gift-confirm"));
        Reach(game, prompt => HasBranch(prompt, Bique, "adjust"));
        Answer(game, choice => IsBranch(choice, "adjust"));
        Reach(game, prompt => HasBranch(prompt, Bique, "discard-card"));
        foreach (var card in hand)
            Answer(game, choice => IsBranch(choice, "discard-card") && choice.Cards.SequenceEqual([card.Id]));
        Answer(game, choice => IsBranch(choice, "discard-confirm"));
        Settle(game);

        var invoice = game.Events.Select(e => e.Payload).OfType<NativeLogicalDiscardInvocationRecordedEvent>()
            .Single(e => e.Reason.Value == Discard);
        var materials = game.Events.Select(e => e.Payload).OfType<NativeLogicalDiscardMaterialCompletedEvent>()
            .Where(e => e.InvocationId == invoice.InvocationId).OrderBy(e => e.CompletionSequence).ToArray();
        Require(invoice is { ActualTurnOwnerSeat: 0, InitiatorSeat: 0, ExecutorSeat: 0 } && invoice.TurnNumber == turn &&
                invoice.ActualCount == hand.Count && invoice.HandCount == hand.Count &&
                materials.Select(e => e.CardId).Order().SequenceEqual(hand.Select(card => card.Id).Order()) &&
                materials.All(e => e.From == CardLocation.Hand(0) && e.To == CardLocation.DiscardPile &&
                    game.CardMovements.Any(move => move.CardId == e.CardId && move.Sequence == e.CompletionSequence &&
                        move.From == e.From && move.To == e.To && move.Reason == invoice.Reason)) &&
                game.CreateSnapshot(0).Players[0].Hand.Count == 0,
            "Real Bique discards the entire hand and records each original entry for the actual executor.");

        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        for (var step = 0; step < 60 && game.State.TurnNumber == turn && P(game) is not { PlayerSeat: 0 }; step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        Require(game.State.TurnNumber == turn && P(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Zhongchun } offer &&
                offer.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"),
            "The actual Ending actor must be offered Zhongchun after the native Bique discard.");

        Activate(game);
        Reach(game, prompt => HasBranch(prompt, Zhongchun, "obtain"));
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Zhongchun);
        Require(frame.TriggerId == "own-ending" &&
                frame.MouGuoYouZhi is { ActorSeat: 0, Stage: MouGuoYouZhiStage.DiscardChoice } receipt && receipt.ActualTurn == turn &&
                P(game)!.Choices.Where(choice => IsBranch(choice, "obtain")).Select(choice => choice.Cards.Single())
                    .SequenceEqual(materials.Select(e => e.CardId)),
            "Zhongchun offers the recorded materials in original discard order to the actual Ending actor.");

        game = Restore(game, content);
        Answer(game, choice => IsBranch(choice, "obtain") && choice.Cards.SequenceEqual([drawTwo]));
        Reach(game, prompt => HasBranch(prompt, Zhongchun, "use"));
        var original = materials.Single(e => e.CardId == drawTwo);
        var obtained = game.CardMovements.Single(move => move.Reason.Value == Obtain);
        Require(obtained.CardId == drawTwo && obtained.Sequence > original.CompletionSequence &&
                obtained.From == CardLocation.DiscardPile && obtained.To == CardLocation.Hand(0) &&
                P(game)!.Choices.Any(choice => IsBranch(choice, "decline")),
            "The beneficiary obtains the original Draw Two and can independently use or keep it.");

        game = Restore(game, content);
        Answer(game, choice => IsBranch(choice, "use"));
        for (var step = 0; step < 60 && !Completed(game, frame.Id); step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        var issued = game.Events.Select(e => e.Payload).OfType<MouGuoYouZhiNativeIssuedEvent>().Single(e => e.FrameId == frame.Id);
        Require(Completed(game, frame.Id) && issued is { ActorSeat: 0 } && issued.CardId == drawTwo &&
                game.Events.Select(e => e.Payload).OfType<MouGuoYouZhiNativeReturnedEvent>().Count(e => e.UseId == issued.UseId) == 1 &&
                game.Events.Select(e => e.Payload).OfType<CardUseDeclaredEvent>().Count(e => e.ResolutionId == issued.UseId &&
                    e.SourceSeat == 0 && e.CardId == drawTwo && e.CardKind == CardKind.DrawTwo) == 1 &&
                game.CardMovements.Count(move => move.Reason.Value == Obtain) == 1,
            "After cold recovery the beneficiary uses the same obtained entity once through the actual use frame.");
    }

    private static bool IsBranch(PromptChoice choice, string branch) => choice.Parameters.GetValueOrDefault("branch") == branch;

    private static bool HasBranch(PendingDecision prompt, string skill, string branch) =>
        prompt.PlayerSeat == 0 && prompt.SkillPrompt?.SkillId == skill && prompt.Choices.Any(choice => IsBranch(choice, branch));

    private static bool Completed(GameEngine game, long frameId) =>
        game.Events.Select(e => e.Payload).OfType<MouGuoYouZhiCompletedEvent>().Any(e => e.FrameId == frameId);

    private static GameEngine Restore(GameEngine game, ContentRegistry content)
    {
        var replay = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(SnapshotJson.Serialize(replay.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)) &&
                replay.CardMovements.SequenceEqual(game.CardMovements),
            "Cold recovery preserves current public commands, pending Zhongchun choices and movement entries.");
        return replay;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "zhongchun-bique-discard", Hero, 3, 0, 0, 0, 0, [],
            [new(CardKind.DrawTwo, Suit.Heart, 7), new(CardKind.Dodge, Suit.Club, 7)], [],
            [new(Peer, Hp: 8) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 7)] },
                new(Peer, Hp: 8) { StartingHand = [] }, new(Peer, Hp: 8) { StartingHand = [] }], [], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Settle(game);
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-zhongchun-bique-discard", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Peer, "实际同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定实体闪", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 10)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "裨阙弃牌与忠纯", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
