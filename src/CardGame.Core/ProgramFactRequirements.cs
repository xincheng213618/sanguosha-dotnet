namespace CardGame.Core;

[Flags]
public enum ProgramTrackedFact
{
    None = 0,
    NativeLogicalDiscard = 1,
    ActualDiscardRecoveryPhase = 2,
    NativeTrueUseProcessingEntry = 4,
    WholeActualUseDeclaration = 8,
    NativeDrawInvocation = 16,
    NativeActionAppearance = 32,
    ActualCardUseHistory = 64,
    ActualPhaseEnd = 128,
    MovementTiming = 256,
    HandGainOccurrence = 512,
    ActualRound = 1024,
    PublicCardRevealHistory = 2048
}

// Definition-only demand for shared native facts. Keep trigger and activation
// sources separate: using an operation at another entry point must not enable
// facts that the historical catalog predicate did not request.
internal readonly record struct ProgramFactRequirements(
    ProgramTrackedFact Triggered = ProgramTrackedFact.None,
    ProgramTrackedFact Activated = ProgramTrackedFact.None)
{
    internal ProgramFactRequirements Union(ProgramFactRequirements other) =>
        new(Triggered | other.Triggered, Activated | other.Activated);

    internal static ProgramTrackedFact Close(ProgramTrackedFact facts)
    {
        if ((facts & ProgramTrackedFact.ActualPhaseEnd) != 0) facts |= ProgramTrackedFact.ActualDiscardRecoveryPhase;
        if ((facts & ProgramTrackedFact.HandGainOccurrence) != 0) facts |= ProgramTrackedFact.NativeDrawInvocation;
        if ((facts & ProgramTrackedFact.PublicCardRevealHistory) != 0) facts |= ProgramTrackedFact.ActualDiscardRecoveryPhase;
        return facts;
    }

    internal bool IsValid => ((Triggered | Activated) &
        ~(ProgramTrackedFact.NativeLogicalDiscard | ProgramTrackedFact.ActualDiscardRecoveryPhase |
          ProgramTrackedFact.NativeTrueUseProcessingEntry | ProgramTrackedFact.WholeActualUseDeclaration |
          ProgramTrackedFact.NativeDrawInvocation | ProgramTrackedFact.NativeActionAppearance |
          ProgramTrackedFact.ActualCardUseHistory | ProgramTrackedFact.ActualPhaseEnd |
          ProgramTrackedFact.MovementTiming | ProgramTrackedFact.HandGainOccurrence | ProgramTrackedFact.ActualRound |
          ProgramTrackedFact.PublicCardRevealHistory)) == ProgramTrackedFact.None;
}
