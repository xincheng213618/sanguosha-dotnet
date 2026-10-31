#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAtomicCardMovementRuntime
{
    internal AtomicCardMovementRuntime AtomicCardMovements => _atomicCardMovementOperations.Get();

    bool IAtomicCardMovementFacts.TracksAdjacentDiscardStorage => _coreAdjacentDiscardStorageOperations.TracksAdjacentDiscardStorage;

    bool IAtomicCardMovementFacts.IsCarrier(int id) => _characterDiaoChanLeOperations.IsDiouCarrier(id);
    bool IAtomicCardMovementFacts.IsPurchasedEquipment(int id) => _coreWushuangShopCardsOperations.IsWushuangShopEquipment(id);
    bool IAtomicCardMovementFacts.IsStartingResource(int id) => _coreShanheStartingResourcesOperations.IsShanheStartingResource(id);
    bool IAtomicCardMovementPolicies.ClaimDeclarationPayment(Card card, CardLocation from, CardLocation to) => _coreCardDeclarationsOperations.ClaimDeclarationPayment(card, from, to);
    HappyZuoCiPhantomDestinationPlan? IAtomicCardMovementPolicies.PlanPhantomDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _characterZuoCiHappyGodOperations.PlanHappyZuoCiPhantomDestination(card, from, to, reason);
    CardLocation IAtomicCardMovementPolicies.NormalizeViewAsCostDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _coreAlternativeViewAsCostsOperations.NormalizeProgramViewAsCostDestination(card, from, to, reason);
    CardLocation IAtomicCardMovementPolicies.NormalizeEphemeralDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, to, reason);
    CardLocation IAtomicCardMovementPolicies.NormalizeCopiedDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _characterZhugeLiangWumiaoOperations.NormalizeWuZglCopyDestination(card, from, to, reason);
    CardLocation IAtomicCardMovementPolicies.NormalizeDiscardDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _characterZhongHuiAnniversaryGodOperations.NormalizeLishiDiscardDestination(card, from, to, reason);
    bool IAtomicCardMovementPolicies.IsGeneratedDeparture(Card card, CardLocation from, CardLocation to) => CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, from, to);
    HappyWeiYanGainDestinationPlan? IAtomicCardMovementPolicies.PlanGainDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _coreHappyWeiYanGainDestinationsOperations.PlanHappyWeiYanGainDestination(card, from, to, reason);
    CardLocation IAtomicCardMovementPolicies.NormalizeGainDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _characterWeiYanHappyGodOperations.NormalizeHappyWeiYanGainDestination(card, from, to, reason);
    bool IAtomicCardMovementPolicies.SettleRetainedGain(HappyWeiYanGainDestinationPlan plan) => _coreHappyWeiYanGainDestinationsOperations.TrySettleHappyWeiYanRetainedGain2(plan);
    CardLocation IAtomicCardMovementPolicies.NormalizePhantomDestination(Card card, CardLocation from, CardLocation to) => _characterZuoCiHappyGodOperations.NormalizeHappyZuoCiPhantomDestination(card, from, to);
    bool IAtomicCardMovementPolicies.CanPlanDiscardTransfers(CardLocation to) => _coreHellActivitiesOperations.CanPlanHellDiscard(to);
    HellShiyouTransferPlan IAtomicCardMovementPolicies.PrepareDiscardTransfers(IReadOnlyList<CardTransfer> transfers, CardMoveReason reason) => _coreHellMengpoProgramsOperations.PrepareHellShiyouDiscardTransfers(transfers, reason);
    void IAtomicCardMovementPolicies.RecordInterceptedMovement(HellShiyouTransferPlan plan, CardMovementRecord movement, Action<CardMovementRecord>? beforeFact) => _coreHellActivitiesOperations.RecordHellInterceptedMovement(plan, movement, beforeFact);
    CardKind? IAtomicCardMovementPolicies.FreezeAdjacentIdentity(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _coreAdjacentDiscardStorageOperations.FreezeAdjacentDiscardPaymentIdentity(card, from, to, reason);
    void IAtomicCardMovementPolicies.DestroyReturnSoulIfDiscarded(Card card, CardLocation to) => _coreHellSoulCardsOperations.DestroyHellReturnSoulIfDiscarded(card, to);
    CardMovementRecord IAtomicCardMovementEffects.Record(Card card, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact, CardKind? adjacentDiscardIdentity, CardMovementDisclosure disclosure, HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination, HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination) => _corePhysicalMovementFacadesOperations.RecordMovement(card, from, to, reason, beforeFact, adjacentDiscardIdentity, disclosure, happyWeiYanGainDestination, happyZuoCiPhantomDestination);
    void IAtomicCardMovementEffects.ResolveEquipmentSkillGrant(Card card, CardLocation from, CardLocation to) => _coreEquipmentSkillsOperations.ResolveEquipmentSkillGrant(card, from, to);
    void IAtomicCardMovementEffects.ClearJudgmentEffectiveKind(Card card, CardLocation from, CardLocation to) => _corePhysicalMovementFacadesOperations.ClearJudgmentEffectiveKindAfterMove(card, from, to);
    void IAtomicCardMovementEffects.ResolveArmorRemoval(Card card, CardLocation from, CardMoveReason reason) => _corePhysicalMovementFacadesOperations.ResolveSilverLionRemoval(card, from, reason);
    void IAtomicCardMovementEffects.ResolveStorageEquipmentMove(Card card, CardLocation from, CardLocation to) => _corePhysicalMovementFacadesOperations.ResolveWoodenOxMove(card, from, to);
    void IAtomicCardMovementEffects.CollectDiscardPhaseHandDiscard(Card card, CardLocation from, CardLocation to) => _coreActualDiscardPhaseLedgerOperations.CollectDiscardPhaseHandDiscard(card, from, to);
}
