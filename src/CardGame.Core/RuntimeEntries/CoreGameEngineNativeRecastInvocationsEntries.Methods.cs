#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeRecastInvocationsEntries
{
    internal global::CardGame.Core.ProgramSkillFrame BeginFrameNativeRecastInvocation(global::CardGame.Core.ProgramSkillFrame frame, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> originalPrimaryIds, int? participantCursor = default) => _resolve().BeginFrameNativeRecastInvocation(frame, executorSeat, originalPrimaryIds, participantCursor);
    internal global::CardGame.Core.NativeRecastInvocationDraft? CaptureNativeRecastInvocation(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> originalPrimaryIds, long? parentFrameId = default, int? instructionIndex = default, int? participantCursor = default, global::CardGame.Core.CardMoveReason? reason = default) => _resolve().CaptureNativeRecastInvocation(initiatorSeat, executorSeat, originalPrimaryIds, parentFrameId, instructionIndex, participantCursor, reason);
    internal global::CardGame.Core.ProgramSkillFrame CompleteFrameNativeRecastInvocation(global::CardGame.Core.ProgramSkillFrame frame, int actualDrawCount) => _resolve().CompleteFrameNativeRecastInvocation(frame, actualDrawCount);
    internal void CompleteNativeRecastInvocation(global::CardGame.Core.NativeRecastInvocationDraft? draft, int actualDrawCount) => _resolve().CompleteNativeRecastInvocation(draft, actualDrawCount);
    internal bool TracksNativeRecastInvocations { get => _resolve().TracksNativeRecastInvocations; }
}
