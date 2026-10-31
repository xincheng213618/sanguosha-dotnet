#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAtomicCardMovementRuntime
{
    internal AtomicCardMovementRuntime AtomicCardMovements => _atomicCardMovementOperations.Get();

    bool global::CardGame.Core.IAtomicCardMovementFacts.TracksAdjacentDiscardStorage => _coreAdjacentDiscardStorageOperations.TracksAdjacentDiscardStorage;

    bool global::CardGame.Core.IAtomicCardMovementFacts.IsCarrier(int id) => _characterDiaoChanLeOperations.IsDiouCarrier(id);
    bool global::CardGame.Core.IAtomicCardMovementFacts.IsPurchasedEquipment(int id) => _coreWushuangShopCardsOperations.IsWushuangShopEquipment(id);
    bool global::CardGame.Core.IAtomicCardMovementFacts.IsStartingResource(int id) => _coreShanheStartingResourcesOperations.IsShanheStartingResource(id);
    bool global::CardGame.Core.IAtomicCardMovementPolicies.ClaimDeclarationPayment(Card card, CardLocation from, CardLocation to) => _coreCardDeclarationsOperations.ClaimDeclarationPayment(card, from, to);
    HappyZuoCiPhantomDestinationPlan? global::CardGame.Core.IAtomicCardMovementPolicies.PlanPhantomDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _characterZuoCiHappyGodOperations.PlanHappyZuoCiPhantomDestination(card, from, to, reason);
    CardLocation global::CardGame.Core.IAtomicCardMovementPolicies.NormalizeViewAsCostDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _coreAlternativeViewAsCostsOperations.NormalizeProgramViewAsCostDestination(card, from, to, reason);
    CardLocation global::CardGame.Core.IAtomicCardMovementPolicies.NormalizeEphemeralDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, from, to, reason);
    CardLocation global::CardGame.Core.IAtomicCardMovementPolicies.NormalizeCopiedDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _characterZhugeLiangWumiaoOperations.NormalizeWuZglCopyDestination(card, from, to, reason);
    CardLocation global::CardGame.Core.IAtomicCardMovementPolicies.NormalizeDiscardDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _characterZhongHuiAnniversaryGodOperations.NormalizeLishiDiscardDestination(card, from, to, reason);
    bool global::CardGame.Core.IAtomicCardMovementPolicies.IsGeneratedDeparture(Card card, CardLocation from, CardLocation to) => global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, from, to);
    HappyWeiYanGainDestinationPlan? global::CardGame.Core.IAtomicCardMovementPolicies.PlanGainDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _coreHappyWeiYanGainDestinationsOperations.PlanHappyWeiYanGainDestination(card, from, to, reason);
    CardLocation global::CardGame.Core.IAtomicCardMovementPolicies.NormalizeGainDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _characterWeiYanHappyGodOperations.NormalizeHappyWeiYanGainDestination(card, from, to, reason);
    bool global::CardGame.Core.IAtomicCardMovementPolicies.SettleRetainedGain(HappyWeiYanGainDestinationPlan plan) => _coreHappyWeiYanGainDestinationsOperations.TrySettleHappyWeiYanRetainedGain(plan);
    CardLocation global::CardGame.Core.IAtomicCardMovementPolicies.NormalizePhantomDestination(Card card, CardLocation from, CardLocation to) => _characterZuoCiHappyGodOperations.NormalizeHappyZuoCiPhantomDestination(card, from, to);
    bool global::CardGame.Core.IAtomicCardMovementPolicies.CanPlanDiscardTransfers(CardLocation to) => _coreHellActivitiesOperations.CanPlanHellDiscard(to);
    HellShiyouTransferPlan global::CardGame.Core.IAtomicCardMovementPolicies.PrepareDiscardTransfers(IReadOnlyList<CardTransfer> transfers, CardMoveReason reason) => _coreHellMengpoProgramsOperations.PrepareHellShiyouDiscardTransfers(transfers, reason);
    void global::CardGame.Core.IAtomicCardMovementPolicies.RecordInterceptedMovement(HellShiyouTransferPlan plan, CardMovementRecord movement, Action<CardMovementRecord>? beforeFact) => _coreHellActivitiesOperations.RecordHellInterceptedMovement(plan, movement, beforeFact);
    CardKind? global::CardGame.Core.IAtomicCardMovementPolicies.FreezeAdjacentIdentity(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _coreAdjacentDiscardStorageOperations.FreezeAdjacentDiscardPaymentIdentity(card, from, to, reason);
    void global::CardGame.Core.IAtomicCardMovementPolicies.DestroyReturnSoulIfDiscarded(Card card, CardLocation to) => _coreHellSoulCardsOperations.DestroyHellReturnSoulIfDiscarded(card, to);
    CardMovementRecord global::CardGame.Core.IAtomicCardMovementEffects.Record(Card card, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact, CardKind? adjacentDiscardIdentity, CardMovementDisclosure disclosure, HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination, HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination) => _corePhysicalMovementFacadesOperations.RecordMovement(card, from, to, reason, beforeFact, adjacentDiscardIdentity, disclosure, happyWeiYanGainDestination, happyZuoCiPhantomDestination);
    void global::CardGame.Core.IAtomicCardMovementEffects.ResolveEquipmentSkillGrant(Card card, CardLocation from, CardLocation to) => _coreEquipmentSkillsOperations.ResolveEquipmentSkillGrant(card, from, to);
    void global::CardGame.Core.IAtomicCardMovementEffects.ClearJudgmentEffectiveKind(Card card, CardLocation from, CardLocation to) => _corePhysicalMovementFacadesOperations.ClearJudgmentEffectiveKindAfterMove(card, from, to);
    void global::CardGame.Core.IAtomicCardMovementEffects.ResolveArmorRemoval(Card card, CardLocation from, CardMoveReason reason) => _corePhysicalMovementFacadesOperations.ResolveSilverLionRemoval(card, from, reason);
    void global::CardGame.Core.IAtomicCardMovementEffects.ResolveStorageEquipmentMove(Card card, CardLocation from, CardLocation to) => _corePhysicalMovementFacadesOperations.ResolveWoodenOxMove(card, from, to);
    void global::CardGame.Core.IAtomicCardMovementEffects.CollectDiscardPhaseHandDiscard(Card card, CardLocation from, CardLocation to) => _coreActualDiscardPhaseLedgerOperations.CollectDiscardPhaseHandDiscard(card, from, to);
}
