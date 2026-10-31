#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreCardsMovedWindowRuntimePortsRuntime
{
    Winner ICardsMovedWindowFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus ICardsMovedWindowFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    TurnPhase ICardsMovedWindowFacts.Phase => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase;

    int ICardsMovedWindowFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;

    int ICardsMovedWindowFacts.TurnNumber => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    int ICardsMovedWindowFacts.PlayerCount => _coreMatchParticipantsPlayersAccess._players.Count;

    bool ICardsMovedWindowFacts.IsValidPlayerSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool ICardsMovedWindowFacts.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool ICardsMovedWindowFacts.HasActiveJudgment => _coreJudgmentCursorRuntimeOperations.Value.ActiveJudgment is not null;

    SkillProgramTrigger ICardsMovedWindowFacts.GetProgramTrigger(ProgramTriggerCandidate candidate) => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
    IEnumerable<(CardMovementSourceCount Count, bool DiscardOriginOnly)> ICardsMovedWindowFacts.ProgramMovementSourceCounts(CardMovementBatchContext batch, SkillProgramTriggerWindow window) => _coreProgramMovementSourceCountsOperations.ProgramMovementSourceCounts(batch, window);
    bool ICardsMovedWindowFacts.ContainsMovement(CardMovementRecord movement) => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(movement);
    CardLocation? ICardsMovedWindowFacts.GetProgramDiscardSource(CardMovementRecord movement) => _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement);
    PublicPersistentPileSource? ICardsMovedWindowFacts.RicePile(ProgramTriggerCandidate candidate) => _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId));
    bool ICardsMovedWindowFacts.RiceProducer(PublicPersistentPileSource pile) => _coreRicePileOperations.RiceProducer(pile);
    CardKind ICardsMovedWindowFacts.GetAttackCardKind(int cardId) => _coreCardAttackStateOperations.GetAttackCard(cardId).Kind;
    string ICardsMovedWindowFacts.PlayerName(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    string ICardsMovedWindowFacts.SkillName(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId).Name;
    SkillProgramTriggerFacts IProgramTriggerQueries.CaptureProgramTriggerFacts(int ownerSeat) => _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[ownerSeat]);
    IReadOnlyList<ProgramTriggerCandidate> IProgramTriggerQueries.CollectProgramTriggerCandidates(int ownerSeat, SkillProgramTriggerWindow window) => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[ownerSeat], window);
    bool ICardsMovedWindowNativeProofs.IsShanheNullifiedClaimAwaitingMovement(NullificationWindowFrame window) => _coreShanheNullifiedTrickClaimsOperations.IsShanheNullifiedClaimAwaitingMovement(window);
    bool ICardsMovedWindowNativeProofs.IsShanheNullifiedClaimMovement(NullificationWindowFrame window, CardMovementBatchContext batch) => _coreShanheNullifiedTrickClaimsOperations.IsShanheNullifiedClaimMovement(window, batch);
    bool ICardsMovedWindowNativeProofs.IsKemingEchoMovement(KemingSkillEchoFrame f, CardMovementBatchContext b) => _characterCaoPiAnniversaryGodOperations.IsKemingEchoMovement(f, b);
    bool ICardsMovedWindowNativeProofs.IsSkillInvocationMovementBatch(SkillInvocationFrame f, CardMovementBatchContext batch) => _characterHuaTuoAnniversaryGodOperations.IsSkillInvocationMovementBatch(f, batch);
    bool ICardsMovedWindowNativeProofs.IsRoundEndingMovement(RoundEndingBoundaryFrame parent, CardMovementBatchContext batch) => _coreRoundEndingOperations.Value.IsRoundEndingMovement(parent, batch);
    bool ICardsMovedWindowNativeProofs.IsNianRevivalMovementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => _coreNianRevivalOperations.Value.IsNianRevivalMovementBatch(f, batch);
    bool ICardsMovedWindowCardUseProofs.HasPaidCardUseMovementTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => _coreProgramCardUsePaymentsOperations.Value.HasPaidCardUseMovementTail(use, continuation);
    bool ICardsMovedWindowCardUseProofs.IsCardUsePaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => _coreCardUsePaymentsOperations.IsCardUsePaymentBatch(use, batch);
    bool ICardsMovedWindowCardUseProofs.IsSoleIncomingRangeDrawMovement(CardUseFrame use, CardMovementBatchContext batch) => _coreSoleIncomingRangeDrawOperations.IsSoleIncomingRangeDrawMovement(use, batch);
    bool ICardsMovedWindowCardUseProofs.IsWumiaoFloodMovement(CardUseFrame use, CardMovementBatchContext batch) => _characterGuanYuWumiaoOperations.IsWumiaoFloodMovement(use, batch);
    bool ICardsMovedWindowCardUseProofs.IsGuanduCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => _coreGuanduCardsOperations.IsGuanduCardEffectMovement(use, batch);
    bool ICardsMovedWindowCardUseProofs.IsDragonBoatCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => _coreDragonBoatCardsOperations.IsDragonBoatCardEffectMovement(use, batch);
    bool ICardsMovedWindowCardUseProofs.IsWenheCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => _coreWenheCardsOperations.IsWenheCardEffectMovement(use, batch);
    bool ICardsMovedWindowCardUseProofs.IsNativeCardEffectDrawMovement(CardUseFrame use, CardMovementBatchContext batch) => _coreNativeCardEffectDrawsOperations.IsNativeCardEffectDrawMovement(use, batch);
    bool ICardsMovedWindowCardUseProofs.IsGuanduCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => _coreGuanduCardsOperations.IsGuanduCardEffectMovementReturn(use, frame);
    bool ICardsMovedWindowCardUseProofs.IsWumiaoFloodMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => _characterGuanYuWumiaoOperations.IsWumiaoFloodMovementReturn(use, frame);
    bool ICardsMovedWindowCardUseProofs.IsDragonBoatCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => _coreDragonBoatCardsOperations.IsDragonBoatCardEffectMovementReturn(use, frame);
    bool ICardsMovedWindowCardUseProofs.IsWenheCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => _coreWenheCardsOperations.IsWenheCardEffectMovementReturn(use, movement);
    bool ICardsMovedWindowCardUseProofs.IsSoleIncomingRangeDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame window) => _coreSoleIncomingRangeDrawOperations.IsSoleIncomingRangeDrawMovementReturn(use, window);
    bool ICardsMovedWindowCardUseProofs.IsNativeCardEffectDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => _coreNativeCardEffectDrawsOperations.IsNativeCardEffectDrawMovementReturn(use, frame);
    bool ICardsMovedWindowCardUseProofs.IsFriendDuelResponseMovementReturn(FriendDuelResponseFrame parent, CardsMovedTriggerWindowFrame movement) => _coreFriendDuelResponsesOperations.IsFriendDuelResponseMovementReturn(parent, movement);
    bool ICardsMovedWindowCardUseProofs.IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => _coreCardUsePaymentsOperations.IsPaidCardUseMovementReturn(use, movement);
    void ICardsMovedWindowNativeReturns.ReturnShanheNullifiedClaimMovement(CardsMovedTriggerWindowFrame child) => _coreShanheNullifiedTrickClaimsOperations.ReturnShanheNullifiedClaimMovement(child);
    void ICardsMovedWindowNativeReturns.ReturnHuxinMirrorPlacementMovement(CardsMovedTriggerWindowFrame window) => _coreHuxinMirrorOperations.ReturnHuxinMirrorPlacementMovement(window);
    void ICardsMovedWindowNativeReturns.ReturnKemingEchoMovement(CardsMovedTriggerWindowFrame window) => _characterCaoPiAnniversaryGodOperations.ReturnKemingEchoMovement(window);
    bool ICardsMovedWindowNativeReturns.ReturnWenhePoisonFireAttackMovement(CardsMovedTriggerWindowFrame child) => _coreWenhePoisonOperations.ReturnWenhePoisonFireAttackMovement(child);
    bool ICardsMovedWindowNativeReturns.ReturnQiaoZhouPreparationMovement(CardsMovedTriggerWindowFrame child) => _characterQiaoZhouOlOperations.ReturnQiaoZhouPreparationMovement(child);
    bool ICardsMovedWindowNativeReturns.ReturnCardResponseCompletionMovement(CardsMovedTriggerWindowFrame child) => _coreResponseCompletionOperations.ReturnCardResponseCompletionMovement(child);
    bool ICardsMovedWindowNativeReturns.ReturnCardSupplyCompletionMovement(CardsMovedTriggerWindowFrame child) => _coreCardSupplyCompletionOperations.ReturnCardSupplyCompletionMovement(child);
    void ICardsMovedWindowNativeReturns.ReturnDeferredTurnEndPrelude(CardsMovedTriggerWindowFrame child) => _coreDeferredTurnEndPreludesOperations.ReturnDeferredTurnEndPrelude(child);
    void ICardsMovedWindowNativeReturns.ReturnSoleTargetQuarantineMovement(CardsMovedTriggerWindowFrame child) => _coreSoleTargetQuarantineOperations.ReturnSoleTargetQuarantineMovement(child);
    void ICardsMovedWindowNativeReturns.ResumeDrawFundedDistinctBasicMovement(CardsMovedTriggerWindowFrame completed) => _coreDrawFundedDistinctBasicPaymentOperations.ResumeDrawFundedDistinctBasicMovement(completed);
    void ICardsMovedWindowNativeReturns.ReturnRoundEndingMovement(CardsMovedTriggerWindowFrame child) => _coreRoundEndingOperations.Value.ReturnRoundEndingMovement(child);
    void ICardsMovedWindowNativeReturns.ReturnRuntimeProgramMovement(long frameId) => _coreRuntimeOperations.ReturnRuntimeProgramMovement(frameId);
    void ICardsMovedWindowCardUseContinuations.ContinueGuanduCardEffect(long id) => _coreGuanduCardsOperations.ContinueGuanduCardEffect(id);
    void ICardsMovedWindowCardUseContinuations.ContinueWumiaoFlood(long id) => _characterGuanYuWumiaoOperations.ContinueWumiaoFlood(id);
    void ICardsMovedWindowCardUseContinuations.ContinueDragonBoatCardEffect(long id) => _coreDragonBoatCardsOperations.ContinueDragonBoatCardEffect(id);
    void ICardsMovedWindowCardUseContinuations.ContinueWenheCardEffect(long id) => _coreWenheCardsOperations.ContinueWenheCardEffect(id);
    void ICardsMovedWindowCardUseContinuations.ContinueSoleIncomingRangeDraw(long id) => _coreSoleIncomingRangeDrawOperations.ContinueSoleIncomingRangeDraw(id);
    void ICardsMovedWindowCardUseContinuations.ContinueNativeDrawTwo(long id) => _coreNativeCardEffectDrawsOperations.ContinueNativeDrawTwo(id);
    void ICardsMovedWindowCardUseContinuations.ContinueFriendDuelResponse(long id) => _coreFriendDuelResponsesOperations.ContinueFriendDuelResponse(id);
    void ICardsMovedWindowCardUseContinuations.ContinueRecoveryPaidCardUse(long useId) => _coreRecoveryReplacementOperations.ContinueRecoveryPaidCardUse(useId);
    void ICardsMovedWindowCardUseContinuations.ContinueDenglouNativePayment(long id) => _characterWangCanOlOperations.ContinueDenglouNativePayment(id);
    void ICardsMovedWindowCardUseContinuations.ContinueGuoSiNativePayment(long id) => _characterGuoSiOlOperations.ContinueGuoSiNativePayment(id);
    void ICardsMovedWindowCardUseContinuations.AssertGuoSiNativeUse(CardUseFrame use) => _characterGuoSiOlOperations.AssertGuoSiNativeUse(use);
    void ICardsMovedWindowCardUseContinuations.ContinueHistoricalEndingPayment(long id) => _coreEndingHistoricalUsesOperations.ContinueHistoricalEndingPayment(id);
    void ICardsMovedWindowCardUseContinuations.ContinueRoundPileAlcoholUse(long id) => _coreRoundPricedPileAlcoholOperations.ContinueRoundPileAlcoholUse(id);
    void ICardsMovedWindowCardUseContinuations.ContinuePolicyCounterspellPayment(long windowId) => _coreUnrespondableCounterspellsOperations.ContinuePolicyCounterspellPayment(windowId);
    void ICardsMovedWindowCardUseContinuations.ContinueMouZhitianFirePayment(long id) => _characterZhugeLiangMouOperations.ContinueMouZhitianFirePayment(id);
    void ICardsMovedWindowCardUseContinuations.ContinueColorFireAttackPayment(long useId) => _coreColorFireAttacksOperations.ContinueColorFireAttackPayment(useId);
    int ICardsMovedWindowSelections.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    IEnumerable<ProgramTriggerCandidate> ICardsMovedWindowSelections.CollectSourceOwnerGainCandidates(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardMovementSourceCount count) => _coreSourceOwnerGainsOperations.Value.CollectSourceOwnerGainCandidates(batch, candidate, trigger, count);
    int ICardsMovedWindowSelections.FriendManjuanMovedCount(CardMovementBatchContext batch, int seat, SkillProgramTrigger trigger, int fallback) => _characterPangTongFriendOperations.FriendManjuanMovedCount(batch, seat, trigger, fallback);
    int[] ICardsMovedWindowSelections.MatchingNativeDrawOrdinalIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _coreNativeDrawOrdinalsOperations.Value.MatchingNativeDrawOrdinalIndexes(batch, candidate);
    int[] ICardsMovedWindowSelections.MatchingNativeDrawThresholdIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _coreNativeDrawThresholdOperations.Value.MatchingNativeDrawThresholdIndexes(batch, candidate);
    NativeDrawThresholdProgress? ICardsMovedWindowSelections.MatchingNativeDrawThresholdProgress(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _coreNativeDrawThresholdOperations.Value.MatchingNativeDrawThresholdProgress(batch, candidate);
    int[] ICardsMovedWindowSelections.MatchingLiJueUseDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _characterLiJueOlOperations.MatchingLiJueUseDiscardIndexes(batch, candidate);
    int[] ICardsMovedWindowSelections.MatchingMouXiahouDunDiscardIndexes(CardMovementBatchContext batch) => _characterXiahouDunMouMobileOperations.Value.MatchingMouXiahouDunDiscardIndexes(batch);
    ProgramDiscardedEntityOriginEvent? ICardsMovedWindowSelections.DiscardedProvenanceOrigin(CardMovementRecord movement) => _coreDiscardedProvenanceOperations.Value.DiscardedProvenanceOrigin(movement);
    CardLocation? ICardsMovedWindowSelections.FaluDiscardOwnedSource(CardMovementRecord movement) => _characterZhangQiYingOlOperations.FaluDiscardOwnedSource(movement);
    void ICardsMovedWindowOperations.MoveCard(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _corePhysicalMovementFacadesOperations.MoveCard(card, from, to, reason);
    void ICardsMovedWindowOperations.AddLog(string type, string message, int actor, int target) => _coreMatchLogOperations.AddLog(type, message, actor, target);
    void ICardsMovedWindowOperations.BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId) => _coreDrawFundedDistinctBasicPaymentOperations.BeginDrawFundedDistinctBasicChild(ownerFrameId, childFrameId);
    void ICardsMovedWindowOperations.ContinueManualDiscard(long frameId) => _coreDiscardOperations.ContinueManualDiscard(frameId);
    void ICardsMovedWindowOperations.ContinuePaidFactionRequestCost(long producerId) => _coreFactionRequestCostRecoveriesOperations.ContinuePaidFactionRequestCost(producerId);
    bool ICardsMovedWindowOperations.TryBeginHpChangedProgramWindow(long? resumeFrameId, PostEventContinuation continuation) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(resumeFrameId, continuation);
    bool ICardsMovedWindowPipeline.ExcludeOrdinaryMovementCandidate(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _programPipelineHooksOperations._programPipelineHooks.ExcludeOrdinaryMovementCandidate(candidate, trigger);
    void ICardsMovedWindowPipeline.CollectMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates) => _programPipelineHooksOperations._programPipelineHooks.CollectMovementCandidates(batch, candidates);
    ProgramSkillWindowContext? ICardsMovedWindowPipeline.CreateMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _programPipelineHooksOperations._programPipelineHooks.CreateMovementContext(frame, candidate);
    bool ICardsMovedWindowPipeline.IsProgramMovementSettlement(ProgramSkillFrame frame, CardMovementBatchContext batch) => _programPipelineHooksOperations._programPipelineHooks.IsProgramMovementSettlement(frame, batch);
    int[]? ICardsMovedWindowPipeline.SelectLegacyMovementIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location) => _programPipelineHooksOperations._programPipelineHooks.SelectLegacyMovementIndexes(batch, candidate, trigger, location);
    int[]? ICardsMovedWindowPipeline.SelectDiscardBatchIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features) => _programPipelineHooksOperations._programPipelineHooks.SelectDiscardBatchIndexes(batch, candidate, features);
    int[]? ICardsMovedWindowPipeline.SelectLegacyDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _programPipelineHooksOperations._programPipelineHooks.SelectLegacyDiscardIndexes(batch, candidate, trigger);
    void ICardsMovedWindowPipeline.PrepareLingkongGainBatch(CardMovementBatchContext batch) => _characterZhouFeiLeOperations.PrepareLingkongGainBatch(batch);
    IEnumerable<ProgramTriggerCandidate> ICardsMovedWindowPipeline.CollectFirstDomainCandidates(CardMovementBatchContext batch) => _coreGameDomainsOperations.CollectFirstDomainCandidates(batch);
    bool ICardsMovedWindowPipeline.HasWenhePoisonDepartures(CardMovementBatchContext batch) => _coreWenhePoisonOperations.HasWenhePoisonDepartures(batch);
}
