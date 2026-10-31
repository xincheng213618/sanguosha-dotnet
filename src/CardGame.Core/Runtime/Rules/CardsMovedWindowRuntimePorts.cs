namespace CardGame.Core;

internal interface ICardsMovedWindowFacts : IProgramTriggerQueries
{
    Winner Winner { get; }
    EngineStatus Status { get; }
    TurnPhase Phase { get; }
    int CurrentSeat { get; }
    int TurnNumber { get; }
    int PlayerCount { get; }
    bool IsValidPlayerSeat(int seat);
    bool IsAlive(int seat);
    bool HasActiveJudgment { get; }
    SkillProgramTrigger GetProgramTrigger(ProgramTriggerCandidate candidate);
    IEnumerable<(CardMovementSourceCount Count, bool DiscardOriginOnly)> ProgramMovementSourceCounts(CardMovementBatchContext batch, SkillProgramTriggerWindow window);
    bool ContainsMovement(CardMovementRecord movement);
    CardLocation? GetProgramDiscardSource(CardMovementRecord movement);
    PublicPersistentPileSource? RicePile(ProgramTriggerCandidate candidate);
    bool RiceProducer(PublicPersistentPileSource pile);
    CardKind GetAttackCardKind(int cardId);
    string PlayerName(int seat);
    string SkillName(string skillId);
}

internal interface ICardsMovedWindowNativeProofs : IProgramSelectionQuery
{
    bool IsShanheNullifiedClaimAwaitingMovement(NullificationWindowFrame window);
    bool IsShanheNullifiedClaimMovement(NullificationWindowFrame window, CardMovementBatchContext batch);
    bool IsKemingEchoMovement(KemingSkillEchoFrame f, CardMovementBatchContext b);
    bool IsSkillInvocationMovementBatch(SkillInvocationFrame f, CardMovementBatchContext batch);
    bool IsRoundEndingMovement(RoundEndingBoundaryFrame parent, CardMovementBatchContext batch);
    bool IsNianRevivalMovementBatch(ProgramSkillFrame f, CardMovementBatchContext batch);
    bool IsPendingWenhePoisonWindow(CardsMovedTriggerWindowFrame frame);
}

internal interface ICardsMovedWindowCardUseProofs : IProgramSelectionQuery
{
    bool HasPaidCardUseMovementTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation);
    bool IsCardUsePaymentBatch(CardUseFrame use, CardMovementBatchContext batch);
    bool IsSoleIncomingRangeDrawMovement(CardUseFrame use, CardMovementBatchContext batch);
    bool IsWumiaoFloodMovement(CardUseFrame use, CardMovementBatchContext batch);
    bool IsGuanduCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch);
    bool IsDragonBoatCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch);
    bool IsWenheCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch);
    bool IsNativeCardEffectDrawMovement(CardUseFrame use, CardMovementBatchContext batch);
    bool IsGuanduCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame);
    bool IsWumiaoFloodMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame);
    bool IsDragonBoatCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame);
    bool IsWenheCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement);
    bool IsSoleIncomingRangeDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame window);
    bool IsNativeCardEffectDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame);
    bool IsFriendDuelResponseMovementReturn(FriendDuelResponseFrame parent, CardsMovedTriggerWindowFrame movement);
    bool IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement);
}

internal interface ICardsMovedWindowNativeReturns : IProgramRuntimeOperations
{
    void ReturnShanheNullifiedClaimMovement(CardsMovedTriggerWindowFrame child);
    void ReturnHuxinMirrorPlacementMovement(CardsMovedTriggerWindowFrame window);
    void ReturnKemingEchoMovement(CardsMovedTriggerWindowFrame window);
    bool ReturnWenhePoisonFireAttackMovement(CardsMovedTriggerWindowFrame child);
    bool ReturnQiaoZhouPreparationMovement(CardsMovedTriggerWindowFrame child);
    bool ReturnCardResponseCompletionMovement(CardsMovedTriggerWindowFrame child);
    bool ReturnCardSupplyCompletionMovement(CardsMovedTriggerWindowFrame child);
    void ReturnDeferredTurnEndPrelude(CardsMovedTriggerWindowFrame child);
    void ReturnSoleTargetQuarantineMovement(CardsMovedTriggerWindowFrame child);
    void ResumeDrawFundedDistinctBasicMovement(CardsMovedTriggerWindowFrame completed);
    void ReturnRoundEndingMovement(CardsMovedTriggerWindowFrame child);
    void ReturnRuntimeProgramMovement(long frameId);
}

