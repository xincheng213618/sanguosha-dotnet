#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeRecastInvocationsEntries
{
    internal ProgramSkillFrame BeginFrameNativeRecastInvocation(ProgramSkillFrame frame, int executorSeat, IReadOnlyList<int> originalPrimaryIds, int? participantCursor = default) => _resolve().BeginFrameNativeRecastInvocation(frame, executorSeat, originalPrimaryIds, participantCursor);
    internal NativeRecastInvocationDraft? CaptureNativeRecastInvocation(int initiatorSeat, int executorSeat, IReadOnlyList<int> originalPrimaryIds, long? parentFrameId = default, int? instructionIndex = default, int? participantCursor = default, CardMoveReason? reason = default) => _resolve().CaptureNativeRecastInvocation(initiatorSeat, executorSeat, originalPrimaryIds, parentFrameId, instructionIndex, participantCursor, reason);
    internal ProgramSkillFrame CompleteFrameNativeRecastInvocation(ProgramSkillFrame frame, int actualDrawCount) => _resolve().CompleteFrameNativeRecastInvocation(frame, actualDrawCount);
    internal void CompleteNativeRecastInvocation(NativeRecastInvocationDraft? draft, int actualDrawCount) => _resolve().CompleteNativeRecastInvocation(draft, actualDrawCount);
    internal bool TracksNativeRecastInvocations { get => _resolve().TracksNativeRecastInvocations; }
}
