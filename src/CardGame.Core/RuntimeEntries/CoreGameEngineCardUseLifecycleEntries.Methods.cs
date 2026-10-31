#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardUseLifecycleEntries
{
    internal void BeginSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().BeginSimpleCardUse(frameId, continuation);
    internal void ContinueCommittedTrickUse(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => _resolve().ContinueCommittedTrickUse(frame);
    internal void ContinueSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueSimpleCardUse(frameId, continuation);
    internal global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => _resolve().LifecycleCardUse(frameId);
    internal bool TryBeginCommittedCardUse(long frameId, global::CardGame.Core.ProgramCardContinuation continuation, global::CardGame.Core.ProgramTrickContinuation? trick = default, global::CardGame.Core.ProgramSimpleCardContinuation? simple = default) => _resolve().TryBeginCommittedCardUse(frameId, continuation, trick, simple);
    internal bool TryMarkFinalizedSimpleProgramsStarted(long frameId) => _resolve().TryMarkFinalizedSimpleProgramsStarted(frameId);
    internal bool TryMarkFinalizedTrickProgramsStarted(long frameId) => _resolve().TryMarkFinalizedTrickProgramsStarted(frameId);
    internal bool TryMarkProgramUseAccepted(long frameId) => _resolve().TryMarkProgramUseAccepted(frameId);
    internal bool TryMarkProgramUseCommitted(long frameId) => _resolve().TryMarkProgramUseCommitted(frameId);
    internal bool TryMarkTargetsAdjusted(long frameId) => _resolve().TryMarkTargetsAdjusted(frameId);
    internal bool TryMarkUnlimitedUse(long frameId) => _resolve().TryMarkUnlimitedUse(frameId);
    internal bool TryMarkYingboRepeated(long frameId) => _resolve().TryMarkYingboRepeated(frameId);
    internal bool TryMarkYingboUnrespondable(long frameId) => _resolve().TryMarkYingboUnrespondable(frameId);
    internal void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update) => _resolve().UpdateLifecycleCardUse(frameId, update);
}
