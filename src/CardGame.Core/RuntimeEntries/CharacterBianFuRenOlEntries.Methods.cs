#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBianFuRenOlEntries
{
    internal bool BianFuRenDelayedFinish(global::CardGame.Core.BianFuRenUsedMaterial material, global::CardGame.Core.CardMovementRecord finish) => _resolve().BianFuRenDelayedFinish(material, finish);
    internal global::CardGame.Core.BianFuRenOwnedLossReceipt? BianFuRenLoss(global::CardGame.Core.ProgramSkillWindowContext context, int owner) => _resolve().BianFuRenLoss(context, owner);
    internal bool BianFuRenMaterialAvailable(global::CardGame.Core.BianFuRenUsedMaterial material) => _resolve().BianFuRenMaterialAvailable(material);
    internal long BianFuRenObtainProducer(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.CardMovementRecord entry, global::CardGame.Core.CardMovementRecord completion) => _resolve().BianFuRenObtainProducer(batch, entry, completion);
    internal global::CardGame.Core.CardMovementRecord? BianFuRenOriginalEntry(global::CardGame.Core.CardMovementRecord movement) => _resolve().BianFuRenOriginalEntry(movement);
    internal bool BianFuRenOriginalPaymentMatches(global::CardGame.Core.BianFuRenUsedMaterial material, long nativeUseId, long actionId) => _resolve().BianFuRenOriginalPaymentMatches(material, nativeUseId, actionId);
    internal global::CardGame.Core.BianFuRenUsedDiscardReceipt? BianFuRenUsedDiscard(global::CardGame.Core.ProgramSkillWindowContext context, int owner) => _resolve().BianFuRenUsedDiscard(context, owner);
    internal bool CanOfferBianFuRen(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferBianFuRen(candidate, trigger, context);
    internal void CaptureBianFuRenClaimedDeclarationPayment(long nativeUseId) => _resolve().CaptureBianFuRenClaimedDeclarationPayment(nativeUseId);
    internal void CaptureBianFuRenDelayedPlacement(global::CardGame.Core.CardMovementRecord movement) => _resolve().CaptureBianFuRenDelayedPlacement(movement);
    internal void CaptureBianFuRenForeignDiscardProducer(global::CardGame.Core.CardMovementRecord movement) => _resolve().CaptureBianFuRenForeignDiscardProducer(movement);
    internal global::CardGame.Core.CardMovementBatchContext CaptureBianFuRenMovementBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureBianFuRenMovementBatch(batch);
    internal void CaptureBianFuRenNativeUsePayment(global::CardGame.Core.CardMovementRecord movement) => _resolve().CaptureBianFuRenNativeUsePayment(movement);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectBianFuRenLossCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectBianFuRenLossCandidates(batch);
    internal void CollectWindow_CollectBianFuRenLossCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectBianFuRenLossCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureBianFuRenMovementBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureBianFuRenMovementBatch(batch, completed);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateBianFuRenMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateBianFuRenMovementContext(parent, candidate);
    internal bool ExcludeOrdinaryMovement_IsBianFuRenWanweiTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsBianFuRenWanweiTrigger(candidate, trigger);
    internal bool HasBianFuRenWanwei(int owner) => _resolve().HasBianFuRenWanwei(owner);
    internal bool HasBianFuRenYuejian(int owner) => _resolve().HasBianFuRenYuejian(owner);
    internal int[] MatchingBianFuRenUsedDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().MatchingBianFuRenUsedDiscardIndexes(batch, candidate);
    internal bool? ProveBianFuRenMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.CharacterState owner) => _resolve().ProveBianFuRenMovement(trigger, context, owner);
    internal void RecordBianFuRenForeignDiscardProducer(global::CardGame.Core.CardMovementRecord movement, int actorSeat, long producerFrameId) => _resolve().RecordBianFuRenForeignDiscardProducer(movement, actorSeat, producerFrameId);
    internal void RecordBianFuRenOwnedTransferProducer(global::CardGame.Core.CardMovementRecord movement, int recipientSeat, long producerId) => _resolve().RecordBianFuRenOwnedTransferProducer(movement, recipientSeat, producerId);
    internal void ResolveBianFuRenMovement(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().ResolveBianFuRenMovement(supplied, operation);
    internal int[]? SelectWindowIndexes_MatchingBianFuRenUsedDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingBianFuRenUsedDiscardIndexes(batch, candidate, trigger);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateBianFuRenMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateBianFuRenMovementContext(frame, candidate);
    internal void ValidateBianFuRenTrigger(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().ValidateBianFuRenTrigger(trigger, operation);
    internal bool HasBianFuRenCapability { get => _resolve().HasBianFuRenCapability; }
}
