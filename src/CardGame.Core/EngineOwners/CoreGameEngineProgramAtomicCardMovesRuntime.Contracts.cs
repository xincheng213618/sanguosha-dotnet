#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramAtomicCardMovesCoreHellMengpoProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HellShiyouTransferPlan PrepareHellShiyouDiscardTransfers(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardTransfer> input, global::CardGame.Core.CardMoveReason reason);
    void CompleteHellShiyouDiscardTransfers(global::CardGame.Core.HellShiyouTransferPlan plan, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements);
    bool IsHellShiyouPaidDiscardMovement(global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.CardLocation expectedSource, global::CardGame.Core.CardMoveReason expectedReason);
}

internal interface ICoreProgramAtomicCardMovesCharacterZuoCiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HappyZuoCiPhantomDestinationPlan? PlanHappyZuoCiPhantomDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation requestedTo, global::CardGame.Core.CardMoveReason reason);
    global::CardGame.Core.CardLocation NormalizeHappyZuoCiPhantomDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    bool IsHappyZuoCiDestroyedMaterial(global::CardGame.Core.CardMovementRecord m);
}

internal interface ICoreProgramAtomicCardMovesCoreActualDiscardPhaseLedgerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CollectDiscardPhaseHandDiscard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreProgramAtomicCardMovesCoreHellActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanPlanHellDiscard(global::CardGame.Core.CardLocation destination);
}

internal interface ICoreProgramAtomicCardMovesCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void DestroyHellReturnSoulIfDiscarded(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation to);
}

internal interface ICoreProgramAtomicCardMovesCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeEphemeralCardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason = default);
}

internal interface ICoreProgramAtomicCardMovesCharacterZhongHuiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeLishiDiscardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason);
    bool IsLishiPaidDiscardMovement(global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.CardLocation expectedSource, global::CardGame.Core.CardMoveReason expectedReason);
}

internal interface ICoreProgramAtomicCardMovesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreProgramAtomicCardMovesCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardMovementRecord RecordMovement(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, global::CardGame.Core.CardKind? adjacentDiscardIdentity = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::CardGame.Core.HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination = default, global::CardGame.Core.HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination = default);
    void ClearJudgmentEffectiveKindAfterMove(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    void ResolveSilverLionRemoval(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason);
    void ResolveWoodenOxMove(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreProgramAtomicCardMovesCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HappyWeiYanGainDestinationPlan? PlanHappyWeiYanGainDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation requestedTo, global::CardGame.Core.CardMoveReason reason);
    global::CardGame.Core.HappyWeiYanRetainedGainSettlement SettleHappyWeiYanRetainedGain(global::CardGame.Core.HappyWeiYanGainDestinationPlan plan, long? producerFrameId = default);
}

internal interface ICoreProgramAtomicCardMovesCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardMovementBatchBuilder BeginCardMovementBatch(global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> sources, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> destinations);
    void CompleteCardMovementBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements, bool committed);
}

internal interface ICoreProgramAtomicCardMovesCoreEquipmentSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveEquipmentSkillGrant(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreProgramAtomicCardMovesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}
