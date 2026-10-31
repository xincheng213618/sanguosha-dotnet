#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualPhaseEndsEntries
{
    internal void ConfirmActualDiscardPhaseExecution() => _resolve().ConfirmActualDiscardPhaseExecution();
    internal bool IsExactActualPhaseEndCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsExactActualPhaseEndCandidate(candidate, context);
    internal void ReturnActualPhaseEndWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().ReturnActualPhaseEndWindow(window);
    internal bool TryBeginActualPhaseEnd(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, global::CardGame.Core.ActualPhaseEndContinuation continuation, int delayedEffects = 0, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false, long? parentFrameId = default) => _resolve().TryBeginActualPhaseEnd(kind, continuation, delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted, parentFrameId);
    internal bool TracksActualPhaseEnds { get => _resolve().TracksActualPhaseEnds; }
}
