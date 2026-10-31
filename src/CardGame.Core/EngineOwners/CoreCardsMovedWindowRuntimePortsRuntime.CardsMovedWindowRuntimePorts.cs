#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreCardsMovedWindowRuntimePortsRuntime
{
    Winner global::CardGame.Core.ICardsMovedWindowFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus global::CardGame.Core.ICardsMovedWindowFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    TurnPhase global::CardGame.Core.ICardsMovedWindowFacts.Phase => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase;

    int global::CardGame.Core.ICardsMovedWindowFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;

    int global::CardGame.Core.ICardsMovedWindowFacts.TurnNumber => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    int global::CardGame.Core.ICardsMovedWindowFacts.PlayerCount => _coreMatchParticipantsPlayersAccess._players.Count;

    bool global::CardGame.Core.ICardsMovedWindowFacts.IsValidPlayerSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool global::CardGame.Core.ICardsMovedWindowFacts.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.ICardsMovedWindowFacts.HasActiveJudgment => _coreJudgmentCursorRuntimeOperations.ActiveJudgment is not null;

    SkillProgramTrigger global::CardGame.Core.ICardsMovedWindowFacts.GetProgramTrigger(ProgramTriggerCandidate candidate) => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
    IEnumerable<(CardMovementSourceCount Count, bool DiscardOriginOnly)> global::CardGame.Core.ICardsMovedWindowFacts.ProgramMovementSourceCounts(CardMovementBatchContext batch, SkillProgramTriggerWindow window) => _coreProgramMovementSourceCountsOperations.ProgramMovementSourceCounts(batch, window);
    bool global::CardGame.Core.ICardsMovedWindowFacts.ContainsMovement(CardMovementRecord movement) => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(movement);
    CardLocation? global::CardGame.Core.ICardsMovedWindowFacts.GetProgramDiscardSource(CardMovementRecord movement) => _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement);
    PublicPersistentPileSource? global::CardGame.Core.ICardsMovedWindowFacts.RicePile(ProgramTriggerCandidate candidate) => _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId));
    bool global::CardGame.Core.ICardsMovedWindowFacts.RiceProducer(PublicPersistentPileSource pile) => _coreRicePileOperations.RiceProducer(pile);
    CardKind global::CardGame.Core.ICardsMovedWindowFacts.GetAttackCardKind(int cardId) => _coreCardAttackStateOperations.GetAttackCard(cardId).Kind;
    string global::CardGame.Core.ICardsMovedWindowFacts.PlayerName(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    string global::CardGame.Core.ICardsMovedWindowFacts.SkillName(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId).Name;
    SkillProgramTriggerFacts global::CardGame.Core.IProgramTriggerQueries.CaptureProgramTriggerFacts(int ownerSeat) => _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[ownerSeat]);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IProgramTriggerQueries.CollectProgramTriggerCandidates(int ownerSeat, SkillProgramTriggerWindow window) => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[ownerSeat], window);
    bool global::CardGame.Core.ICardsMovedWindowNativeProofs.IsShanheNullifiedClaimAwaitingMovement(NullificationWindowFrame window) => _coreShanheNullifiedTrickClaimsOperations.IsShanheNullifiedClaimAwaitingMovement(window);
    bool global::CardGame.Core.ICardsMovedWindowNativeProofs.IsShanheNullifiedClaimMovement(NullificationWindowFrame window, CardMovementBatchContext batch) => _coreShanheNullifiedTrickClaimsOperations.IsShanheNullifiedClaimMovement(window, batch);
    bool global::CardGame.Core.ICardsMovedWindowNativeProofs.IsKemingEchoMovement(KemingSkillEchoFrame f, CardMovementBatchContext b) => _characterCaoPiAnniversaryGodOperations.IsKemingEchoMovement(f, b);
    bool global::CardGame.Core.ICardsMovedWindowNativeProofs.IsSkillInvocationMovementBatch(SkillInvocationFrame f, CardMovementBatchContext batch) => _characterHuaTuoAnniversaryGodOperations.IsSkillInvocationMovementBatch(f, batch);
    bool global::CardGame.Core.ICardsMovedWindowNativeProofs.IsRoundEndingMovement(RoundEndingBoundaryFrame parent, CardMovementBatchContext batch) => _coreRoundEndingOperations.IsRoundEndingMovement(parent, batch);
    bool global::CardGame.Core.ICardsMovedWindowNativeProofs.IsNianRevivalMovementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => _coreNianRevivalOperations.IsNianRevivalMovementBatch(f, batch);
    bool global::CardGame.Core.ICardsMovedWindowNativeProofs.IsPendingWenhePoisonWindow(CardsMovedTriggerWindowFrame frame) => _coreWenhePoisonOperations.IsPendingWenhePoisonWindow(frame);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.HasPaidCardUseMovementTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => _coreProgramCardUsePaymentsOperations.HasPaidCardUseMovementTail(use, continuation);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsCardUsePaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => _coreCardUsePaymentsOperations.IsCardUsePaymentBatch(use, batch);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsSoleIncomingRangeDrawMovement(CardUseFrame use, CardMovementBatchContext batch) => _coreSoleIncomingRangeDrawOperations.IsSoleIncomingRangeDrawMovement(use, batch);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsWumiaoFloodMovement(CardUseFrame use, CardMovementBatchContext batch) => _characterGuanYuWumiaoOperations.IsWumiaoFloodMovement(use, batch);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsGuanduCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => _coreGuanduCardsOperations.IsGuanduCardEffectMovement(use, batch);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsDragonBoatCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => _coreDragonBoatCardsOperations.IsDragonBoatCardEffectMovement(use, batch);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsWenheCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => _coreWenheCardsOperations.IsWenheCardEffectMovement(use, batch);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsNativeCardEffectDrawMovement(CardUseFrame use, CardMovementBatchContext batch) => _coreNativeCardEffectDrawsOperations.IsNativeCardEffectDrawMovement(use, batch);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsGuanduCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => _coreGuanduCardsOperations.IsGuanduCardEffectMovementReturn(use, frame);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsWumiaoFloodMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => _characterGuanYuWumiaoOperations.IsWumiaoFloodMovementReturn(use, frame);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsDragonBoatCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => _coreDragonBoatCardsOperations.IsDragonBoatCardEffectMovementReturn(use, frame);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsWenheCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => _coreWenheCardsOperations.IsWenheCardEffectMovementReturn(use, movement);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsSoleIncomingRangeDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame window) => _coreSoleIncomingRangeDrawOperations.IsSoleIncomingRangeDrawMovementReturn(use, window);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsNativeCardEffectDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => _coreNativeCardEffectDrawsOperations.IsNativeCardEffectDrawMovementReturn(use, frame);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsFriendDuelResponseMovementReturn(FriendDuelResponseFrame parent, CardsMovedTriggerWindowFrame movement) => _coreFriendDuelResponsesOperations.IsFriendDuelResponseMovementReturn(parent, movement);
    bool global::CardGame.Core.ICardsMovedWindowCardUseProofs.IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => _coreCardUsePaymentsOperations.IsPaidCardUseMovementReturn(use, movement);
    void global::CardGame.Core.ICardsMovedWindowNativeReturns.ReturnShanheNullifiedClaimMovement(CardsMovedTriggerWindowFrame child) => _coreShanheNullifiedTrickClaimsOperations.ReturnShanheNullifiedClaimMovement(child);
    void global::CardGame.Core.ICardsMovedWindowNativeReturns.ReturnHuxinMirrorPlacementMovement(CardsMovedTriggerWindowFrame window) => _coreHuxinMirrorOperations.ReturnHuxinMirrorPlacementMovement(window);
    void global::CardGame.Core.ICardsMovedWindowNativeReturns.ReturnKemingEchoMovement(CardsMovedTriggerWindowFrame window) => _characterCaoPiAnniversaryGodOperations.ReturnKemingEchoMovement(window);
    bool global::CardGame.Core.ICardsMovedWindowNativeReturns.ReturnWenhePoisonFireAttackMovement(CardsMovedTriggerWindowFrame child) => _coreWenhePoisonOperations.ReturnWenhePoisonFireAttackMovement(child);
    bool global::CardGame.Core.ICardsMovedWindowNativeReturns.ReturnQiaoZhouPreparationMovement(CardsMovedTriggerWindowFrame child) => _characterQiaoZhouOlOperations.ReturnQiaoZhouPreparationMovement(child);
    bool global::CardGame.Core.ICardsMovedWindowNativeReturns.ReturnCardResponseCompletionMovement(CardsMovedTriggerWindowFrame child) => _coreResponseCompletionOperations.ReturnCardResponseCompletionMovement(child);
    bool global::CardGame.Core.ICardsMovedWindowNativeReturns.ReturnCardSupplyCompletionMovement(CardsMovedTriggerWindowFrame child) => _coreCardSupplyCompletionOperations.ReturnCardSupplyCompletionMovement(child);
    void global::CardGame.Core.ICardsMovedWindowNativeReturns.ReturnDeferredTurnEndPrelude(CardsMovedTriggerWindowFrame child) => _coreDeferredTurnEndPreludesOperations.ReturnDeferredTurnEndPrelude(child);
    void global::CardGame.Core.ICardsMovedWindowNativeReturns.ReturnSoleTargetQuarantineMovement(CardsMovedTriggerWindowFrame child) => _coreSoleTargetQuarantineOperations.ReturnSoleTargetQuarantineMovement(child);
    void global::CardGame.Core.ICardsMovedWindowNativeReturns.ResumeDrawFundedDistinctBasicMovement(CardsMovedTriggerWindowFrame completed) => _coreDrawFundedDistinctBasicPaymentOperations.ResumeDrawFundedDistinctBasicMovement(completed);
    void global::CardGame.Core.ICardsMovedWindowNativeReturns.ReturnRoundEndingMovement(CardsMovedTriggerWindowFrame child) => _coreRoundEndingOperations.ReturnRoundEndingMovement(child);
    void global::CardGame.Core.ICardsMovedWindowNativeReturns.ReturnRuntimeProgramMovement(long frameId) => _coreRuntimeOperations.ReturnRuntimeProgramMovement(frameId);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueGuanduCardEffect(long id) => _coreGuanduCardsOperations.ContinueGuanduCardEffect(id);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueWumiaoFlood(long id) => _characterGuanYuWumiaoOperations.ContinueWumiaoFlood(id);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueDragonBoatCardEffect(long id) => _coreDragonBoatCardsOperations.ContinueDragonBoatCardEffect(id);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueWenheCardEffect(long id) => _coreWenheCardsOperations.ContinueWenheCardEffect(id);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueSoleIncomingRangeDraw(long id) => _coreSoleIncomingRangeDrawOperations.ContinueSoleIncomingRangeDraw(id);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueNativeDrawTwo(long id) => _coreNativeCardEffectDrawsOperations.ContinueNativeDrawTwo(id);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueFriendDuelResponse(long id) => _coreFriendDuelResponsesOperations.ContinueFriendDuelResponse(id);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueRecoveryPaidCardUse(long useId) => _coreRecoveryReplacementOperations.ContinueRecoveryPaidCardUse(useId);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueDenglouNativePayment(long id) => _characterWangCanOlOperations.ContinueDenglouNativePayment(id);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueGuoSiNativePayment(long id) => _characterGuoSiOlOperations.ContinueGuoSiNativePayment(id);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.AssertGuoSiNativeUse(CardUseFrame use) => _characterGuoSiOlOperations.AssertGuoSiNativeUse(use);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueHistoricalEndingPayment(long id) => _coreEndingHistoricalUsesOperations.ContinueHistoricalEndingPayment(id);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueRoundPileAlcoholUse(long id) => _coreRoundPricedPileAlcoholOperations.ContinueRoundPileAlcoholUse(id);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinuePolicyCounterspellPayment(long windowId) => _coreUnrespondableCounterspellsOperations.ContinuePolicyCounterspellPayment(windowId);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueMouZhitianFirePayment(long id) => _characterZhugeLiangMouOperations.ContinueMouZhitianFirePayment(id);
    void global::CardGame.Core.ICardsMovedWindowCardUseContinuations.ContinueColorFireAttackPayment(long useId) => _coreColorFireAttacksOperations.ContinueColorFireAttackPayment(useId);
    int global::CardGame.Core.ICardsMovedWindowSelections.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    IEnumerable<ProgramTriggerCandidate> global::CardGame.Core.ICardsMovedWindowSelections.CollectSourceOwnerGainCandidates(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardMovementSourceCount count) => _coreSourceOwnerGainsOperations.CollectSourceOwnerGainCandidates(batch, candidate, trigger, count);
    int global::CardGame.Core.ICardsMovedWindowSelections.FriendManjuanMovedCount(CardMovementBatchContext batch, int seat, SkillProgramTrigger trigger, int fallback) => _characterPangTongFriendOperations.FriendManjuanMovedCount(batch, seat, trigger, fallback);
    int[] global::CardGame.Core.ICardsMovedWindowSelections.MatchingNativeDrawOrdinalIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _coreNativeDrawOrdinalsOperations.MatchingNativeDrawOrdinalIndexes(batch, candidate);
    int[] global::CardGame.Core.ICardsMovedWindowSelections.MatchingNativeDrawThresholdIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _coreNativeDrawThresholdOperations.MatchingNativeDrawThresholdIndexes(batch, candidate);
    NativeDrawThresholdProgress? global::CardGame.Core.ICardsMovedWindowSelections.MatchingNativeDrawThresholdProgress(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _coreNativeDrawThresholdOperations.MatchingNativeDrawThresholdProgress(batch, candidate);
    int[] global::CardGame.Core.ICardsMovedWindowSelections.MatchingLiJueUseDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _characterLiJueOlOperations.MatchingLiJueUseDiscardIndexes(batch, candidate);
    int[] global::CardGame.Core.ICardsMovedWindowSelections.MatchingMouXiahouDunDiscardIndexes(CardMovementBatchContext batch) => _characterXiahouDunMouMobileOperations.MatchingMouXiahouDunDiscardIndexes(batch);
    ProgramDiscardedEntityOriginEvent? global::CardGame.Core.ICardsMovedWindowSelections.DiscardedProvenanceOrigin(CardMovementRecord movement) => _coreDiscardedProvenanceOperations.DiscardedProvenanceOrigin(movement);
    CardLocation? global::CardGame.Core.ICardsMovedWindowSelections.FaluDiscardOwnedSource(CardMovementRecord movement) => _characterZhangQiYingOlOperations.FaluDiscardOwnedSource(movement);
    void global::CardGame.Core.ICardsMovedWindowOperations.MoveCard(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _corePhysicalMovementFacadesOperations.MoveCard(card, from, to, reason);
    void global::CardGame.Core.ICardsMovedWindowOperations.AddLog(string type, string message, int actor, int target) => _coreMatchLogOperations.AddLog(type, message, actor, target);
    void global::CardGame.Core.ICardsMovedWindowOperations.BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId) => _coreDrawFundedDistinctBasicPaymentOperations.BeginDrawFundedDistinctBasicChild(ownerFrameId, childFrameId);
    void global::CardGame.Core.ICardsMovedWindowOperations.ContinueManualDiscard(long frameId) => _coreDiscardOperations.ContinueManualDiscard(frameId);
    void global::CardGame.Core.ICardsMovedWindowOperations.ContinuePaidFactionRequestCost(long producerId) => _coreFactionRequestCostRecoveriesOperations.ContinuePaidFactionRequestCost(producerId);
    bool global::CardGame.Core.ICardsMovedWindowOperations.TryBeginHpChangedProgramWindow(long? resumeFrameId, PostEventContinuation continuation) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(resumeFrameId, continuation);
    bool global::CardGame.Core.ICardsMovedWindowPipeline.ExcludeOrdinaryMovementCandidate(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _programPipelineHooksOperations._programPipelineHooks.ExcludeOrdinaryMovementCandidate(candidate, trigger);
    void global::CardGame.Core.ICardsMovedWindowPipeline.CollectMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates) => _programPipelineHooksOperations._programPipelineHooks.CollectMovementCandidates(batch, candidates);
    ProgramSkillWindowContext? global::CardGame.Core.ICardsMovedWindowPipeline.CreateMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _programPipelineHooksOperations._programPipelineHooks.CreateMovementContext(frame, candidate);
    bool global::CardGame.Core.ICardsMovedWindowPipeline.IsProgramMovementSettlement(ProgramSkillFrame frame, CardMovementBatchContext batch) => _programPipelineHooksOperations._programPipelineHooks.IsProgramMovementSettlement(frame, batch);
    int[]? global::CardGame.Core.ICardsMovedWindowPipeline.SelectLegacyMovementIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location) => _programPipelineHooksOperations._programPipelineHooks.SelectLegacyMovementIndexes(batch, candidate, trigger, location);
    int[]? global::CardGame.Core.ICardsMovedWindowPipeline.SelectDiscardBatchIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features) => _programPipelineHooksOperations._programPipelineHooks.SelectDiscardBatchIndexes(batch, candidate, features);
    int[]? global::CardGame.Core.ICardsMovedWindowPipeline.SelectLegacyDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _programPipelineHooksOperations._programPipelineHooks.SelectLegacyDiscardIndexes(batch, candidate, trigger);
    void global::CardGame.Core.ICardsMovedWindowPipeline.PrepareLingkongGainBatch(CardMovementBatchContext batch) => _characterZhouFeiLeOperations.PrepareLingkongGainBatch(batch);
    IEnumerable<ProgramTriggerCandidate> global::CardGame.Core.ICardsMovedWindowPipeline.CollectFirstDomainCandidates(CardMovementBatchContext batch) => _coreGameDomainsOperations.CollectFirstDomainCandidates(batch);
    bool global::CardGame.Core.ICardsMovedWindowPipeline.HasWenhePoisonDepartures(CardMovementBatchContext batch) => _coreWenhePoisonOperations.HasWenhePoisonDepartures(batch);
}
