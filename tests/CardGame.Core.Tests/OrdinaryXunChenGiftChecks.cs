using CardGame.Core;
using CardGame.Content.Standard;
using static BoundaryLiDianChecks;

internal static partial class OrdinaryXunChenChecks
{
    private const string Zongshi = "classic:zongshi-pindian";
    private const string FenglveGiftReason = "skill-program.ol:fenglve.fenglve-gift";

    public static void FenglveGiftsItsExactCurrentPindianCardPendingAndCold()
    {
        var (g, registry) = Start();
        if (P(g)!.Choices.Any(c => c.Parameters.GetValueOrDefault("skill-id") == Moushi)) Skip(g);
        Activate(g); SelectTarget(g, 1);
        for (var step = 0; step < 80 && !IsGift(P(g)); step++)
        {
            if (P(g) is { } pick && pick.Choices.Any(c => c.Cards.Count == 1))
                Answer(g, c => c.Cards.Count == 1);
            else Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        Require(IsGift(P(g)), "The actual Fenglve contest reaches its optional original-card gift.");
        var parent = g.ResolutionStack.OfType<ProgramSkillFrame>().Last();
        var binding = parent.PindianResultBindings.Single(b => b.Name == "fenglve-contest");
        var result = g.Events.Select(e => e.Payload).OfType<PindianResultDeterminedEvent>()
            .Single(e => e.FrameId == binding.NativePindianFrameId && e.SkillId == Fenglve).Result;
        Require(binding.SourceDiscard is { } source && source.CardId == result.SourceCardId &&
            g.CardMovements.Any(m => m.Sequence == source.EntrySequence && m.CardId == source.CardId &&
                m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.PindianFinish),
            "The original Fenglve frame retains the exact native discard entry for its own contestant's card.");
        ReplayFourViews(g, registry);
        Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "give");
        Reach(g, p => p.Kind == DecisionKind.PlayCard && p.PlayerSeat == 0);
        Require(g.CardMovements.Count(m => m.CardId == result.SourceCardId && m.From == CardLocation.DiscardPile &&
                m.To == CardLocation.Hand(1) && m.Reason.Value == FenglveGiftReason) == 1 &&
            g.CreateCardZoneDiagnostics().Single(c => c.CardId == result.SourceCardId).Location == CardLocation.Hand(1),
            "The exact returned source entity is gifted once after its contest and ordinary loss payment.");
        ReplayFourViews(g, registry);

        // A fully autonomous Start is one command: its current Pindian is pending,
        // so a committed-history lookup cannot see the result at the gift choice.
        registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(allFenglve: true));
        g = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, HumanSeat = -1, HumanRole = null, ModeId = "identity:xc",
            UseInteractiveSetup = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);
        Accept(g, new StartGameCommand());
        var gifts = g.CardMovements.Where(m => m.Reason.Value == FenglveGiftReason).ToArray();
        var contests = g.Events.Select(e => e.Payload).OfType<PindianResultDeterminedEvent>().Where(e => e.SkillId == Fenglve).ToArray();
        Require(gifts.Length > 0 && gifts.All(m => contests.Any(e => e.Result.SourceCardId == m.CardId &&
                m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(e.Result.OpponentSeat))),
            "Pending results within the same autonomous command pay only their actual contest source entities.");
        ReplayFourViews(g, registry);
    }

    public static void FenglveGiftContinuesWhenZongshiAlreadyClaimedItsSource()
    {
        var (game, registry) = Start(claimOwnPindianCard: true);
        if (P(game)!.Choices.Any(c => c.Parameters.GetValueOrDefault("skill-id") == Moushi)) Skip(game);
        Activate(game); SelectTarget(game, 1);
        var sourceCard = P(game)!.Choices.First(c => c.Parameters.GetValueOrDefault("action") == "pindian-source-card").Cards.Single();
        Answer(game, c => c.Cards.SequenceEqual([sourceCard]));
        Reach(game, p => p is { PlayerSeat: 1, Kind: DecisionKind.SkillModule });
        Answer(game, c => c.Parameters.GetValueOrDefault("action") == "pindian-card");
        Reach(game, p => p.SkillPrompt?.SkillId == Zongshi);
        Answer(game, c => c.Parameters.GetValueOrDefault("take") == "true" && c.Cards.SequenceEqual([sourceCard]));

        Reach(game, p => p.SkillPrompt?.SkillId == Fenglve && p.Choices.Any(c => c.Cards is [var id] && id != sourceCard));
        Answer(game, c => c.Cards is [var id] && id != sourceCard);
        Reach(game, IsGift);

        var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Last();
        var binding = parent.PindianResultBindings.Single(b => b.Name == "fenglve-contest");
        Require(binding is { SourceWon: false, SourceDiscard: null, OpponentDiscard: not null } &&
            game.CardMovements.Any(m => m.CardId == sourceCard && m.From == CardLocation.Processing &&
                m.To == CardLocation.Hand(0) && m.Reason.Value == "program.pindian.claim") &&
            !game.CardMovements.Any(m => m.CardId == sourceCard && m.Reason == CardMoveReasons.PindianFinish),
            "Zongshi takes the source card during the tied contest, and native cleanup only discards the opponent's card.");
        ReplayFourViews(game, registry);
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        foreach (var branch in new[] { game, restored })
        {
            Require(P(branch)!.Choices.Select(c => c.Parameters.GetValueOrDefault("option-id")).ToHashSet().SetEquals(["give", "keep"]),
                "Fenglve keeps both of its published choices after the original source card is claimed.");
            Answer(branch, c => c.Parameters.GetValueOrDefault("option-id") == "give");
            Settle(branch);
            Require(branch.CreateSnapshot(0).Players[0].Hand.Any(c => c.Id == sourceCard) &&
                !branch.CardMovements.Any(m => m.Reason.Value == FenglveGiftReason),
                "The issued give choice settles without moving an already claimed source card, including after cold restore.");
        }
    }

    private static bool IsGift(PendingDecision? p) => p?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "give") == true;
}
