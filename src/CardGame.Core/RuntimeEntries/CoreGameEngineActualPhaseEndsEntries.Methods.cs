#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualPhaseEndsEntries
{
    internal void ConfirmActualDiscardPhaseExecution() => _resolve().ConfirmActualDiscardPhaseExecution();
    internal bool IsExactActualPhaseEndCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsExactActualPhaseEndCandidate(candidate, context);
    internal void ReturnActualPhaseEndWindow(ProgramLifecycleTriggerWindowFrame window) => _resolve().ReturnActualPhaseEndWindow(window);
    internal bool TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind kind, ActualPhaseEndContinuation continuation, int delayedEffects = 0, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false, long? parentFrameId = default) => _resolve().TryBeginActualPhaseEnd(kind, continuation, delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted, parentFrameId);
    internal bool TracksActualPhaseEnds { get => _resolve().TracksActualPhaseEnds; }
}
