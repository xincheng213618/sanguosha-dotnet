using CardGame.Core;
using CardGame.Content.Standard;
using static BoundaryLiDianChecks;

internal static partial class OrdinaryXunChenChecks
{
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
        ReplayFourViews(g, registry);
        Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "give");
        Reach(g, p => p.Kind == DecisionKind.PlayCard && p.PlayerSeat == 0);
        Require(g.CardMovements.Count(m => m.CardId == result.SourceCardId && m.From == CardLocation.DiscardPile &&
                m.To == CardLocation.Hand(1) && m.Reason.Value == "skill-program.ol:fenglve.fenglve-gift") == 1 &&
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
        var gifts = g.CardMovements.Where(m => m.Reason.Value == "skill-program.ol:fenglve.fenglve-gift").ToArray();
        var contests = g.Events.Select(e => e.Payload).OfType<PindianResultDeterminedEvent>().Where(e => e.SkillId == Fenglve).ToArray();
        Require(gifts.Length > 0 && gifts.All(m => contests.Any(e => e.Result.SourceCardId == m.CardId &&
                m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(e.Result.OpponentSeat))),
            "Pending results within the same autonomous command pay only their actual contest source entities.");
        ReplayFourViews(g, registry);

        static bool IsGift(PendingDecision? p) => p?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "give") == true;
    }
}
