#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeLogicalDiscardsRuntime
{
    internal bool TracksNativeLogicalDiscards => _nativeLogicalDiscardOperations.Get().TracksInvocations;

    internal NativeLogicalDiscardInvocationDraft? CaptureNativeLogicalDiscardInvocation(int initiatorSeat, int executorSeat, IReadOnlyList<int> cardIds, CardMoveReason reason, long? parentFrameId = null) => _nativeLogicalDiscardOperations.Get().Capture(initiatorSeat, executorSeat, cardIds, reason, parentFrameId);
    internal void CompleteNativeLogicalDiscardInvocation(NativeLogicalDiscardInvocationDraft? draft, IReadOnlyList<CardMovementRecord>? prepaidMaterials = null) => _nativeLogicalDiscardOperations.Get().Complete(draft, prepaidMaterials);
    internal void MoveProgramLogicalDiscard(ProgramSkillFrame frame, int executorSeat, IReadOnlyList<int> ids, CardMoveReason reason, Action<long, IReadOnlyList<CardMovementRecord>>? afterPhysicalRecords = null) => _nativeLogicalDiscardOperations.Get().MoveProgram(frame, executorSeat, ids, reason, afterPhysicalRecords);
    internal void MoveLogicalDiscard(int initiatorSeat, int executorSeat, Card card, CardLocation from, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = null, long? parentFrameId = null) => _nativeLogicalDiscardOperations.Get().Move(initiatorSeat, executorSeat, card, from, reason, beforeFact, parentFrameId);
    internal void MoveLogicalDiscardCards(int initiatorSeat, int executorSeat, IReadOnlyList<Card> cards, CardLocation from, CardMoveReason reason) => _nativeLogicalDiscardOperations.Get().MoveCards(initiatorSeat, executorSeat, cards, from, reason);
    internal ProgramSkillFrame AppendFrameNativeLogicalDiscard(ProgramSkillFrame frame, int executorSeat, IReadOnlyList<int> ids, CardMoveReason reason) => _nativeLogicalDiscardOperations.Get().AppendFrame(frame, executorSeat, ids, reason);
    internal ProgramSkillFrame CompleteFrameNativeLogicalDiscard(ProgramSkillFrame frame) => _nativeLogicalDiscardOperations.Get().CompleteFrame(frame);
}
