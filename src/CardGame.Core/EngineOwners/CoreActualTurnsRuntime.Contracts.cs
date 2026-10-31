#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreActualTurnsCoreIdentityConcealmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginIdentityAppearanceForTurn(global::CardGame.Core.CharacterState player);
}

internal interface ICoreActualTurnsCorePrepDiscardEndingPromisesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpirePrepDiscardEndingPromises();
}

internal interface ICoreActualTurnsCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearActualDiscardRecoveryPhase();
    void StartActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int actor);
}

internal interface ICoreActualTurnsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreActualTurnsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResetProgramContributionUsesForPlayPhase(int providerSeat);
}

internal interface ICoreActualTurnsCharacterRuanJiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool KeepsAnniversaryRuanJiAlcohol(int seat);
}

internal interface ICoreActualTurnsCharacterHuaXiongMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecoverMouYangweiAtEnding(int owner);
}

internal interface ICoreActualTurnsCoreActualDiscardPhaseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDiscardPhase(bool beforeEntryPolicyCompleted = false);
}

internal interface ICoreActualTurnsCoreEndRefillNextTurnOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeEndRefillNextTurn(global::CardGame.Core.CharacterState current);
}

internal interface ICoreActualTurnsCoreShanheS5ShensuOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordShanheS5ActualPhaseSkip(int owner, global::CardGame.Core.SkillProgramTurnPhase phase, long extraDrawFrameId = 0, long? costProgramFrameId = default);
}

internal interface ICoreActualTurnsCharacterCaoXiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int FreezeAndCaptureCaoXiNativeDrawBase(global::CardGame.Core.CharacterState player, bool replaced = false);
}

internal interface ICoreActualTurnsCoreDrawPhaseObligationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginActualDrawCompletion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects);
}

internal interface ICoreActualTurnsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreActualTurnsCoreCharacterTurnedOverProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown);
}

internal interface ICoreActualTurnsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreActualTurnsCoreProgramHoldCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnProgramPojunHoldsAtTurnEnd();
}

internal interface ICoreActualTurnsCoreDeferredCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeSkippedNextTurnDrawBenefits(global::CardGame.Core.CharacterState current);
    void CleanupLostDeferredPileSources();
}

internal interface ICoreActualTurnsCharacterBeiMiHuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginBeiMiHuActualTurnPrelude(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreActualTurnsCharacterGuoHuaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMouGuoHuaiActualStart(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreActualTurnsCoreCardUseDebitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResetCardUseDebitPhase();
}

internal interface ICoreActualTurnsCharacterLvQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ObserveLvQianActualPlayEntry(int ownerSeat, int phaseInstance);
    bool TryBeginForcedWeiluPhaseEnd();
}

internal interface ICoreActualTurnsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginRoundStartingProgramWindow(global::CardGame.Core.CharacterState turnOwner);
    bool TryBeginTurnStartProgramWindow(global::CardGame.Core.CharacterState owner);
    void BeginNormalTurnStartAfterProgramBindings(global::CardGame.Core.CharacterState current);
    bool TryBeginDrawPhaseProgramWindow(global::CardGame.Core.CharacterState owner, bool skipPlayPhaseAfterDraw);
    bool TryBeginDrawPhaseEndedProgramWindow(global::CardGame.Core.CharacterState owner, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects);
    bool TryBeginAfterNormalDrawProgramWindow(global::CardGame.Core.CharacterState owner);
    bool ResumeScheduledProgramPhase();
    bool TryBeginPlayPhaseStartingBoundary(global::CardGame.Core.CharacterState owner);
    void CompletePlayPhaseAfterProgramWindow();
    void CancelScheduledProgramPhase();
    bool TryBeginTurnEndingBoundary(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreActualTurnsCoreDeferredHandAlignmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CleanupDeferredHandAlignments();
    bool TryBeginDeferredTurnEnd(global::CardGame.Core.CharacterState previous, bool skipped);
}

internal interface ICoreActualTurnsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreActualTurnsCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordCharacterStateChange(int targetSeat, global::CardGame.Core.SkillProgramTriggerWindow window, long? parentFrameId = default, global::CardGame.Core.CharacterTurnedOverState? turnedOver = default);
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
    void CompleteFaceUpSkippedTurn(global::CardGame.Core.CharacterState current);
    bool TryBeginJudgmentPhaseStartingPrograms(global::CardGame.Core.CharacterState turnOwner);
}

internal interface ICoreActualTurnsCoreWholeActualTurnUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksWholeActualTurnUseOrdinal { get; }
}

internal interface ICoreActualTurnsProgramPhaseScheduleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramPhaseSchedule? _programPhaseSchedule { get; }
}

internal interface ICoreActualTurnsCoreMouLuMengOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginRoundForTurn(global::CardGame.Core.CharacterState current);
}

internal interface ICoreActualTurnsCoreForeignTurnContestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginForeignActualTurnStart(global::CardGame.Core.CharacterState turnOwner);
}

internal interface ICoreActualTurnsCharacterZhuHuanBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordBoundaryZhuHuanSkippedPhase(int owner, global::CardGame.Core.SkillProgramTurnPhase phase, long extraDrawFrameId = 0);
}

internal interface ICoreActualTurnsCoreNianActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordNianActualPhaseSkips(int owner, bool draw, bool play);
}

internal interface ICoreActualTurnsCoreDelayedNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDelayedNullificationWindow(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
}

internal interface ICoreActualTurnsCorePrivateCardOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDeferredPrivateOffers(global::CardGame.Core.CharacterState target);
}

