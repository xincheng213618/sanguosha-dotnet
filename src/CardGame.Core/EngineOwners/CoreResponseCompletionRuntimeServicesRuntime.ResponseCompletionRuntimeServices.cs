#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreResponseCompletionRuntimeServicesRuntime
{
    Winner IResponseCompletionFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int IResponseCompletionFacts.TurnNumber => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    int IResponseCompletionFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;

    int IResponseCompletionFacts.FrameCount => _resolutionStackOperations._resolutionStack.Count;

    ResolutionFrame IResponseCompletionFacts.FrameAt(int index) => _resolutionStackOperations._resolutionStack[index];
    int IResponseCompletionFacts.PendingMovementBatchCount => _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Count;

    CardMovementBatchContext IResponseCompletionFacts.PendingMovementBatchAt(int index) => _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches[index];
    int IResponseCompletionFacts.PendingHpChangeCount => _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Count;

    HpChangeContext IResponseCompletionFacts.PendingHpChangeAt(int index) => _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges[index];
    bool IResponseCompletionFacts.ContainsMovement(CardMovementRecord movement) => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(movement);
    CardMovementRecord? IResponseCompletionFacts.LastMovement(Func<CardMovementRecord, bool> predicate) => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(predicate);
    bool IResponseCompletionFacts.HasTriggerWindow(SkillProgramTriggerWindow window) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerWindow(window);
    bool IResponseCompletionFacts.UsesTriggerCondition(SkillProgramTriggerConditionKind condition) => _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerCondition(condition);
    bool IResponseCompletionFacts.IsValidPlayerSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool IResponseCompletionFacts.IsAlive(int playerIndex) => _coreMatchParticipantsPlayersAccess._players[playerIndex].IsAlive;
    int IResponseCompletionFacts.PlayerSeat(int playerIndex) => _coreMatchParticipantsPlayersAccess._players[playerIndex].Seat;
    SkillProgramTriggerFacts IResponseCompletionFacts.CaptureProgramTriggerFacts(int playerIndex) => _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[playerIndex]);
    IReadOnlyList<ProgramTriggerCandidate> IResponseCompletionFacts.CollectProgramTriggerCandidates(int playerIndex, SkillProgramTriggerWindow window) => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[playerIndex], window);
    SkillProgramTrigger IResponseCompletionFacts.GetProgramTrigger(ProgramTriggerCandidate candidate) => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
    SkillProgram? IResponseCompletionFacts.Program(string skillId) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(skillId)?.Program;
    SkillProgramViewAs? IResponseCompletionFacts.ViewAsRule(CardConversionSource source) => _corePlayCardHistoryOperations.ViewAsRule(source);
    IEnumerable<IGameEvent> IResponseCompletionFacts.CompleteProgramEventHistory() => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory();
    NullificationWindowFrame? IResponseCompletionFacts.ActiveNullificationWindow => _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow;

    long? IResponseCompletionFacts.ActiveDuelOwnerId => _coreNativeCardContinuationCursorsOperations.ActiveDuel?.ResolutionId;

    int IResponseCompletionFacts.DuelOpponentSeat(long ownerId) => _kernelCoreMainOperations.CreateDuelHandle(ownerId).OpponentSeat;
    int IResponseCompletionFacts.DuelResponderSeat(long ownerId) => _kernelCoreMainOperations.CreateDuelHandle(ownerId).ResponderSeat;
    long? IResponseCompletionFacts.ActiveGroupOwnerId => _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.ResolutionId;

    int IResponseCompletionFacts.GroupSourceSeat(long ownerId) => _kernelCoreMainOperations.CreateGroupCardHandle(ownerId).SourceSeat;
    CardKind? IResponseCompletionFacts.GroupRequiredCardKind(long ownerId) => _kernelCoreMainOperations.CreateGroupCardHandle(ownerId).RequiredCardKind;
    long? IResponseCompletionFacts.GroupCurrentAttackOwnerId(long ownerId) => _kernelCoreMainOperations.CreateGroupCardHandle(ownerId).CurrentAttack?.ResolutionId;
    bool IResponseCompletionNativeProtocols.HasJiyunResponseCompletion(CardActionContext action, ProgramCardContinuation continuation) => _characterLiFengOlOperations.HasJiyunResponseCompletion(action, continuation);
    bool IResponseCompletionNativeProtocols.IsNativePublicPileOriginalActionCost(CardActionContext action, CardActionCost cost) => _coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalActionCost(action, cost);
    bool IResponseCompletionNativeProtocols.IsJueshengDuelUse(long useId) => _characterFanJiangZhangDaOlOperations.IsJueshengDuelUse(useId);
    bool IResponseCompletionNativeProtocols.HasGuanduChariotResponsePayment(CardActionContext action, ProgramCardContinuation continuation) => _coreGuanduChariotResponsesOperations.HasGuanduChariotResponsePayment(action, continuation);
    bool IResponseCompletionNativeProtocols.HasChenglieResponseObserver(CardActionContext action, ProgramCardContinuation continuation) => _characterMaChengOlOperations.HasChenglieResponseObserver(action, continuation);
    bool IResponseCompletionNativeProtocols.HasBoundaryZhouCangResponseObserver(CardActionContext action, ProgramCardContinuation continuation) => _characterZhouCangBoundaryMobileOperations.Value.HasBoundaryZhouCangResponseObserver(action, continuation);
    bool IResponseCompletionNativeProtocols.TryGetXieweiResponseCostMovement(CardActionContext action, int cardId, out CardMovementRecord movement) => _characterGuanHaiOlOperations.TryGetXieweiResponseCostMovement(action, cardId, out movement);
    bool IResponseCompletionNativeProtocols.TryGetSiruiDestroyedResponseCost(CardActionContext action, int cardId, out CardMovementRecord movement) => _characterMiHengLeOperations.TryGetSiruiDestroyedResponseCost(action, cardId, out movement);
    bool IResponseCompletionNativeProtocols.TryGetWushuangJinzhiResponseMovements(CardActionContext action, int cardId, out IReadOnlyList<CardMovementRecord> movements) => _coreWushuangJinzhiOperations.TryGetWushuangJinzhiResponseMovements(action, cardId, out movements);
    CardMovementRecord? IResponseCompletionNativeProtocols.GuanduChariotResponsePayment(CardActionContext action, long parentFrameId, CardActionCost cost, ProgramCardContinuation continuation) => _coreGuanduChariotResponsesOperations.GuanduChariotResponsePayment(action, parentFrameId, cost, continuation);
    CardMovementRecord? IResponseCompletionNativeProtocols.DestroyedEquipmentResponsePayment(CardActionContext action, long parentFrameId, CardActionCost cost, ProgramCardContinuation continuation) => _coreWushuangShopCardsOperations.DestroyedEquipmentResponsePayment(action, parentFrameId, cost, continuation) ?? _coreAlternativeViewAsCostsOperations.DestroyedAlternativeResponsePayment(action, parentFrameId, cost, continuation);
    bool IResponseCompletionNativeProtocols.IsProgramAlternativeCost(CardActionContext action, int cardId) => _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, cardId);
    bool IResponseCompletionNativeProtocols.IsExchangedCardClaim(long actionId, int cardId) => _coreResponseEntityExchangeOperations.IsExchangedCardClaim(actionId, cardId);
    bool IResponseCompletionNativeProtocols.IsWuZglReturnedCopyCost(CardActionContext action, CardActionCost cost, CardMovementRecord finish) => _characterZhugeLiangWumiaoOperations.IsWuZglReturnedCopyCost(action, cost, finish);
    bool IResponseCompletionNativeProtocols.IsShanheStartingResourceResponseFinish(CardActionContext action, CardActionCost cost, CardMovementRecord finish) => _coreShanheStartingResourcesOperations.IsShanheStartingResourceResponseFinish(action, cost, finish);
    bool IResponseCompletionNativeProtocols.IsHappyZuoCiDestroyedResponseFinish(CardActionContext action, CardActionCost cost, CardMovementRecord finish) => _characterZuoCiHappyGodOperations.IsHappyZuoCiDestroyedResponseFinish(action, cost, finish);
    bool IResponseCompletionNativeProtocols.HasNullificationEffectOwner(NullificationWindowFrame window) => _coreDelayedNullificationOperations.Value.HasNullificationEffectOwner(window);
    long? IResponseCompletionNativeProtocols.NullificationParentActionId(NullificationWindowFrame window) => _coreDelayedNullificationOperations.Value.NullificationParentActionId(window);
    void IResponseCompletionNativeProtocols.CapturePairedColorResponseLink(ProgramCardTriggerWindowFrame window) => _corePairedColorResponsesOperations.CapturePairedColorResponseLink(window);
    void IResponseCompletionNativeProtocols.CaptureMaMiDiResponseLink(ProgramCardTriggerWindowFrame window) => _characterMaMiDiOlOperations.CaptureMaMiDiResponseLink(window);
    bool IResponseCompletionNativeProtocols.RemovePendingHpChange(HpChangeContext change) => _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Remove(change);
    void IResponseCompletionNativeProtocols.RemovePendingMovementBatch(CardMovementBatchContext batch) => _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Remove(batch);
    CardsMovedTriggerWindowFrame? IResponseCompletionNativeProtocols.CreateCardsMovedProgramWindow(CardMovementBatchContext batch, long? resumeProgramFrameId) => _coreDeferredTurnEndPreludesOperations.CreateCardsMovedProgramWindow(batch, resumeProgramFrameId);
    void IResponseCompletionNativeProtocols.CaptureLeCaoZhiWenlanQualification(CardActionContext action, SkillProgramTriggerWindow window) => _characterCaoZhiLeOperations.CaptureLeCaoZhiWenlanQualification(action, window);
    IReadOnlyList<ProgramCardTriggerCandidate> IResponseCompletionNativeProtocols.CollectSharedCardActionCandidates(CardActionContext action, SkillProgramTriggerWindow window, IReadOnlyList<int> eventTargets, bool? cardUseCausedDamage) => _coreCardActionsOperations.CollectSharedCardActionCandidates(action, window, eventTargets, cardUseCausedDamage);
    bool IResponseCompletionNativeProtocols.TryBeginCompletedResponseUsePrograms(long? attackOwnerFrameId, CardActionContext action, ProgramCardContinuation continuation) => _coreResponseUseCompletionOperations.TryBeginCompletedResponseUsePrograms(attackOwnerFrameId is { } id ? _kernelCoreMainOperations.CreateCardAttackHandle(id) : null, action, continuation);
    NullificationWindowFrame IResponseCompletionNativeProtocols.ClearTieredRoundCounterspellResponse(NullificationWindowFrame window) => _coreTieredRoundZeroResponsesOperations.ClearTieredRoundCounterspellResponse(window);
    void IResponseCompletionNativeProtocols.ContinueNullificationWindow(NullificationWindowFrame window) => _coreNativeNullificationOperations.ContinueNullificationWindow(window);
    void IResponseCompletionNativeProtocols.ContinueFinishedCardResponse(long attackOwnerFrameId, CardActionContext action, ProgramCardContinuation continuation) => _coreNativeDodgeOperations.ContinueFinishedCardResponse(_kernelCoreMainOperations.CreateCardAttackHandle(attackOwnerFrameId), action, continuation);
}
