#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRuntimeResolutionSchedulerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ResolutionScheduler _resolutionScheduler { get; }
}

internal interface ICoreRuntimeKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteGame();
}

internal interface ICoreRuntimeCorePaidOwnTargetObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishPaidOwnTargetBeforeProgramCompletion(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreRuntimeCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceRequestedDeckBasicAi();
}

internal interface ICoreRuntimeCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolvePendingAiProgramTrigger();
    void CompleteCurrentPlayPhase();
    bool TryBeginDiscardPhaseEndedProgramWindow(global::CardGame.Core.CharacterState phaseOwner);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}

internal interface ICoreRuntimeResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRuntimeCoreParticipantAndReserveProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool BeginGameStartingPrograms();
}

internal interface ICoreRuntimeCoreWenheChaosOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteWenheChaosProgram(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreRuntimeCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveAiRecoveryReplacement();
}

internal interface ICoreRuntimeMatchExecutionLoopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchExecutionLoop _matchExecutionLoop { get; }
}

internal interface ICoreRuntimeKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreRuntimeCoreProgramRepeatedJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAiProgramRepeatJudgmentPending();
    void ResolvePendingAiProgramRepeatJudgment();
    void ContinueProgramRepeatedJudgmentAfterMovement(long frameId);
}

internal interface ICoreRuntimeCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceZonghuAi();
}

internal interface ICoreRuntimeCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RequiredSelfHandLimitDiscard(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreRuntimeCoreMantianGuohaiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteMantianGuohaiGift(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreRuntimeCoreShenwuCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteShenwuTransplantProgram(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreRuntimeCoreActualDiscardPhaseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AutoDiscard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreRuntimeCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAiSkipDiscardPolicyPending();
    bool IsAiTargetCardSelectionPending();
    bool IsAiFireAttackPending();
    bool IsAiNullificationPending();
    bool IsAiResponsePending();
    bool IsAiDyingResponsePending();
    bool IsAiHarvestPending();
    void RequestHumanPlay();
}

internal interface ICoreRuntimeCoreJudgmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAiProgramJudgmentPending();
    void ResolvePendingAiProgramJudgment();
}

internal interface ICoreRuntimeCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentWenhePoisonDying();
}

internal interface ICoreRuntimeCoreProgramTopReorderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAiProgramTopReorderPending();
    void ResolvePendingAiProgramTopReorder();
}

internal interface ICoreRuntimeProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreRuntimeCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRuntimeCoreForcedPhaseAlcoholOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanContinuePhaseAlcohol(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreRuntimeCoreNativeGroupRecoveryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RunOneGroupRecoveryStep();
}

internal interface ICoreRuntimeKernelCoreGameEngineProgramContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnMovement(global::CardGame.Core.GameEngine.ProgramContinuationCatalog __nativeReceiver, global::CardGame.Core.ProgramSkillFrame frame, bool afterPayment = false);
}

internal interface ICoreRuntimeCoreJudgmentReplacementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAiProgramJudgmentReplacementPending();
    void ResolvePendingAiProgramJudgmentReplacement();
}

internal interface ICoreRuntimeCoreDamageTargetMountProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidDamageTargetMount(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreRuntimeNativeFrameAutomaticOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeFrameAutomaticRuntime Get();
}

internal interface ICoreRuntimeCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreRuntimeCorePindianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolvePendingAiPindian();
}

internal interface ICoreRuntimeCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreRuntimeNativeCardGameCoreProgramWindowReturnCatalogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void Return(global::CardGame.Core.ProgramWindowReturnCatalog __nativeReceiver, global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreRuntimeCoreDamageTargetObtainProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidDamageTargetObtain(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreRuntimeCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginTurn();
    void EndTurn();
}

internal interface ICoreRuntimeCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolvePendingAiSkipDiscardPolicy();
    void ResolvePendingAiTargetCardSelection();
    void ResolvePendingAiFireAttack();
    void ResolvePendingAiNullification();
    void ResolvePendingAiResponse();
    void ResolvePendingAiHarvest();
    void RunOneAiPlayDecision(global::CardGame.Core.CharacterState player);
}

internal interface ICoreRuntimeCoreSkillPromptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAiPindianPending();
}

internal interface ICoreRuntimeCoreHpConditionalSkillGrantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanContinueHpConditionalProgram(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreRuntimeMatchStepDispatcherOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MatchStepDispatcher Get();
}

internal interface ICoreRuntimeCoreFirstSlashDuelLegacyWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteFirstLegacyBenefitCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreRuntimeModeRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}

internal interface ICoreRuntimeCoreTypedRecoveryDamageCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.TypedResponseCompletionHealthCursorProjection ProjectTypedResponseCompletionHealthCursor();
    global::CardGame.Core.TypedCommittedSlashPaymentCursorProjection ProjectTypedCommittedSlashPaymentCursor();
    bool HasTypedRecoveryProgramDying();
}

internal interface ICoreRuntimeCoreOwnedDeathBenefitReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishOwnedDeathBenefitReturn(global::CardGame.Core.ProgramSkillFrame f, bool completed);
}

internal interface ICoreRuntimeNativeFrameRoutingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeFrameRoutingRuntime Get();
}

internal interface ICoreRuntimeCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueCardDeclarationChallenge(long id);
    void AnswerCardDeclaration(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreRuntimeCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RunOneDyingStep(bool skipRequestedDeckBasic = false);
}

internal interface ICoreRuntimeCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreRuntimeCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceChainedStateBasicAi();
}

internal interface ICoreRuntimeCharacterHeQiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanContinueEquipmentSuitProgram(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreRuntimeCoreDamageCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
}

internal interface ICoreRuntimeCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceWeijieAi();
}

internal interface ICoreRuntimeProgramCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCompletionRuntime Get();
}

internal interface ICoreRuntimeCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteHellSoulProgram(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreRuntimeCoreDrawFundedDistinctBasicResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceDrawFundedDistinctBasicAi();
}

internal interface ICoreRuntimeCoreNativeDamageWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RunOneDamageTriggerStep();
}

internal interface ICoreRuntimeCoreDiscardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RequestHumanDiscard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreRuntimeCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int CountLivingInAttackRange(int subjectSeat);
    void SetProgramAttackRangeCoverage(long frameId, string name, int subjectSeat, int beforeCount, int afterCount);
}

internal interface ICoreRuntimeCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAiYingboPending();
    void ResolvePendingAiYingbo();
}

internal interface ICoreRuntimeCoreProgramRuntimeHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreRuntimeCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceHuanpeiAi();
}

internal interface ICoreRuntimeCoreDeferredHandAlignmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CompleteDeferredTurnEndBinding(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreRuntimeCoreMatchParticipantSetupOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RunOneSetupStep();
}

internal interface ICoreRuntimeCharacterCaoPiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteKemingProgram(global::CardGame.Core.ProgramSkillFrame f, bool completed);
}

internal interface ICoreRuntimeCharacterLiuFengBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvanceBoundaryLiuFengAi();
}