internal interface ICoreActualTurnsCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RuleQueryEvaluation EvaluateDrawCount(global::CardGame.Core.CharacterState player);
}

internal interface ICoreActualTurnsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreActualTurnsCoreDeferredBenefitsProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ApplyQueuedNextTurnRuleModifiers(int targetSeat);
}

internal interface ICoreActualTurnsCoreStrategicProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResetPlayPhaseProgramStates(int ownerSeat);
    void ExpireProgramSuppressions(int turnOwnerSeat);
}

internal interface ICoreActualTurnsCorePlayPhaseSkillGrantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPlayPhaseSkillGrantCapability { get; }
}

internal interface ICoreActualTurnsCharacterZhangLiaoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ApplyAnniversaryCuixiDrawSkip(int currentSeat);
}

internal interface ICoreActualTurnsCharacterLuBuOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMoLuBuOpening();
    bool TryStartMoLuBuOpeningPlay(global::CardGame.Core.CharacterState owner);
    bool TryEndMoLuBuOpeningEarly();
}

internal interface ICoreActualTurnsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireOwnerTurnStartDamageModifiers(int ownerSeat);
    void ExpireTurnCardUseEffects(int turnNumber, int turnSeat);
}

internal interface ICoreActualTurnsCoreRoundEndingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginRoundEndingBoundary(global::CardGame.Core.CharacterState previous, bool skipped);
    bool TracksRoundEnding { get; }
}

internal interface ICoreActualTurnsCharacterChengGongYingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeChengGongYingDraw(int actorSeat);
}

internal interface ICoreActualTurnsCoreMatchOutcomesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void EndAsDraw(string reason);
}

internal interface ICoreActualTurnsCoreActualTurnProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceActualTurnProgression(global::CardGame.Core.CharacterState previous);
}

internal interface ICoreActualTurnsCharacterSunQuanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHappyLearningActualStart(global::CardGame.Core.CharacterState current);
}

internal interface ICoreActualTurnsCharacterLuBuHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHappyLuBuActualStart(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreActualTurnsCoreNamedUseConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResetTurnProgramBooleanStates();
}

internal interface ICoreActualTurnsModeRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}

internal interface ICoreActualTurnsCoreActualPhaseEndsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginActualPhaseEnd(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, global::CardGame.Core.ActualPhaseEndContinuation continuation, int delayedEffects = 0, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false, long? parentFrameId = default);
    bool TracksActualPhaseEnds { get; }
}

internal interface ICoreActualTurnsCoreConvertingGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireGiftRetentionObligations();
}

internal interface ICoreActualTurnsCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveHengyeTurnStart(global::CardGame.Core.CharacterState player);
}

internal interface ICoreActualTurnsCharacterQiaoZhouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQiaoZhouPreparation(global::CardGame.Core.CharacterState current);
}

internal interface ICoreActualTurnsCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHeldOriginalUseLightning(global::CardGame.Core.CharacterState current, int? excludedCard);
    bool HasHeldOriginalUseLightning(global::CardGame.Core.CharacterState player);
}

internal interface ICoreActualTurnsCoreGrantedEntityPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void IssueGrantedEntityPhaseBoundary(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreActualTurnsCoreProgramRuntimeHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreActualTurnsCorePublicPersistentPilesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CleanupLostPublicPersistentPiles();
}

internal interface ICoreActualTurnsCharacterKeBiNengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireKeBiNengPingduanAlcohol(int actualTurn, int turnOwner);
}

internal interface ICoreActualTurnsCorePrivateTurnHoldsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnPrivateTurnHoldsAtTurnEnd();
}

internal interface ICoreActualTurnsCharacterDaQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZixiQinJudgmentCard(int id);
    global::CardGame.Core.CardKind? ZixiQinJudgmentSlot(int id);
}

internal interface ICoreActualTurnsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreActualTurnsCoreDrawPhaseAttemptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginNormalDrawAttemptPhase(global::CardGame.Core.CharacterState owner, global::CardGame.Core.GameEngine.DelayedTurnEffects delayed, bool afterStartingWindow = false, bool replaced = false, int adjustment = 0, int? frozenBase = default);
}

internal interface ICoreActualTurnsCharacterDuFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeDuFuRenPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int ownerSeat);
}

internal interface ICoreActualTurnsCharacterKanZeOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ConsumeKuanshiDrawSkip(int ownerSeat);
}

internal interface ICoreActualTurnsCharacterLiuQiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ObserveLiuQiPlayBoundary(int ownerSeat, int phaseInstance, bool skipped);
}

internal interface ICoreActualTurnsSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICoreActualTurnsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreActualTurnsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreActualTurnsCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryQueueHengyeRecovery(global::CardGame.Core.CharacterState player);
}

internal interface ICoreActualTurnsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int FindNextAliveSeat(int fromSeat);
}

internal interface ICoreActualTurnsCoreMatchPresentationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string GetPublicGeneralName(global::CardGame.Core.CharacterState player);
}

internal interface ICoreActualTurnsCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesTreatedWounded { get; }
}

internal interface ICoreActualTurnsCorePhaseHandSeizureOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginForcedPhaseHandDebtReturn();
}

internal interface ICoreActualTurnsCharacterQianZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireWeifuActualTurnGrants();
}

internal interface ICoreActualTurnsCharacterSunQuanAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAnniversaryCangmingActualTurnStart(global::CardGame.Core.CharacterState current);
}
