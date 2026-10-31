#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardsMovedWindowRuntimePortsCharacterQiaoZhouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ReturnQiaoZhouPreparationMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreShanheNullifiedTrickClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheNullifiedClaimAwaitingMovement(global::CardGame.Core.NullificationWindowFrame window);
    bool IsShanheNullifiedClaimMovement(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.CardMovementBatchContext batch);
    void ReturnShanheNullifiedClaimMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreWenheCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenheCardEffectMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch);
    bool IsWenheCardEffectMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement);
    void ContinueWenheCardEffect(long id);
}

internal interface ICoreCardsMovedWindowRuntimePortsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreCardsMovedWindowRuntimePortsCharacterGuoSiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueGuoSiNativePayment(long id);
    void AssertGuoSiNativeUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreCardsMovedWindowRuntimePortsCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWumiaoFloodMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch);
    bool IsWumiaoFloodMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame frame);
    void ContinueWumiaoFlood(long id);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreSoleTargetQuarantineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnSoleTargetQuarantineMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueManualDiscard(long frameId);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUsePaymentBatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch);
    bool IsPaidCardUseMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueRecoveryPaidCardUse(long useId);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreNativeCardEffectDrawsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNativeCardEffectDrawMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch);
    bool IsNativeCardEffectDrawMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame frame);
    void ContinueNativeDrawTwo(long id);
}

internal interface ICoreCardsMovedWindowRuntimePortsCharacterZhangQiYingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation? FaluDiscardOwnedSource(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreSourceOwnerGainsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectSourceOwnerGainCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardMovementSourceCount count);
}

internal interface ICoreCardsMovedWindowRuntimePortsCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSkillInvocationMovementBatch(global::CardGame.Core.SkillInvocationFrame f, global::CardGame.Core.CardMovementBatchContext batch);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreCardsMovedWindowRuntimePortsCharacterXiahouDunMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] MatchingMouXiahouDunDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreProgramDiscardOriginsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation? GetProgramDiscardSource(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreEndingHistoricalUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueHistoricalEndingPayment(long id);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreHuxinMirrorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnHuxinMirrorPlacementMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame window);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreCardSupplyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ReturnCardSupplyCompletionMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child);
}

internal interface ICoreCardsMovedWindowRuntimePortsCharacterWangCanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueDenglouNativePayment(long id);
}

internal interface ICoreCardsMovedWindowRuntimePortsCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueMouZhitianFirePayment(long id);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreFriendDuelResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFriendDuelResponseMovementReturn(global::CardGame.Core.FriendDuelResponseFrame parent, global::CardGame.Core.CardsMovedTriggerWindowFrame movement);
    void ContinueFriendDuelResponse(long id);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreSoleIncomingRangeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSoleIncomingRangeDrawMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch);
    bool IsSoleIncomingRangeDrawMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame window);
    void ContinueSoleIncomingRangeDraw(long id);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreCardsMovedWindowRuntimePortsCharacterPangTongFriendOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int FriendManjuanMovedCount(global::CardGame.Core.CardMovementBatchContext batch, int seat, global::CardGame.Core.SkillProgramTrigger trigger, int fallback);
}

internal interface ICoreCardsMovedWindowRuntimePortsCharacterCaoPiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKemingEchoMovement(global::CardGame.Core.KemingSkillEchoFrame f, global::CardGame.Core.CardMovementBatchContext b);
    void ReturnKemingEchoMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame window);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreFactionRequestCostRecoveriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinuePaidFactionRequestCost(long producerId);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreDrawFundedDistinctBasicPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResumeDrawFundedDistinctBasicMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame completed);
    void BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreDiscardedProvenanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDiscardedEntityOriginEvent? DiscardedProvenanceOrigin(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreCardsMovedWindowRuntimePortsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreRicePileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RiceProducer(global::CardGame.Core.PublicPersistentPileSource p);
}

internal interface ICoreCardsMovedWindowRuntimePortsCharacterLiJueOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] MatchingLiJueUseDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnRuntimeProgramMovement(long frameId);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreProgramMovementSourceCountsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardMovementSourceCount Count, bool DiscardOriginOnly)> ProgramMovementSourceCounts(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPendingWenhePoisonWindow(global::CardGame.Core.CardsMovedTriggerWindowFrame frame);
    bool ReturnWenhePoisonFireAttackMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child);
    bool HasWenhePoisonDepartures(global::CardGame.Core.CardMovementBatchContext batch);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreNativeDrawOrdinalsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] MatchingNativeDrawOrdinalIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreNativeDrawThresholdOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] MatchingNativeDrawThresholdIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate);
    global::CardGame.Core.NativeDrawThresholdProgress? MatchingNativeDrawThresholdProgress(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduCardEffectMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch);
    bool IsGuanduCardEffectMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame frame);
    void ContinueGuanduCardEffect(long id);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreColorFireAttacksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueColorFireAttackPayment(long useId);
}

internal interface ICoreCardsMovedWindowRuntimePortsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ReturnCardResponseCompletionMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreNianRevivalOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNianRevivalMovementBatch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementBatchContext batch);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreProgramCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPaidCardUseMovementTail(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreGameDomainsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectFirstDomainCandidates(global::CardGame.Core.CardMovementBatchContext batch);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreJudgmentCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame? ActiveJudgment { get; }
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreUnrespondableCounterspellsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinuePolicyCounterspellPayment(long windowId);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreDeferredTurnEndPreludesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnDeferredTurnEndPrelude(global::CardGame.Core.CardsMovedTriggerWindowFrame child);
}

internal interface ICoreCardsMovedWindowRuntimePortsCharacterZhouFeiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareLingkongGainBatch(global::CardGame.Core.CardMovementBatchContext batch);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreRoundEndingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRoundEndingMovement(global::CardGame.Core.RoundEndingBoundaryFrame parent, global::CardGame.Core.CardMovementBatchContext batch);
    void ReturnRoundEndingMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreRoundPricedPileAlcoholOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueRoundPileAlcoholUse(long id);
}

internal interface ICoreCardsMovedWindowRuntimePortsCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDragonBoatCardEffectMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch);
    bool IsDragonBoatCardEffectMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame frame);
    void ContinueDragonBoatCardEffect(long id);
}
