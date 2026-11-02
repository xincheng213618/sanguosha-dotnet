namespace CardGame.Core;

public enum ActualPhaseHistoryOutcome { Entered, Skipped }

/// <summary>
/// A real native skip, distinct from a requested substitution or a restored
/// phase. A null frame is the natural turn slot; scheduled phases retain the
/// issuing frame and cursor, and extra Draw phases retain their owning frame.
/// </summary>
public sealed record ActualPhaseSkippedEvent(int TurnNumber, int ActualTurnOwnerSeat, int ActorSeat,
    ActualDiscardRecoveryPhaseKind Kind, long? NativeFrameId, int NativeInstructionIndex, bool IsExtra) : IGameEvent;

internal interface IActualPhaseHistoryQueries : IProgramSelectionQuery
{
    int CountPhases(int actualTurn, int turnOwner, int actor, ActualDiscardRecoveryPhaseKind? kind,
        ActualPhaseHistoryOutcome outcome, long atCursor);
}

// The actual native boundary and its existing continuation data, never a
// requested skip or a second phase scheduler.
public sealed record ProgramActualPhaseSkipContext(int OwnerSeat, int ActualTurn, SkillProgramTurnPhase Phase,
    int DelayedEffects, bool AfterNormalDrawProgramsCompleted = false, bool DrawPhaseEndedProgramsCompleted = false,
    bool ActualDrawCompletionCompleted = false);