internal interface ICardsMovedWindowCardUseContinuations : IProgramRuntimeOperations
{
    void ContinueGuanduCardEffect(long id);
    void ContinueWumiaoFlood(long id);
    void ContinueDragonBoatCardEffect(long id);
    void ContinueWenheCardEffect(long id);
    void ContinueSoleIncomingRangeDraw(long id);
    void ContinueNativeDrawTwo(long id);
    void ContinueFriendDuelResponse(long id);
    void ContinueRecoveryPaidCardUse(long useId);
    void ContinueDenglouNativePayment(long id);
    void ContinueGuoSiNativePayment(long id);
    void AssertGuoSiNativeUse(CardUseFrame use);
    void ContinueHistoricalEndingPayment(long id);
    void ContinueRoundPileAlcoholUse(long id);
    void ContinuePolicyCounterspellPayment(long windowId);
    void ContinueMouZhitianFirePayment(long id);
    void ContinueColorFireAttackPayment(long useId);
}

internal interface ICardsMovedWindowSelections : IProgramSelectionQuery
{
    int RelativeTableOrder(int from, int target);
    IEnumerable<ProgramTriggerCandidate> CollectSourceOwnerGainCandidates(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardMovementSourceCount count);
    int FriendManjuanMovedCount(CardMovementBatchContext batch, int seat, SkillProgramTrigger trigger, int fallback);
    int[] MatchingNativeDrawOrdinalIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate);
    int[] MatchingNativeDrawThresholdIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate);
    NativeDrawThresholdProgress? MatchingNativeDrawThresholdProgress(CardMovementBatchContext batch, ProgramTriggerCandidate candidate);
    int[] MatchingLiJueUseDiscardIndexes(CardMovementBatchContext batch,ProgramTriggerCandidate candidate);
    int[] MatchingMouXiahouDunDiscardIndexes(CardMovementBatchContext batch);
    ProgramDiscardedEntityOriginEvent? DiscardedProvenanceOrigin(CardMovementRecord movement);
    CardLocation? FaluDiscardOwnedSource(CardMovementRecord movement);
}

internal interface ICardsMovedWindowOperations : IProgramRuntimeOperations
{
    void MoveCard(Card card, CardLocation from, CardLocation to, CardMoveReason reason);
    void AddLog(string type, string message, int actor, int target);
    void BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId);
    void ContinueManualDiscard(long frameId);
    void ContinuePaidFactionRequestCost(long producerId);
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId, PostEventContinuation continuation);
}

internal interface ICardsMovedWindowPipeline : IProgramRuntimeOperations
{
    bool ExcludeOrdinaryMovementCandidate(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger);
    void CollectMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates);
    ProgramSkillWindowContext? CreateMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate);
    bool IsProgramMovementSettlement(ProgramSkillFrame frame, CardMovementBatchContext batch);
    int[]? SelectLegacyMovementIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location);
    int[]? SelectDiscardBatchIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features);
    int[]? SelectLegacyDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger);
    void PrepareLingkongGainBatch(CardMovementBatchContext batch);
    IEnumerable<ProgramTriggerCandidate> CollectFirstDomainCandidates(CardMovementBatchContext batch);
    bool HasWenhePoisonDepartures(CardMovementBatchContext batch);
}

public sealed partial class GameEngine
{
}
