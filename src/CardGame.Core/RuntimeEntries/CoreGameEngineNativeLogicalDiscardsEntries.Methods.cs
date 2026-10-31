#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeLogicalDiscardsEntries
{
    internal ProgramSkillFrame AppendFrameNativeLogicalDiscard(ProgramSkillFrame frame, int executorSeat, IReadOnlyList<int> ids, CardMoveReason reason) => _resolve().AppendFrameNativeLogicalDiscard(frame, executorSeat, ids, reason);
    internal NativeLogicalDiscardInvocationDraft? CaptureNativeLogicalDiscardInvocation(int initiatorSeat, int executorSeat, IReadOnlyList<int> cardIds, CardMoveReason reason, long? parentFrameId = default) => _resolve().CaptureNativeLogicalDiscardInvocation(initiatorSeat, executorSeat, cardIds, reason, parentFrameId);
    internal ProgramSkillFrame CompleteFrameNativeLogicalDiscard(ProgramSkillFrame frame) => _resolve().CompleteFrameNativeLogicalDiscard(frame);
    internal void CompleteNativeLogicalDiscardInvocation(NativeLogicalDiscardInvocationDraft? draft, IReadOnlyList<CardMovementRecord>? prepaidMaterials = default) => _resolve().CompleteNativeLogicalDiscardInvocation(draft, prepaidMaterials);
    internal void MoveLogicalDiscard(int initiatorSeat, int executorSeat, Card card, CardLocation from, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = default, long? parentFrameId = default) => _resolve().MoveLogicalDiscard(initiatorSeat, executorSeat, card, from, reason, beforeFact, parentFrameId);
    internal void MoveLogicalDiscardCards(int initiatorSeat, int executorSeat, IReadOnlyList<Card> cards, CardLocation from, CardMoveReason reason) => _resolve().MoveLogicalDiscardCards(initiatorSeat, executorSeat, cards, from, reason);
    internal void MoveProgramLogicalDiscard(ProgramSkillFrame frame, int executorSeat, IReadOnlyList<int> ids, CardMoveReason reason, Action<long, IReadOnlyList<CardMovementRecord>>? afterPhysicalRecords = default) => _resolve().MoveProgramLogicalDiscard(frame, executorSeat, ids, reason, afterPhysicalRecords);
    internal bool TracksNativeLogicalDiscards { get => _resolve().TracksNativeLogicalDiscards; }
}
