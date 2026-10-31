#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreRecoveryReplacementEntries
{
    internal void ApplyRecoveryAttemptProducerFacts(long childId, RecoveryAttempt attempt, int recovered) => _resolve().ApplyRecoveryAttemptProducerFacts(childId, attempt, recovered);
    internal void ApplyRecoveryReplacement(RecoveryReplacementFrame frame, int? candidateIndex) => _resolve().ApplyRecoveryReplacement(frame, candidateIndex);
    internal void CompleteDeferredRecoveryProducer(RecoveryAttempt attempt, int recovered, long parentId) => _resolve().CompleteDeferredRecoveryProducer(attempt, recovered, parentId);
    internal void CompletePaidEquipmentEntry(CharacterState source, Card equipment, long useId, EquipmentSlot slot, int? replacedCardId) => _resolve().CompletePaidEquipmentEntry(source, equipment, useId, slot, replacedCardId);
    internal void ContinueRecoveryPaidCardUse(long useId) => _resolve().ContinueRecoveryPaidCardUse(useId);
    internal void ContinueRecoveryReplacement() => _resolve().ContinueRecoveryReplacement();
    internal bool IsRecoveryReplacementCandidateCurrent(RecoveryReplacementFrame frame, RecoveryReplacementCandidate c) => _resolve().IsRecoveryReplacementCandidateCurrent(frame, c);
    internal IReadOnlyList<RecoveryReplacementCandidate> RecoveryReplacementCandidates(int seat) => _resolve().RecoveryReplacementCandidates(seat);
    internal IReadOnlyList<PromptChoice> RecoveryReplacementChoices(RecoveryReplacementFrame frame) => _resolve().RecoveryReplacementChoices(frame);
    internal void ResolveAiRecoveryReplacement() => _resolve().ResolveAiRecoveryReplacement();
    internal void ResumeRecoveryReplacementProducer(RecoveryReplacementFrame completed) => _resolve().ResumeRecoveryReplacementProducer(completed);
    internal CommandResult SubmitRecoveryReplacementAnswer(PromptChoice selected) => _resolve().SubmitRecoveryReplacementAnswer(selected);
    internal bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, PostEventContinuation continuation, int? cardId = default, CardKind? cardKind = default) => _resolve().TryBeginQueuedRecoveryReplacement(parentFrameId, continuation, cardId, cardKind);
    internal bool TryPauseRecoveryPaidCardUse(long useId, RecoveryPaidCardUseContinuation continuation) => _resolve().TryPauseRecoveryPaidCardUse(useId, continuation);
    internal bool TryQueueHengyeRecovery(CharacterState player) => _resolve().TryQueueHengyeRecovery(player);
    internal bool TryQueueRecoveryReplacement(long parentFrameId, int sourceSeat, int targetSeat, int amount, RecoveryAttemptCompletion completion, HappyQiaoRecoverToOrigin? happyQiaoRecoverToOrigin = default) => _resolve().TryQueueRecoveryReplacement(parentFrameId, sourceSeat, targetSeat, amount, completion, happyQiaoRecoverToOrigin);
    internal bool HasRecoveryReplacementCapability { get => _resolve().HasRecoveryReplacementCapability; }
}
