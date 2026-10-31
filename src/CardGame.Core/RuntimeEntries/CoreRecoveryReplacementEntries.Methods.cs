#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreRecoveryReplacementEntries
{
    internal void ApplyRecoveryAttemptProducerFacts(long childId, global::CardGame.Core.RecoveryAttempt attempt, int recovered) => _resolve().ApplyRecoveryAttemptProducerFacts(childId, attempt, recovered);
    internal void ApplyRecoveryReplacement(global::CardGame.Core.RecoveryReplacementFrame frame, int? candidateIndex) => _resolve().ApplyRecoveryReplacement(frame, candidateIndex);
    internal void AssertRecoveryReplacementInvariants() => _resolve().AssertRecoveryReplacementInvariants();
    internal void CompleteDeferredRecoveryProducer(global::CardGame.Core.RecoveryAttempt attempt, int recovered, long parentId) => _resolve().CompleteDeferredRecoveryProducer(attempt, recovered, parentId);
    internal void CompletePaidEquipmentEntry(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long useId, global::CardGame.Core.EquipmentSlot slot, int? replacedCardId) => _resolve().CompletePaidEquipmentEntry(source, equipment, useId, slot, replacedCardId);
    internal void ContinueRecoveryPaidCardUse(long useId) => _resolve().ContinueRecoveryPaidCardUse(useId);
    internal void ContinueRecoveryReplacement() => _resolve().ContinueRecoveryReplacement();
    internal bool HasRecoveryPaidCardUseObserver(long useId) => _resolve().HasRecoveryPaidCardUseObserver(useId);
    internal bool IsRecoveryReplacementCandidateCurrent(global::CardGame.Core.RecoveryReplacementFrame frame, global::CardGame.Core.RecoveryReplacementCandidate c) => _resolve().IsRecoveryReplacementCandidateCurrent(frame, c);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RecoveryReplacementCandidate> RecoveryReplacementCandidates(int seat) => _resolve().RecoveryReplacementCandidates(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RecoveryReplacementChoices(global::CardGame.Core.RecoveryReplacementFrame frame) => _resolve().RecoveryReplacementChoices(frame);
    internal void ResolveAiRecoveryReplacement() => _resolve().ResolveAiRecoveryReplacement();
    internal void ResumeRecoveryReplacementProducer(global::CardGame.Core.RecoveryReplacementFrame completed) => _resolve().ResumeRecoveryReplacementProducer(completed);
    internal global::CardGame.Core.CommandResult SubmitRecoveryReplacementAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitRecoveryReplacementAnswer(selected);
    internal bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => _resolve().TryBeginQueuedRecoveryReplacement(parentFrameId, continuation, cardId, cardKind);
    internal bool TryPauseRecoveryPaidCardUse(long useId, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => _resolve().TryPauseRecoveryPaidCardUse(useId, continuation);
    internal bool TryQueueHengyeRecovery(global::CardGame.Core.CharacterState player) => _resolve().TryQueueHengyeRecovery(player);
    internal bool TryQueueRecoveryReplacement(long parentFrameId, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.RecoveryAttemptCompletion completion, global::CardGame.Core.HappyQiaoRecoverToOrigin? happyQiaoRecoverToOrigin = default) => _resolve().TryQueueRecoveryReplacement(parentFrameId, sourceSeat, targetSeat, amount, completion, happyQiaoRecoverToOrigin);
    internal bool HasRecoveryReplacementCapability { get => _resolve().HasRecoveryReplacementCapability; }
}
