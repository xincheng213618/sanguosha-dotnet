#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreResponseCompletionRuntimeServicesRuntime
{
    Winner global::CardGame.Core.IResponseCompletionFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int global::CardGame.Core.IResponseCompletionFacts.TurnNumber => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    int global::CardGame.Core.IResponseCompletionFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;

    int global::CardGame.Core.IResponseCompletionFacts.FrameCount => _resolutionStackOperations._resolutionStack.Count;

    ResolutionFrame global::CardGame.Core.IResponseCompletionFacts.FrameAt(int index) => _resolutionStackOperations._resolutionStack[index];
    int global::CardGame.Core.IResponseCompletionFacts.PendingMovementBatchCount => _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Count;

    CardMovementBatchContext global::CardGame.Core.IResponseCompletionFacts.PendingMovementBatchAt(int index) => _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches[index];
    int global::CardGame.Core.IResponseCompletionFacts.PendingHpChangeCount => _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Count;

    HpChangeContext global::CardGame.Core.IResponseCompletionFacts.PendingHpChangeAt(int index) => _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges[index];
    bool global::CardGame.Core.IResponseCompletionFacts.ContainsMovement(CardMovementRecord movement) => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(movement);
    CardMovementRecord? global::CardGame.Core.IResponseCompletionFacts.LastMovement(Func<CardMovementRecord, bool> predicate) => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(predicate);
    bool global::CardGame.Core.IResponseCompletionFacts.HasTriggerWindow(SkillProgramTriggerWindow window) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerWindow(window);
    bool global::CardGame.Core.IResponseCompletionFacts.UsesTriggerCondition(SkillProgramTriggerConditionKind condition) => _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerCondition(condition);
    bool global::CardGame.Core.IResponseCompletionFacts.IsValidPlayerSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool global::CardGame.Core.IResponseCompletionFacts.IsAlive(int playerIndex) => _coreMatchParticipantsPlayersAccess._players[playerIndex].IsAlive;
    int global::CardGame.Core.IResponseCompletionFacts.PlayerSeat(int playerIndex) => _coreMatchParticipantsPlayersAccess._players[playerIndex].Seat;
    SkillProgramTriggerFacts global::CardGame.Core.IResponseCompletionFacts.CaptureProgramTriggerFacts(int playerIndex) => _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[playerIndex]);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IResponseCompletionFacts.CollectProgramTriggerCandidates(int playerIndex, SkillProgramTriggerWindow window) => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[playerIndex], window);
    SkillProgramTrigger global::CardGame.Core.IResponseCompletionFacts.GetProgramTrigger(ProgramTriggerCandidate candidate) => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
    SkillProgram? global::CardGame.Core.IResponseCompletionFacts.Program(string skillId) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(skillId)?.Program;
    SkillProgramViewAs? global::CardGame.Core.IResponseCompletionFacts.ViewAsRule(CardConversionSource source) => _corePlayCardHistoryOperations.ViewAsRule(source);
    IEnumerable<IGameEvent> global::CardGame.Core.IResponseCompletionFacts.CompleteProgramEventHistory() => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory();
    NullificationWindowFrame? global::CardGame.Core.IResponseCompletionFacts.ActiveNullificationWindow => _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow;

    long? global::CardGame.Core.IResponseCompletionFacts.ActiveDuelOwnerId => _coreNativeCardContinuationCursorsOperations.ActiveDuel?.ResolutionId;

    int global::CardGame.Core.IResponseCompletionFacts.DuelOpponentSeat(long ownerId) => _kernelCoreMainOperations.CreateDuelHandle(ownerId).OpponentSeat;
    int global::CardGame.Core.IResponseCompletionFacts.DuelResponderSeat(long ownerId) => _kernelCoreMainOperations.CreateDuelHandle(ownerId).ResponderSeat;
    long? global::CardGame.Core.IResponseCompletionFacts.ActiveGroupOwnerId => _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.ResolutionId;

    int global::CardGame.Core.IResponseCompletionFacts.GroupSourceSeat(long ownerId) => _kernelCoreMainOperations.CreateGroupCardHandle(ownerId).SourceSeat;
    CardKind? global::CardGame.Core.IResponseCompletionFacts.GroupRequiredCardKind(long ownerId) => _kernelCoreMainOperations.CreateGroupCardHandle(ownerId).RequiredCardKind;
    long? global::CardGame.Core.IResponseCompletionFacts.GroupCurrentAttackOwnerId(long ownerId) => _kernelCoreMainOperations.CreateGroupCardHandle(ownerId).CurrentAttack?.ResolutionId;
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.HasJiyunResponseCompletion(CardActionContext action, ProgramCardContinuation continuation) => _characterLiFengOlOperations.HasJiyunResponseCompletion(action, continuation);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.IsNativePublicPileOriginalActionCost(CardActionContext action, CardActionCost cost) => _coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalActionCost(action, cost);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.IsJueshengDuelUse(long useId) => _characterFanJiangZhangDaOlOperations.IsJueshengDuelUse(useId);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.HasGuanduChariotResponsePayment(CardActionContext action, ProgramCardContinuation continuation) => _coreGuanduChariotResponsesOperations.HasGuanduChariotResponsePayment(action, continuation);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.HasChenglieResponseObserver(CardActionContext action, ProgramCardContinuation continuation) => _characterMaChengOlOperations.HasChenglieResponseObserver(action, continuation);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.HasBoundaryZhouCangResponseObserver(CardActionContext action, ProgramCardContinuation continuation) => _characterZhouCangBoundaryMobileOperations.HasBoundaryZhouCangResponseObserver(action, continuation);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.TryGetXieweiResponseCostMovement(CardActionContext action, int cardId, out CardMovementRecord movement) => _characterGuanHaiOlOperations.TryGetXieweiResponseCostMovement(action, cardId, out movement);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.TryGetSiruiDestroyedResponseCost(CardActionContext action, int cardId, out CardMovementRecord movement) => _characterMiHengLeOperations.TryGetSiruiDestroyedResponseCost(action, cardId, out movement);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.TryGetWushuangJinzhiResponseMovements(CardActionContext action, int cardId, out IReadOnlyList<CardMovementRecord> movements) => _coreWushuangJinzhiOperations.TryGetWushuangJinzhiResponseMovements(action, cardId, out movements);
    CardMovementRecord? global::CardGame.Core.IResponseCompletionNativeProtocols.GuanduChariotResponsePayment(CardActionContext action, long parentFrameId, CardActionCost cost, ProgramCardContinuation continuation) => _coreGuanduChariotResponsesOperations.GuanduChariotResponsePayment(action, parentFrameId, cost, continuation);
    CardMovementRecord? global::CardGame.Core.IResponseCompletionNativeProtocols.DestroyedEquipmentResponsePayment(CardActionContext action, long parentFrameId, CardActionCost cost, ProgramCardContinuation continuation) => _coreWushuangShopCardsOperations.DestroyedEquipmentResponsePayment(action, parentFrameId, cost, continuation) ?? _coreAlternativeViewAsCostsOperations.DestroyedAlternativeResponsePayment(action, parentFrameId, cost, continuation);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.IsProgramAlternativeCost(CardActionContext action, int cardId) => _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, cardId);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.IsExchangedCardClaim(long actionId, int cardId) => _coreResponseEntityExchangeOperations.IsExchangedCardClaim(actionId, cardId);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.IsWuZglReturnedCopyCost(CardActionContext action, CardActionCost cost, CardMovementRecord finish) => _characterZhugeLiangWumiaoOperations.IsWuZglReturnedCopyCost(action, cost, finish);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.IsShanheStartingResourceResponseFinish(CardActionContext action, CardActionCost cost, CardMovementRecord finish) => _coreShanheStartingResourcesOperations.IsShanheStartingResourceResponseFinish(action, cost, finish);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.IsHappyZuoCiDestroyedResponseFinish(CardActionContext action, CardActionCost cost, CardMovementRecord finish) => _characterZuoCiHappyGodOperations.IsHappyZuoCiDestroyedResponseFinish(action, cost, finish);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.HasNullificationEffectOwner(NullificationWindowFrame window) => _coreDelayedNullificationOperations.HasNullificationEffectOwner(window);
    long? global::CardGame.Core.IResponseCompletionNativeProtocols.NullificationParentActionId(NullificationWindowFrame window) => _coreDelayedNullificationOperations.NullificationParentActionId(window);
    void global::CardGame.Core.IResponseCompletionNativeProtocols.CapturePairedColorResponseLink(ProgramCardTriggerWindowFrame window) => _corePairedColorResponsesOperations.CapturePairedColorResponseLink(window);
    void global::CardGame.Core.IResponseCompletionNativeProtocols.CaptureMaMiDiResponseLink(ProgramCardTriggerWindowFrame window) => _characterMaMiDiOlOperations.CaptureMaMiDiResponseLink(window);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.RemovePendingHpChange(HpChangeContext change) => _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Remove(change);
    void global::CardGame.Core.IResponseCompletionNativeProtocols.RemovePendingMovementBatch(CardMovementBatchContext batch) => _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Remove(batch);
    CardsMovedTriggerWindowFrame? global::CardGame.Core.IResponseCompletionNativeProtocols.CreateCardsMovedProgramWindow(CardMovementBatchContext batch, long? resumeProgramFrameId) => _coreDeferredTurnEndPreludesOperations.CreateCardsMovedProgramWindow(batch, resumeProgramFrameId);
    void global::CardGame.Core.IResponseCompletionNativeProtocols.CaptureLeCaoZhiWenlanQualification(CardActionContext action, SkillProgramTriggerWindow window) => _characterCaoZhiLeOperations.CaptureLeCaoZhiWenlanQualification(action, window);
    IReadOnlyList<ProgramCardTriggerCandidate> global::CardGame.Core.IResponseCompletionNativeProtocols.CollectSharedCardActionCandidates(CardActionContext action, SkillProgramTriggerWindow window, IReadOnlyList<int> eventTargets, bool? cardUseCausedDamage) => _coreCardActionsOperations.CollectSharedCardActionCandidates(action, window, eventTargets, cardUseCausedDamage);
    bool global::CardGame.Core.IResponseCompletionNativeProtocols.TryBeginCompletedResponseUsePrograms(long? attackOwnerFrameId, CardActionContext action, ProgramCardContinuation continuation) => _coreResponseUseCompletionOperations.TryBeginCompletedResponseUsePrograms(attackOwnerFrameId is { } id ? _kernelCoreMainOperations.CreateCardAttackHandle(id) : null, action, continuation);
    NullificationWindowFrame global::CardGame.Core.IResponseCompletionNativeProtocols.ClearTieredRoundCounterspellResponse(NullificationWindowFrame window) => _coreTieredRoundZeroResponsesOperations.ClearTieredRoundCounterspellResponse(window);
    void global::CardGame.Core.IResponseCompletionNativeProtocols.ContinueNullificationWindow(NullificationWindowFrame window) => _coreNativeNullificationOperations.ContinueNullificationWindow(window);
    void global::CardGame.Core.IResponseCompletionNativeProtocols.ContinueFinishedCardResponse(long attackOwnerFrameId, CardActionContext action, ProgramCardContinuation continuation) => _coreNativeDodgeOperations.ContinueFinishedCardResponse(_kernelCoreMainOperations.CreateCardAttackHandle(attackOwnerFrameId), action, continuation);
}
