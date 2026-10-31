namespace CardGame.Core;

internal interface IActualDiscardRecoveryPhaseQueries : IProgramSelectionQuery
{
    ActualDiscardRecoveryPhaseKey? Current();
    bool IsRecorded(ActualDiscardRecoveryPhaseKey phase);
}

internal interface IActualDiscardRecoveryPhaseOperations : IProgramRuntimeOperations
{
    void Start(ActualDiscardRecoveryPhaseKind kind, int actor);
    void Clear();
    void Restore(ProgramPhaseSchedule schedule);
    void Restore(DrawPhaseObligationFrame parent);
}

// The original action-query scope remains the sole cache owner. Its two fields
// are exposed directly; the runtime retains that exact scope across its read.
internal interface IActualPhaseQueryCache : IProgramSelectionQuery
{
    IActualDiscardPhaseQueryCacheState? Current { get; }
}

internal interface IActualDiscardPhaseQueryCacheState
{
    bool HasActualDiscardRecoveryPhase { get; set; }
    ActualDiscardRecoveryPhaseKey? ActualDiscardRecoveryPhase { get; set; }
}

internal interface IActualDiscardRecoveryPhaseFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    bool ModeTracksActualDiscardRecoveryPhases { get; }
    bool HasLishiCapability { get; }
}

public sealed partial class GameEngine
{
}
