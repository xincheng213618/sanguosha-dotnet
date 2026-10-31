#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreResponseCompletionRuntimeServicesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreResponseCompletionRuntimeServicesCorePairedColorResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CapturePairedColorResponseLink(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreResponseCompletionRuntimeServicesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreResponseCompletionRuntimeServicesCoreDelayedNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window);
    long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window);
}

internal interface ICoreResponseCompletionRuntimeServicesCharacterZuoCiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyZuoCiDestroyedResponseFinish(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord finish);
}

internal interface ICoreResponseCompletionRuntimeServicesCharacterFanJiangZhangDaOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsJueshengDuelUse(long id);
}

internal interface ICoreResponseCompletionRuntimeServicesCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasChenglieResponseObserver(global::CardGame.Core.CardActionContext a, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreResponseCompletionRuntimeServicesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreResponseCompletionRuntimeServicesCoreWushuangJinzhiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetWushuangJinzhiResponseMovements(global::CardGame.Core.CardActionContext action, int card, out global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements);
}

internal interface ICoreResponseCompletionRuntimeServicesCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetXieweiResponseCostMovement(global::CardGame.Core.CardActionContext action, int cardId, out global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreResponseUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCompletedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreGuanduChariotResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasGuanduChariotResponsePayment(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
    global::CardGame.Core.CardMovementRecord? GuanduChariotResponsePayment(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.ProgramCardContinuation? continuation = default);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> CollectSharedCardActionCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> eventTargets, bool? cardUseCausedDamage, global::CardGame.Core.ProgramCardContinuation? continuation = default);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreTieredRoundZeroResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame ClearTieredRoundCounterspellResponse(global::CardGame.Core.NullificationWindowFrame input);
}

internal interface ICoreResponseCompletionRuntimeServicesKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(long ownerId);
    global::CardGame.Core.NativeOwners.GroupCardHandle CreateGroupCardHandle(long ownerId);
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueFinishedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreResponseCompletionRuntimeServicesCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureLeCaoZhiWenlanQualification(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreDeferredTurnEndPreludesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardsMovedTriggerWindowFrame? CreateCardsMovedProgramWindow(global::CardGame.Core.CardMovementBatchContext batch, long? resumeProgramFrameId = default);
}

internal interface ICoreResponseCompletionRuntimeServicesCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreResponseCompletionRuntimeServicesCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetSiruiDestroyedResponseCost(global::CardGame.Core.CardActionContext action, int cardId, out global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreShanheStartingResourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheStartingResourceResponseFinish(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord finish);
}

internal interface ICoreResponseCompletionRuntimeServicesCharacterZhugeLiangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuZglReturnedCopyCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord? finish = default);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNativePublicPileOriginalActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost);
}

internal interface ICoreResponseCompletionRuntimeServicesCharacterZhouCangBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBoundaryZhouCangResponseObserver(global::CardGame.Core.CardActionContext a, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueNullificationWindow(global::CardGame.Core.NullificationWindowFrame pending);
}

internal interface ICoreResponseCompletionRuntimeServicesCharacterMaMiDiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureMaMiDiResponseLink(global::CardGame.Core.ProgramCardTriggerWindowFrame w);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExchangedCardClaim(long actionId, int cardId);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreWushuangShopCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardMovementRecord? DestroyedEquipmentResponsePayment(global::CardGame.Core.CardActionContext action, long windowId, global::CardGame.Core.CardActionCost material, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreResponseCompletionRuntimeServicesCharacterLiFengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasJiyunResponseCompletion(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }

    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreResponseCompletionRuntimeServicesPendingCardsMovedBatchesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches { get; }
}

internal interface ICoreResponseCompletionRuntimeServicesCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardMovementRecord? DestroyedAlternativeResponsePayment(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.ProgramCardContinuation continuation);
    bool IsProgramAlternativeCost(global::CardGame.Core.CardActionContext? action, int cardId);
}

internal interface ICoreResponseCompletionRuntimeServicesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}
