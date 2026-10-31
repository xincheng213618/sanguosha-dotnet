#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreAtomicCardMovementCharacterZhongHuiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeLishiDiscardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreAtomicCardMovementCoreActualDiscardPhaseLedgerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CollectDiscardPhaseHandDiscard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreAtomicCardMovementCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void DestroyHellReturnSoulIfDiscarded(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation to);
}

internal interface ICoreAtomicCardMovementCoreWushuangShopCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangShopEquipment(int cardId);
}

internal interface ICoreAtomicCardMovementCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeProgramViewAsCostDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreAtomicCardMovementAtomicCardMovementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.AtomicCardMovementRuntime Get();
}

internal interface ICoreAtomicCardMovementCoreHellMengpoProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HellShiyouTransferPlan PrepareHellShiyouDiscardTransfers(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardTransfer> input, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreAtomicCardMovementCoreShanheStartingResourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheStartingResource(int id);
}

internal interface ICoreAtomicCardMovementCharacterZhugeLiangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeWuZglCopyDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreAtomicCardMovementCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ClaimDeclarationPayment(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreAtomicCardMovementCoreEquipmentSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveEquipmentSkillGrant(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreAtomicCardMovementCharacterZuoCiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HappyZuoCiPhantomDestinationPlan? PlanHappyZuoCiPhantomDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation requestedTo, global::CardGame.Core.CardMoveReason reason);
    global::CardGame.Core.CardLocation NormalizeHappyZuoCiPhantomDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreAtomicCardMovementCharacterWeiYanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeHappyWeiYanGainDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreAtomicCardMovementCoreHellActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanPlanHellDiscard(global::CardGame.Core.CardLocation destination);
    void RecordHellInterceptedMovement(global::CardGame.Core.HellShiyouTransferPlan plan, global::CardGame.Core.CardMovementRecord move, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact);
}

internal interface ICoreAtomicCardMovementCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HappyWeiYanGainDestinationPlan? PlanHappyWeiYanGainDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation requestedTo, global::CardGame.Core.CardMoveReason reason);
    bool TrySettleHappyWeiYanRetainedGain(global::CardGame.Core.HappyWeiYanGainDestinationPlan plan, long? producerFrameId = default);
}

internal interface ICoreAtomicCardMovementCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDiouCarrier(int id);
}

internal interface ICoreAtomicCardMovementCoreAdjacentDiscardStorageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksAdjacentDiscardStorage { get; }

    global::CardGame.Core.CardKind? FreezeAdjacentDiscardPaymentIdentity(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreAtomicCardMovementCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardMovementRecord RecordMovement(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, global::CardGame.Core.CardKind? adjacentDiscardIdentity = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::CardGame.Core.HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination = default, global::CardGame.Core.HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination = default);
    void ClearJudgmentEffectiveKindAfterMove(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    void ResolveSilverLionRemoval(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason);
    void ResolveWoodenOxMove(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreAtomicCardMovementCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeEphemeralCardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason = default);
}
