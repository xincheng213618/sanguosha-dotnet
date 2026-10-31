#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramLifecycleCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryChooseKenshangAi(global::CardGame.Core.PendingDecision p, out global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramLifecycleCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }
}

internal interface ICoreProgramLifecycleCoreLostHpOwnedGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveLostHpOwnedGift(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramLifecycleCoreDamageJudgmentSuitPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDamageJudgmentSuitPaymentChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramLifecycleCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode);
}

internal interface ICoreProgramLifecycleCoreProgramChoicesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsClaimableProgramOptionStillAvailable(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetLivingFactionCount();
}

internal interface ICoreProgramLifecycleProgramPhaseSchedulingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramPhaseSchedulingRuntime Get();
}

internal interface ICoreProgramLifecycleCharacterChengJiChengCuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryChooseTousuiAi(global::CardGame.Core.PendingDecision p, out global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramLifecycleCharacterLuBuOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryFinishMoLuBuOpeningPlay();
}

internal interface ICoreProgramLifecycleCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTurnCardTargetRestriction(int actorSeat, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int? targetSeat = default);
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreProgramLifecycleCoreTeamActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PublicTeamEnemy(int owner, global::CardGame.Core.CharacterState target);
}

internal interface ICoreProgramLifecycleCoreColdDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryChooseColdDamageAi(global::CardGame.Core.PendingDecision decision, out global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleResolutionSchedulerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ResolutionScheduler _resolutionScheduler { get; }
}

internal interface ICoreProgramLifecycleCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreProgramLifecycleCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreProgramLifecycleCoreSlashSuitDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramSlashSuitDiscardChoiceLegal(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramLifecycleProgramBindingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramBindingRuntime Get();
}

internal interface ICoreProgramLifecycleCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramLifecycleCoreAlternativePhaseCostProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAlternativePhaseCostChoiceLegal(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramLifecycleCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int DamageInstancesTakenThisTurn(int ownerSeat);
}

internal interface ICoreProgramLifecycleProgramTargetSelectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetSelectionRuntime Get();
}

internal interface ICoreProgramLifecycleCharacterQingheGongzhuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::CardGame.Core.ProgramAiEstimate Estimate, int? TargetSeat) EstimateActualDodgeZengouForAi(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramSkillWindowContext? context);
}

internal interface ICoreProgramLifecycleCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GetProgramBooleanState(int ownerSeat, string skillId, string skillInstanceId, string stateId);
}

internal interface ICoreProgramLifecycleCoreActualDiscardPhaseLedgerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TurnOwnerDiscardPhaseHandDiscardCount { get; }
}

internal interface ICoreProgramLifecycleProgramOwnedZoneDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramOwnedZoneDiscardRuntime Get();
}

internal interface ICoreProgramLifecycleCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreProgramLifecycleCoreTargetedTrickHandHpAiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    double GetTargetedTrickHandHpPublicScore(int owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> effects, global::CardGame.Core.ProgramSkillWindowContext? context);
}

internal interface ICoreProgramLifecycleCoreAttributedMarkerEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramAiPublicContext CreateAttributedPaymentAiContext(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger);
    bool HasAttributedEventOperations();
}

internal interface ICoreProgramLifecycleKernelCoreProgramTriggerRuntimeQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramTriggerQueries CreateProgramTriggerQueries();
}

internal interface ICoreProgramLifecycleCoreEndingPairDrawsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? PublishedEndingPairActorForAi(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> effects, global::CardGame.Core.ProgramSkillWindowContext? context);
}

internal interface ICoreProgramLifecycleCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDying(global::CardGame.Core.DyingFrame dying, bool survived);
    void ExposeHumanDyingPrompt();
}

internal interface ICoreProgramLifecycleCoreCurrentTurnHandExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveCurrentTurnHandExchangeChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PromptChoice supplied);
}

internal interface ICoreProgramLifecycleCharacterXiZhiCaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXianfuEchoTrigger(global::CardGame.Core.SkillProgramTrigger trigger);
    global::CardGame.Core.ProgramSkillWindowContext CreateXianfuDamageContext(global::CardGame.Core.DamageTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate c);
}

internal interface ICoreProgramLifecycleCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearActualDiscardRecoveryPhase();
}

internal interface ICoreProgramLifecycleCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreProgramLifecycleCoreActualEndedTurnEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveActualEndedEquipmentChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramLifecycleCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreProgramLifecycleCorePlayPhaseSkillGrantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CloseCurrentPlayPhaseSkillGrants(string reason);
}

