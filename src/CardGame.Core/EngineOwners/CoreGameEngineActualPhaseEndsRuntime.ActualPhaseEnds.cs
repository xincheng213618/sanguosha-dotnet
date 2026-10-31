#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualPhaseEndsRuntime
{
    internal bool TracksActualPhaseEnds => _actualPhaseEndOperations.Get().Tracks;

    internal void ConfirmActualDiscardPhaseExecution() => _actualPhaseEndOperations.Get().ConfirmActualDiscardPhaseExecution();
    internal bool TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind kind, ActualPhaseEndContinuation continuation, int delayedEffects = 0, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false, long? parentFrameId = null) => _actualPhaseEndOperations.Get().TryBeginActualPhaseEnd(kind, continuation, delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted, parentFrameId);
    internal bool IsExactActualPhaseEndCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _actualPhaseEndOperations.Get().IsExactActualPhaseEndCandidate(candidate, context);
    internal void ReturnActualPhaseEndWindow(ProgramLifecycleTriggerWindowFrame window) => _actualPhaseEndOperations.Get().ReturnActualPhaseEndWindow(window);
}
