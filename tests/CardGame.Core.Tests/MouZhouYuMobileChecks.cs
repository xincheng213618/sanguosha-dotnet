using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class MouZhouYuMobileChecks
{
    private const string Hero = "mou:zhou-yu-mobile", Fanjian = "mou-mobile:fanjian", Activation = "cover-and-declare";
    private const string Peer = "fixture:mou-zhou-yu-peer", Deck = "fixture:mou-zhou-yu-deck";

    internal static void ActualCoveredPaymentReturnsToTargetCold()
    {
        const int owner = 0, target = 1;
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "mou-zhou-yu-fanjian", Hero, 3, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Spade, 7)], [],
            [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
            [new(Peer) { StartingHand = [] }], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, content);

        Accept(game, new StartGameCommand());
        Settle(game);
        var starts = Facts<TurnStartedEvent>();
        var actual = starts.Last();
        Require(actual.ActorSeat == owner && actual.TurnNumber == game.State.TurnNumber,
            "Fanjian starts in Zhou Yu's actual Play phase.");
        var turn = new MouZhouYuMobileTurn(actual.TurnNumber, actual.ActorSeat, starts.Length);
        var card = game.CreateSnapshot(owner).Players[owner].Hand.Single(c => c.Kind == CardKind.Slash && c.Suit == Suit.Spade);
        Accept(game, new UseProgramSkillCommand(owner, Fanjian, Activation, [], [target], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt.PlayerSeat == owner && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "cover"));
        Answer(game, choice => choice.Cards.SequenceEqual([card.Id]) && choice.Parameters.GetValueOrDefault("declared-suit") == "Heart");
        Reach(game, prompt => prompt.PlayerSeat == target && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "same"));

        var started = Facts<MouZhouYuMobileStartedEvent>().Single();
        var covered = Facts<MouZhouYuMobileCoveredEvent>().Single();
        var receipt = PendingFrame().MouZhouYuMobile!;
        var targetPrompt = P(game)!;
        Require(started.Source is { OwnerSeat: owner, SkillId: Fanjian, BindingId: Activation } && started.TargetSeat == target &&
            started.Turn == turn && receipt is { Stage: MouZhouYuMobileStage.TargetChoice, Revealed: false } &&
            receipt.Turn == turn && receipt.TargetSeat == target && receipt.CardId == card.Id &&
            covered.FrameId == started.FrameId && covered.CardId == card.Id && covered.ActualSuit == Suit.Spade && covered.DeclaredSuit == Suit.Heart &&
            covered.From == CardLocation.Hand(owner) && covered.To == CardLocation.Processing && receipt.PaymentSequence == covered.Sequence &&
            targetPrompt.Choices.All(choice => choice.Cards.Count == 0) && game.CreateSnapshot(owner).Players[owner].Hand.All(c => c.Id != card.Id),
            "The original frame records the global actual-turn ordinal and the hidden physical payment before the target guesses.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(PendingFrame().Id == started.FrameId && PendingFrame().MouZhouYuMobile == receipt &&
            P(game) is { PlayerSeat: target } restoredPrompt && restoredPrompt.PromptId == targetPrompt.PromptId &&
            Facts<TurnStartedEvent>().SequenceEqual(starts) && Facts<MouZhouYuMobileCoveredEvent>().Single() == covered,
            "Cold restore preserves the original actual turn, covered entity and target choice.");

        Accept(game, new AdvanceOneStepCommand(game.Revision));
        Settle(game);
        var gifted = Facts<MouZhouYuMobileGiftedEvent>().Single();
        var payment = game.CardMovements.Single(move => move.CardId == card.Id && move.Reason.Value == "skill-program.mou-mobile:fanjian.cover");
        var gift = game.CardMovements.Single(move => move.CardId == card.Id && move.Reason.Value == "skill-program.mou-mobile:fanjian.gift");
        Require(Facts<MouZhouYuMobileAnsweredEvent>().Single() is { TargetSeat: target, Answer: "same" or "different" } answered &&
            answered.FrameId == started.FrameId && gifted.FrameId == started.FrameId && gifted.TargetSeat == target && gifted.CardId == card.Id &&
            payment.Sequence == covered.Sequence && payment.From == covered.From && payment.To == covered.To &&
            gift.Sequence == gifted.Sequence && gift.Sequence > payment.Sequence && gift.From == CardLocation.Processing && gift.To == CardLocation.Hand(target) &&
            game.CreateSnapshot(target).Players[target].Hand.Single().Id == card.Id &&
            Facts<MouZhouYuMobileCompletedEvent>().Single() is { Paid: true } completed && completed.FrameId == started.FrameId,
            "The real target answers its private choice and receives the original entity once when the same paid frame returns.");

        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        ProgramSkillFrame PendingFrame() => game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.MouZhouYuMobile is not null);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mou-zhou-yu-fanjian-turn", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Peer, "反间对手", "", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "反间固定牌堆", 0, 0, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "反间原回合", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
