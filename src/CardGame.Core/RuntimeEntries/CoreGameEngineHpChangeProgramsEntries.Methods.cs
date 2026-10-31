#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpChangeProgramsEntries
{
    internal void AdvanceHpChangedProgramCandidate(global::CardGame.Core.HpChangedTriggerWindowFrame frame, bool activated, bool completed) => _resolve().AdvanceHpChangedProgramCandidate(frame, activated, completed);
    internal void AdvanceHpChangedProgramCursor(global::CardGame.Core.HpChangedTriggerWindowFrame frame) => _resolve().AdvanceHpChangedProgramCursor(frame);
    internal void CompleteHpChangedProgramWindow(global::CardGame.Core.HpChangedTriggerWindowFrame frame) => _resolve().CompleteHpChangedProgramWindow(frame);
    internal void ContinueHpChangedProgramWindowCore() => _resolve().ContinueHpChangedProgramWindowCore();
    internal global::CardGame.Core.HpChangeContext? RecordHpChange(long? parentFrameId, int? sourceSeat, int targetSeat, int before, int after, global::CardGame.Core.HpChangeKind kind) => _resolve().RecordHpChange(parentFrameId, sourceSeat, targetSeat, before, after, kind);
    internal bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => _resolve().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}
