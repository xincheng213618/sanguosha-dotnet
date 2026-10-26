using CardGame.Core;

internal static partial class YangFuChecks
{
    private sealed class SuyiTerminalSetupFactory : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:yang-fu-terminal-setup";
        public string ModeId => Mode;
        public void ValidateInitialOptions(GameOptions options, ModeConfigurationValidationPhase phase)
        {
            if (options.PlayerCount != 2 || options.ModeId != Mode)
                throw new ArgumentException("The terminal fixture requires its exact two-seat team mode.", nameof(options));
        }
        public IGameModeFeature Create(ModeFeatureCreationContext context) =>
            new CardGame.Modes.Standard.StandardModeSetupFactory().Create(context);
    }

    public static void SuyiLethalNativeSlashCompletesExactPaidAncestorCold()
    {
        var (g, registry) = Start("suyi-terminal");
        Need(V(g, 1).Hp == 1 && g.State.Players.Count == 2,
            "The real two-camp fixture has exactly one living opponent at lethal HP.");
        SuyiOn(g, 1);
        Reach(g, () => P(g)?.SkillPrompt?.SkillId == Watch);
        var origin = E<SuyiNativeIssuedEvent>(g).Single(e => e.Origin.Kind == CardKind.Slash).Origin;
        var parent = g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == origin.ProgramFrameId);
        var native = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == origin.UseId);
        Need(parent.YangFu is { Stage: YangFuStage.SlashUse, NativeCompleted: false } &&
                native.SuyiOrigin == origin && native.CardId == 0 && native.PhysicalCardIds.Count == 0 &&
                native.Action is { PhysicalCards.Count: 0 } action && action.ConversionChain.SequenceEqual([origin.Source]) &&
                E<SuyiNativeIssuedEvent>(g).Count(e => e.Origin.Kind != CardKind.Slash) == 1 &&
                E<SuyiNativeReturnedEvent>(g).Count(e => e.Origin.Kind != CardKind.Slash) == 1,
            "The actual stock equipment returns before its original zero-entity Slash pauses in a real committed child.");
        g = Cold(g, registry);
        Continue(g);
        Reach(g, () => g.State.Winner != Winner.None);
        Need(g.State.Status == EngineStatus.Completed && !V(g, 1).IsAlive && V(g, 0).IsAlive &&
                g.ResolutionStack.Count == 0 && P(g) is null &&
                E<YangFuCompletedEvent>(g).Count(e => e.FrameId == origin.ProgramFrameId) == 1 &&
                E<ProgramSkillResolvedEvent>(g).Count(e => e.FrameId == origin.ProgramFrameId && e.Completed) == 1 &&
                E<SuyiNativeReturnedEvent>(g).Count(e => e.Origin == origin) == 1 &&
                E<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == origin.UseId) == 1 &&
                E<DamageAppliedEvent>(g).Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) == 1,
            "The lethal native return completes its exact paid ancestor once without waiting for another engine step.");
        var equipment = E<SuyiNativeIssuedEvent>(g).Single(e => e.Origin.Kind != CardKind.Slash).Origin;
        Need(g.CardMovements.Count(m => m.CardId == equipment.CardId && m.From == CardLocation.DrawPile &&
                m.To == CardLocation.Processing && m.Reason == CardMoveReasons.EquipmentUse) == 1 &&
                E<SuyiSlashQuotaPaidEvent>(g).Count(e => e.UseId == origin.UseId && e.FrameId == origin.ProgramFrameId) == 1,
            "Final cleanup retains the genuine equipment invoice and single ordinary Slash quota payment.");
        _ = Cold(g, registry);
    }
}