internal interface ICoreProgramLifecycleCoreProgramPhaseHandDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveProgramPhaseHandDiscardChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreProgramLifecycleCharacterYuJinMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::CardGame.Core.ProgramAiEstimate Estimate, int? TargetSeat) EstimateMouXiayuanForAi(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramAiPublicContext context, global::CardGame.Core.ProgramSkillWindowContext? window);
    bool ValidMouXiayuan(global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.PromptChoice SelectAiMouXiayuanCost(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramLifecycleKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreProgramLifecycleCoreProgramOwnCardDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveProgramOwnCardDiscardChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleCoreActualPhaseEndsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginActualPhaseEnd(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, global::CardGame.Core.ActualPhaseEndContinuation continuation, int delayedEffects = 0, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false, long? parentFrameId = default);
}

internal interface ICoreProgramLifecycleCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreProgramLifecycleKernelCoreProgramTriggerFactProjectorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramTriggerFactQueries CreateProgramTriggerFactQueries();
    global::CardGame.Core.NativeOwners.ProgramTriggerActionFactQueries CreateProgramTriggerActionFactQueries();
}

internal interface ICoreProgramLifecycleCoreActualDiscardPhaseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDiscardPhase(bool beforeEntryPolicyCompleted = false);
}

internal interface ICoreProgramLifecycleCorePublicDamagePredictionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateAiSkillContext(global::CardGame.Core.CharacterState player, int viewerSeat, global::CardGame.Core.GameSnapshot? snapshot = default);
    global::System.Func<int, int, int>? CreatePublicSourceOccurrenceDamageBonus(int viewerSeat);
    global::CardGame.Core.PublicDamagePrediction PredictPublicActionDamage(int viewerSeat, global::CardGame.Core.LegalAction action, int targetSeat);
}

internal interface ICoreProgramLifecycleCharacterCaoPiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryChooseKemingEchoAi(global::CardGame.Core.PendingDecision decision, out global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramLifecycleCoreProgramOwnedCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveProgramOwnedCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleKernelCoreProgramRuntimeModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType);
}

internal interface ICoreProgramLifecycleCharacterLiJueOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LangxiChoices(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramLifecycleCoreCardUseDebitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseDebitActive(long actionId);
}

internal interface ICoreProgramLifecycleModeRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}

internal interface ICoreProgramLifecycleCharacterTaoQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateZhaohuoDamageContext(global::CardGame.Core.DamageTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreProgramLifecycleCoreJudgmentCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame? ActiveJudgment { get; }
}

internal interface ICoreProgramLifecycleCoreProgramOwnedCardDistributionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveProgramOwnedCardDistribution(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleCoreFixedRecipientBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? PublishedFixedRecipientForAi(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> effects, global::CardGame.Core.ProgramSkillWindowContext? context);
}

internal interface ICoreProgramLifecycleCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddThought(global::CardGame.Core.AiThoughtRecord thought);
}

internal interface ICoreProgramLifecycleCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginTurnStartAfterProgramLifecycle(global::CardGame.Core.CharacterState current);
    void FinalizeEndTurn(global::CardGame.Core.CharacterState previous);
    int GetTurnDrawCount(global::CardGame.Core.CharacterState current);
}

internal interface ICoreProgramLifecycleProgramTriggerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTriggerRuntime Get();
}

internal interface ICoreProgramLifecycleProgramInteractionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramInteractionRouter? _programInteractions { get; }
}

internal interface ICoreProgramLifecycleCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? GetJudgmentAttack(global::CardGame.Core.JudgmentFrame frame);
    global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame);
    global::CardGame.Core.GameEngine.IDamageAttempt? GetDyingAttack(global::CardGame.Core.DyingFrame frame);
}

internal interface ICoreProgramLifecycleCoreDyingEntryProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDyingEntryProgramWindow(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreProgramLifecycleProgramBoundaryInvariantOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramBoundaryInvariantRuntime Get();
}

internal interface ICoreProgramLifecycleProgramRandomCardAcquisitionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRandomCardAcquisitionRuntime Get();
}

internal interface ICoreProgramLifecycleCorePhaseHandSeizureReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPhaseHandDebtChoiceLegal(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramLifecycleCorePublicFactionCountsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetPublicLivingFactionCount();
}

internal interface ICoreProgramLifecycleCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesTreatedWounded { get; }

    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramLifecycleProgramDamageCardClaimOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDamageCardClaimRuntime Get();
}

internal interface ICoreProgramLifecycleCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreProgramLifecycleProgramWindowCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramWindowCursorRuntime Get();
}

internal interface ICoreProgramLifecycleProgramPindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramPindianRuntime Get();
}

internal interface ICoreProgramLifecycleCharacterZhugeGuoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> QirangExtraChoices(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreProgramLifecycleCoreProgramRevealsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveProgramRevealCardSelection(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleProgramTurnWindowEntryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTurnWindowEntryRuntime Get();
}

internal interface ICoreProgramLifecycleProgramTriggerFactProjectorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTriggerFactProjector Get();
}

internal interface ICoreProgramLifecycleProgramJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramJudgmentRuntime Get();
}

