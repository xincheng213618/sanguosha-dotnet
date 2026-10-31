#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpChangeProgramsEntries
{
    internal void AdvanceHpChangedProgramCandidate(HpChangedTriggerWindowFrame frame, bool activated, bool completed) => _resolve().AdvanceHpChangedProgramCandidate(frame, activated, completed);
    internal void AdvanceHpChangedProgramCursor(HpChangedTriggerWindowFrame frame) => _resolve().AdvanceHpChangedProgramCursor(frame);
    internal void CompleteHpChangedProgramWindow(HpChangedTriggerWindowFrame frame) => _resolve().CompleteHpChangedProgramWindow(frame);
    internal void ContinueHpChangedProgramWindowCore() => _resolve().ContinueHpChangedProgramWindowCore();
    internal HpChangeContext? RecordHpChange(long? parentFrameId, int? sourceSeat, int targetSeat, int before, int after, HpChangeKind kind) => _resolve().RecordHpChange(parentFrameId, sourceSeat, targetSeat, before, after, kind);
    internal bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, PostEventContinuation continuation = (PostEventContinuation)0, int? cardId = default, CardKind? cardKind = default) => _resolve().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}
