#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBianFuRenOlEntries
{
    internal bool BianFuRenDelayedFinish(BianFuRenUsedMaterial material, CardMovementRecord finish) => _resolve().BianFuRenDelayedFinish(material, finish);
    internal BianFuRenOwnedLossReceipt? BianFuRenLoss(ProgramSkillWindowContext context, int owner) => _resolve().BianFuRenLoss(context, owner);
    internal bool BianFuRenMaterialAvailable(BianFuRenUsedMaterial material) => _resolve().BianFuRenMaterialAvailable(material);
    internal long BianFuRenObtainProducer(CardMovementBatchContext batch, CardMovementRecord entry, CardMovementRecord completion) => _resolve().BianFuRenObtainProducer(batch, entry, completion);
    internal CardMovementRecord? BianFuRenOriginalEntry(CardMovementRecord movement) => _resolve().BianFuRenOriginalEntry(movement);
    internal bool BianFuRenOriginalPaymentMatches(BianFuRenUsedMaterial material, long nativeUseId, long actionId) => _resolve().BianFuRenOriginalPaymentMatches(material, nativeUseId, actionId);
    internal BianFuRenUsedDiscardReceipt? BianFuRenUsedDiscard(ProgramSkillWindowContext context, int owner) => _resolve().BianFuRenUsedDiscard(context, owner);
    internal bool CanOfferBianFuRen(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferBianFuRen(candidate, trigger, context);
    internal void CaptureBianFuRenClaimedDeclarationPayment(long nativeUseId) => _resolve().CaptureBianFuRenClaimedDeclarationPayment(nativeUseId);
    internal void CaptureBianFuRenDelayedPlacement(CardMovementRecord movement) => _resolve().CaptureBianFuRenDelayedPlacement(movement);
    internal void CaptureBianFuRenForeignDiscardProducer(CardMovementRecord movement) => _resolve().CaptureBianFuRenForeignDiscardProducer(movement);
    internal CardMovementBatchContext CaptureBianFuRenMovementBatch(CardMovementBatchContext batch) => _resolve().CaptureBianFuRenMovementBatch(batch);
    internal void CaptureBianFuRenNativeUsePayment(CardMovementRecord movement) => _resolve().CaptureBianFuRenNativeUsePayment(movement);
    internal IEnumerable<ProgramTriggerCandidate> CollectBianFuRenLossCandidates(CardMovementBatchContext batch) => _resolve().CollectBianFuRenLossCandidates(batch);
    internal void CollectWindow_CollectBianFuRenLossCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectBianFuRenLossCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureBianFuRenMovementBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureBianFuRenMovementBatch(batch, completed);
    internal ProgramSkillWindowContext CreateBianFuRenMovementContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate candidate) => _resolve().CreateBianFuRenMovementContext(parent, candidate);
    internal bool ExcludeOrdinaryMovement_IsBianFuRenWanweiTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsBianFuRenWanweiTrigger(candidate, trigger);
    internal bool HasBianFuRenWanwei(int owner) => _resolve().HasBianFuRenWanwei(owner);
    internal bool HasBianFuRenYuejian(int owner) => _resolve().HasBianFuRenYuejian(owner);
    internal int[] MatchingBianFuRenUsedDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingBianFuRenUsedDiscardIndexes(batch, candidate);
    internal bool? ProveBianFuRenMovement(SkillProgramTrigger trigger, ProgramSkillWindowContext context, CharacterState owner) => _resolve().ProveBianFuRenMovement(trigger, context, owner);
    internal void RecordBianFuRenForeignDiscardProducer(CardMovementRecord movement, int actorSeat, long producerFrameId) => _resolve().RecordBianFuRenForeignDiscardProducer(movement, actorSeat, producerFrameId);
    internal void RecordBianFuRenOwnedTransferProducer(CardMovementRecord movement, int recipientSeat, long producerId) => _resolve().RecordBianFuRenOwnedTransferProducer(movement, recipientSeat, producerId);
    internal void ResolveBianFuRenMovement(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().ResolveBianFuRenMovement(supplied, operation);
    internal int[]? SelectWindowIndexes_MatchingBianFuRenUsedDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingBianFuRenUsedDiscardIndexes(batch, candidate, trigger);
    internal ProgramSkillWindowContext? TryCreateBianFuRenMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateBianFuRenMovementContext(frame, candidate);
    internal void ValidateBianFuRenTrigger(SkillProgramTrigger trigger, SkillProgramEffectOp operation) => _resolve().ValidateBianFuRenTrigger(trigger, operation);
    internal bool HasBianFuRenCapability { get => _resolve().HasBianFuRenCapability; }
}