internal interface ICoreProgramLifecycleCoreProgramCardUseNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    double GetWholeCardUseNullificationAiScore(int ownerSeat, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreProgramLifecycleCoreProgramOtherOwnedCardDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveProgramOtherOwnedCardDiscardChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleCoreDiamondDelayedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDiamondDelayedChoiceLegal(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramLifecycleProgramDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDrawRuntime Get();
}

internal interface ICoreProgramLifecycleCoreProgramAttackRangeAidOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveProgramAttackRangeAid(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleProgramPhaseBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramPhaseBoundaryRuntime Get();
}

internal interface ICoreProgramLifecycleCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreProgramLifecycleCoreProgramSlashRequestOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveProgramRequestSlashChoice(global::CardGame.Core.PromptChoice selected);
    void ResolveProgramRequestSlashByNearestChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreProgramLifecycleProgramDrawDiscardWindowEntryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDrawDiscardWindowEntryRuntime Get();
}

internal interface ICoreProgramLifecycleCoreNearestLegalSlashRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveNearestLegalSlashChoice(global::CardGame.Core.PromptChoice selected);
    void ResolveUnlimitedVirtualSlashChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleCoreProgramParticipantPromptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? GetPendingDecisionForActor(int actorSeat);
}

internal interface ICoreProgramLifecycleKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreProgramLifecycleCorePrepDiscardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolvePrepDiscardChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramLifecycleCorePairObtainShownGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolvePairObtainChoice(global::CardGame.Core.PromptChoice choice);
    void ResolveShownPairGiftChoice(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreProgramLifecycleCoreHpDamageShieldsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    double GetHpDamageShieldAiTargetValue(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramLifecycleCoreStrategicProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResetPlayPhaseProgramStates(int ownerSeat);
}

internal interface ICoreProgramLifecycleCharacterBaoXinOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateBaoXinYimouContext(global::CardGame.Core.DamageTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext ordinary);
}

internal interface ICoreProgramLifecycleCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreProgramLifecycleResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramLifecycleCoreCurrentTurnUseKindsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksCurrentTurnUseKinds { get; }

    (int HandSuits, int Categories) CurrentTurnUseKinds(int actorSeat);
}

internal interface ICoreProgramLifecycleCoreEndRefillNextTurnOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    double EndRefillNextTurnPublicScore(int owner);
}

internal interface ICoreProgramLifecycleProgramHealthStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramHealthStateRuntime Get();
}

internal interface ICoreProgramLifecycleCoreProgramHoldCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveProgramHoldCardSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreProgramLifecycleCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}

internal interface ICoreProgramLifecycleCoreTurnDrawDebtsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksTurnDrawDebt { get; }
}

internal interface ICoreProgramLifecycleProgramBoundCardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramBoundCardRuntime Get();
}

internal interface ICoreProgramLifecycleCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreProgramLifecycleCoreProgramPairedColorResponseAiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    double GetPairedColorDispositionAiScore(global::CardGame.Core.SkillProgramTrigger trigger, int owner, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreProgramLifecycleCoreQuotaTopProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ComputeTurnCriterionQuota(int ownerSeat);
}

internal interface ICoreProgramLifecycleCoreSoleTargetProtectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveSoleProtectionChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramLifecycleCoreDyingRescueContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ProgramPaidDyingRescue(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreProgramLifecycleProgramActivationWindowOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramActivationWindowRuntime Get();
}

internal interface ICoreProgramLifecycleCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WouldEquipmentRemovalReduceCoverage(int subjectSeat, int equipmentCardId);
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
    void ResolveSelectAndMoveOwnedCardChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreProgramLifecycleCoreActivityOrdinaryTricksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ContentSkillDefinition ActivityRuntimeSkillDefinition(string id);
}

internal interface ICoreProgramLifecycleCoreRoundOnceCardlessProgramRequestsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResolveRoundOnceCardlessProgramSlash(global::CardGame.Core.PromptChoice choice, bool advance);
}

internal interface ICoreProgramLifecycleCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::CardGame.Core.ProgramAiEstimate Estimate, int? TargetSeat) EstimateMouKurouGiftForAi(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramAiPublicContext publicContext);
}

internal interface ICoreProgramLifecycleCharacterHuangQuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDianhuDrawTrigger(global::CardGame.Core.SkillProgramTrigger t);
    global::CardGame.Core.ProgramSkillWindowContext CreateDianhuDamageContext(global::CardGame.Core.DamageTriggerWindowFrame w, global::CardGame.Core.ProgramTriggerCandidate c);
}

internal interface ICoreProgramLifecycleCoreNativeDyingFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame UpdateDyingFrame(long frameId, global::System.Func<global::CardGame.Core.DyingFrame, global::CardGame.Core.DyingFrame> update);
    void SetDyingFrameStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreProgramLifecycleCorePhaseHandExtraTurnAiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramAiEstimate IncludePhaseHandExtraTurnDeclarationForAi(global::CardGame.Core.ProgramAiEstimate baseline, global::CardGame.Core.CharacterState owner, string skillId, string skillInstanceId, global::CardGame.Core.SkillProgram program, global::CardGame.Core.SkillProgramTrigger declaration, global::CardGame.Core.ProgramAiPublicContext context, global::CardGame.Core.ProgramSkillWindowContext windowContext);
}

internal interface ICoreProgramLifecycleCoreEquipmentDonationProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveEquipmentDonationChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice);
}
