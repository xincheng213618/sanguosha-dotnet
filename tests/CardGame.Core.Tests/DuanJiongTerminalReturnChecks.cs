using CardGame.Core;

internal static partial class DuanJiongChecks
{
    private sealed class SaoguTerminalSetupFactory : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:duan-jiong-terminal-setup";
        public string ModeId => Mode;
        public void ValidateInitialOptions(GameOptions options, ModeConfigurationValidationPhase phase)
        {
            if (options.PlayerCount != 2 || options.ModeId != Mode)
                throw new ArgumentException("The terminal Saogu fixture requires its exact two-seat team mode.", nameof(options));
        }
        public IGameModeFeature Create(ModeFeatureCreationContext context) =>
            new CardGame.Modes.Standard.StandardModeSetupFactory().Create(context);
    }

    public static void LethalDiscardSlashCompletesOriginalPaidAncestorCold()
    {
        var (g, registry) = Start("terminal");
        Need(V(g, 1).Hp == 1 && g.State.Players.Count == 2,
            "The real two-camp scene has exactly one living opponent at lethal HP.");
        Begin(g); DiscardBoundary(g);
        var paid = Current(g)!.Candidates.Take(2).Select(c => c.CardId).ToArray();
        Answer(g, c => c.Cards.SequenceEqual([paid[0]]));
        Answer(g, c => c.Cards.SequenceEqual([paid[1]]));
        UseBoundary(g);
        Answer(g, c => c.Cards.SequenceEqual([paid[0]]) && c.Targets.SequenceEqual([1]));
        Reach(g, () => Current(g)?.Stage == DuanJiongSaoguStage.NativeUse && P(g)?.SkillPrompt?.SkillId == Commit);
        var origin = E<DuanJiongSaoguNativeIssuedEvent>(g).Single().Origin;
        Need(origin.CardId == paid[0] && Current(g) is { Paid: true, NativeCompleted: false } &&
                E<DuanJiongSaoguEntityPaidEvent>(g).Count(e => !e.DelegateFee) == 2,
            "The original discarded physical Slash and both genuine payments pause inside their native committed child.");
        g = Cold(g, registry); Continue(g);
        Reach(g, () => g.State.Winner != Winner.None);
        Need(g.State.Status == EngineStatus.Completed && !V(g, 1).IsAlive && V(g).IsAlive &&
                g.ResolutionStack.Count == 0 && P(g) is null && g.CreateSnapshot(0).ProcessingCardCount == 0 &&
                E<DuanJiongSaoguCompletedEvent>(g).Count(e => e.FrameId == origin.ProgramFrameId && e.UsedCards == 1) == 1 &&
                E<ProgramSkillResolvedEvent>(g).Count(e => e.FrameId == origin.ProgramFrameId && e.Completed) == 1 &&
                E<DuanJiongSaoguNativeReturnedEvent>(g).Count(e => e.Origin == origin) == 1 &&
                E<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == origin.CardUseFrameId) == 1 &&
                E<DamageAppliedEvent>(g).Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) == 1,
            "The lethal native return finishes its exact paid ancestor once, without requiring a post-win engine step.");
        Need(E<DuanJiongSaoguEntityPaidEvent>(g).Count(e => !e.DelegateFee) == 2 &&
                E<DuanJiongSaoguNativeIssuedEvent>(g).Length == 1 && E<CardUseDebitRecordedEvent>(g).Length == 0 &&
                g.CardMovements.Count(m => m.CardId == paid[0] && m.From == CardLocation.DiscardPile &&
                    m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1 &&
                paid.All(id => g.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location == CardLocation.DiscardPile),
            "Terminal completion preserves both real discard invoices, one unlimited native use and the unused second paid Slash.");
        _ = Cold(g, registry);
    }
}
