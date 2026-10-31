#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime))]
internal sealed class CoreRuntimeCorePaidOwnTargetObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCorePaidOwnTargetObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> _targetFactory = targetFactory;
    public void FinishPaidOwnTargetBeforeProgramCompletion(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().FinishPaidOwnTargetBeforeProgramCompletion(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime))]
internal sealed class CoreRuntimeCoreRequestedDeckBasicsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreRequestedDeckBasicsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> _targetFactory = targetFactory;
    public bool TryAdvanceRequestedDeckBasicAi() => this._targetFactory().TryAdvanceRequestedDeckBasicAi();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreRuntimeCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public void ResolvePendingAiProgramTrigger() => this._targetFactory().ResolvePendingAiProgramTrigger();
    public void CompleteCurrentPlayPhase() => this._targetFactory().CompleteCurrentPlayPhase();
    public bool TryBeginDiscardPhaseEndedProgramWindow(global::CardGame.Core.CharacterState phaseOwner) => this._targetFactory().TryBeginDiscardPhaseEndedProgramWindow(phaseOwner);
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramTrigger(frame);
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime))]
internal sealed class CoreRuntimeCoreParticipantAndReserveProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreParticipantAndReserveProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime> _targetFactory = targetFactory;
    public bool BeginGameStartingPrograms() => this._targetFactory().BeginGameStartingPrograms();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime))]
internal sealed class CoreRuntimeCoreWenheChaosAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreWenheChaosOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime> _targetFactory = targetFactory;
    public bool TryCompleteWenheChaosProgram(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().TryCompleteWenheChaosProgram(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreRuntimeCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public void ResolveAiRecoveryReplacement() => this._targetFactory().ResolveAiRecoveryReplacement();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime))]
internal sealed class CoreRuntimeCoreProgramRepeatedJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreProgramRepeatedJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> _targetFactory = targetFactory;
    public bool IsAiProgramRepeatJudgmentPending() => this._targetFactory().IsAiProgramRepeatJudgmentPending();
    public void ResolvePendingAiProgramRepeatJudgment() => this._targetFactory().ResolvePendingAiProgramRepeatJudgment();
    public void ContinueProgramRepeatedJudgmentAfterMovement(long frameId) => this._targetFactory().ContinueProgramRepeatedJudgmentAfterMovement(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime))]
internal sealed class CoreRuntimeCharacterLiuZhangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCharacterLiuZhangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> _targetFactory = targetFactory;
    public bool TryAdvanceZonghuAi() => this._targetFactory().TryAdvanceZonghuAi();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime))]
internal sealed class CoreRuntimeCoreTurnHandCategoryRestrictionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreTurnHandCategoryRestrictionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _targetFactory = targetFactory;
    public int RequiredSelfHandLimitDiscard(global::CardGame.Core.CharacterState owner) => this._targetFactory().RequiredSelfHandLimitDiscard(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime))]
internal sealed class CoreRuntimeCoreMantianGuohaiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreMantianGuohaiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> _targetFactory = targetFactory;
    public bool TryCompleteMantianGuohaiGift(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().TryCompleteMantianGuohaiGift(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime))]
internal sealed class CoreRuntimeCoreShenwuCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreShenwuCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> _targetFactory = targetFactory;
    public bool TryCompleteShenwuTransplantProgram(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().TryCompleteShenwuTransplantProgram(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime))]
internal sealed class CoreRuntimeCoreActualDiscardPhaseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreActualDiscardPhaseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> _targetFactory = targetFactory;
    public void AutoDiscard(global::CardGame.Core.CharacterState player) => this._targetFactory().AutoDiscard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreRuntimeCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public bool IsAiSkipDiscardPolicyPending() => this._targetFactory().IsAiSkipDiscardPolicyPending();
    public bool IsAiTargetCardSelectionPending() => this._targetFactory().IsAiTargetCardSelectionPending();
    public bool IsAiFireAttackPending() => this._targetFactory().IsAiFireAttackPending();
    public bool IsAiNullificationPending() => this._targetFactory().IsAiNullificationPending();
    public bool IsAiResponsePending() => this._targetFactory().IsAiResponsePending();
    public bool IsAiDyingResponsePending() => this._targetFactory().IsAiDyingResponsePending();
    public bool IsAiHarvestPending() => this._targetFactory().IsAiHarvestPending();
    public void RequestHumanPlay() => this._targetFactory().RequestHumanPlay();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime))]
internal sealed class CoreRuntimeCoreJudgmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreJudgmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime> _targetFactory = targetFactory;
    public bool IsAiProgramJudgmentPending() => this._targetFactory().IsAiProgramJudgmentPending();
    public void ResolvePendingAiProgramJudgment() => this._targetFactory().ResolvePendingAiProgramJudgment();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed class CoreRuntimeCoreWenhePoisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreWenhePoisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _targetFactory = targetFactory;
    public bool IsCurrentWenhePoisonDying() => this._targetFactory().IsCurrentWenhePoisonDying();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramTopReorderRuntime))]
internal sealed class CoreRuntimeCoreProgramTopReorderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTopReorderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreProgramTopReorderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTopReorderRuntime> _targetFactory = targetFactory;
    public bool IsAiProgramTopReorderPending() => this._targetFactory().IsAiProgramTopReorderPending();
    public void ResolvePendingAiProgramTopReorder() => this._targetFactory().ResolvePendingAiProgramTopReorder();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreRuntimeCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime))]
internal sealed class CoreRuntimeCoreForcedPhaseAlcoholAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreForcedPhaseAlcoholOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> _targetFactory = targetFactory;
    public bool CanContinuePhaseAlcohol(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CanContinuePhaseAlcohol(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime))]
internal sealed class CoreRuntimeCoreNativeGroupRecoveryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreNativeGroupRecoveryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime> _targetFactory = targetFactory;
    public void RunOneGroupRecoveryStep() => this._targetFactory().RunOneGroupRecoveryStep();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime))]
internal sealed class CoreRuntimeCoreJudgmentReplacementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreJudgmentReplacementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> _targetFactory = targetFactory;
    public bool IsAiProgramJudgmentReplacementPending() => this._targetFactory().IsAiProgramJudgmentReplacementPending();
    public void ResolvePendingAiProgramJudgmentReplacement() => this._targetFactory().ResolvePendingAiProgramJudgmentReplacement();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsRuntime))]
internal sealed class CoreRuntimeCoreDamageTargetMountProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreDamageTargetMountProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsRuntime> _targetFactory = targetFactory;
    public bool IsValidDamageTargetMount(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsValidDamageTargetMount(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.NativeFrameAutomaticRuntime))]
internal sealed class CoreRuntimeNativeFrameAutomaticAdapter(global::System.Func<global::CardGame.Core.NativeFrameAutomaticRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeNativeFrameAutomaticOperations
{
    private readonly global::System.Func<global::CardGame.Core.NativeFrameAutomaticRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeFrameAutomaticRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreRuntimeCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime))]
internal sealed class CoreRuntimeCorePindianAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCorePindianOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRuntime> _targetFactory = targetFactory;
    public void ResolvePendingAiPindian() => this._targetFactory().ResolvePendingAiPindian();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreRuntimeCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime))]
internal sealed class CoreRuntimeCoreDamageTargetObtainProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreDamageTargetObtainProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> _targetFactory = targetFactory;
    public bool IsValidDamageTargetObtain(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsValidDamageTargetObtain(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreRuntimeCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public void BeginTurn() => this._targetFactory().BeginTurn();
    public void EndTurn() => this._targetFactory().EndTurn();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime))]
internal sealed class CoreRuntimeCoreNativeAiDecisionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreNativeAiDecisionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> _targetFactory = targetFactory;
    public void ResolvePendingAiSkipDiscardPolicy() => this._targetFactory().ResolvePendingAiSkipDiscardPolicy();
    public void ResolvePendingAiTargetCardSelection() => this._targetFactory().ResolvePendingAiTargetCardSelection();
    public void ResolvePendingAiFireAttack() => this._targetFactory().ResolvePendingAiFireAttack();
    public void ResolvePendingAiNullification() => this._targetFactory().ResolvePendingAiNullification();
    public void ResolvePendingAiResponse() => this._targetFactory().ResolvePendingAiResponse();
    public void ResolvePendingAiHarvest() => this._targetFactory().ResolvePendingAiHarvest();
    public void RunOneAiPlayDecision(global::CardGame.Core.CharacterState player) => this._targetFactory().RunOneAiPlayDecision(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillPromptsRuntime))]
internal sealed class CoreRuntimeCoreSkillPromptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillPromptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreSkillPromptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillPromptsRuntime> _targetFactory = targetFactory;
    public bool IsAiPindianPending() => this._targetFactory().IsAiPindianPending();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime))]
internal sealed class CoreRuntimeCoreHpConditionalSkillGrantsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreHpConditionalSkillGrantsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime> _targetFactory = targetFactory;
    public bool CanContinueHpConditionalProgram(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CanContinueHpConditionalProgram(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MatchStepDispatcher))]
internal sealed class CoreRuntimeMatchStepDispatcherAdapter(global::System.Func<global::CardGame.Core.MatchStepDispatcher> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeMatchStepDispatcherOperations
{
    private readonly global::System.Func<global::CardGame.Core.MatchStepDispatcher> _targetFactory = targetFactory;
    public global::CardGame.Core.MatchStepDispatcher Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime))]
internal sealed class CoreRuntimeCoreFirstSlashDuelLegacyWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreFirstSlashDuelLegacyWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> _targetFactory = targetFactory;
    public void CompleteFirstLegacyBenefitCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CompleteFirstLegacyBenefitCandidate(c, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime))]
internal sealed class CoreRuntimeCoreTypedRecoveryDamageCursorAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreTypedRecoveryDamageCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.TypedResponseCompletionHealthCursorProjection ProjectTypedResponseCompletionHealthCursor() => this._targetFactory().ProjectTypedResponseCompletionHealthCursor();
    public global::CardGame.Core.TypedCommittedSlashPaymentCursorProjection ProjectTypedCommittedSlashPaymentCursor() => this._targetFactory().ProjectTypedCommittedSlashPaymentCursor();
    public bool HasTypedRecoveryProgramDying() => this._targetFactory().HasTypedRecoveryProgramDying();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime))]
internal sealed class CoreRuntimeCoreOwnedDeathBenefitReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreOwnedDeathBenefitReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> _targetFactory = targetFactory;
    public void FinishOwnedDeathBenefitReturn(global::CardGame.Core.ProgramSkillFrame f, bool completed) => this._targetFactory().FinishOwnedDeathBenefitReturn(f, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.NativeFrameRoutingRuntime))]
internal sealed class CoreRuntimeNativeFrameRoutingAdapter(global::System.Func<global::CardGame.Core.NativeFrameRoutingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeNativeFrameRoutingOperations
{
    private readonly global::System.Func<global::CardGame.Core.NativeFrameRoutingRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeFrameRoutingRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime))]
internal sealed class CoreRuntimeCoreCardDeclarationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreCardDeclarationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> _targetFactory = targetFactory;
    public void ContinueCardDeclarationChallenge(long id) => this._targetFactory().ContinueCardDeclarationChallenge(id);
    public void AnswerCardDeclaration(global::CardGame.Core.PromptChoice choice) => this._targetFactory().AnswerCardDeclaration(choice);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime))]
internal sealed class CoreRuntimeCoreNativeDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreNativeDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> _targetFactory = targetFactory;
    public void RunOneDyingStep(bool skipRequestedDeckBasic = false) => this._targetFactory().RunOneDyingStep(skipRequestedDeckBasic);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreRuntimeCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime))]
internal sealed class CoreRuntimeCoreChainedStateBasicConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreChainedStateBasicConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> _targetFactory = targetFactory;
    public bool TryAdvanceChainedStateBasicAi() => this._targetFactory().TryAdvanceChainedStateBasicAi();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHeQiOlRuntime))]
internal sealed class CoreRuntimeCharacterHeQiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCharacterHeQiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiOlRuntime> _targetFactory = targetFactory;
    public bool CanContinueEquipmentSuitProgram(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CanContinueEquipmentSuitProgram(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CoreRuntimeCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CoreRuntimeCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public bool TryAdvanceWeijieAi() => this._targetFactory().TryAdvanceWeijieAi();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramCompletionRuntime))]
internal sealed class CoreRuntimeProgramCompletionAdapter(global::System.Func<global::CardGame.Core.ProgramCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeProgramCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramCompletionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCompletionRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed class CoreRuntimeCoreHellSoulCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreHellSoulCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> _targetFactory = targetFactory;
    public bool TryCompleteHellSoulProgram(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().TryCompleteHellSoulProgram(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime))]
internal sealed class CoreRuntimeCoreDrawFundedDistinctBasicResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreDrawFundedDistinctBasicResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime> _targetFactory = targetFactory;
    public bool TryAdvanceDrawFundedDistinctBasicAi() => this._targetFactory().TryAdvanceDrawFundedDistinctBasicAi();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime))]
internal sealed class CoreRuntimeCoreNativeDamageWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreNativeDamageWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageWindowsRuntime> _targetFactory = targetFactory;
    public void RunOneDamageTriggerStep() => this._targetFactory().RunOneDamageTriggerStep();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime))]
internal sealed class CoreRuntimeCoreDiscardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardRuntime> _targetFactory = targetFactory;
    public void RequestHumanDiscard(global::CardGame.Core.CharacterState player) => this._targetFactory().RequestHumanDiscard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed class CoreRuntimeCoreProgramCardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreProgramCardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _targetFactory = targetFactory;
    public int CountLivingInAttackRange(int subjectSeat) => this._targetFactory().CountLivingInAttackRange(subjectSeat);
    public void SetProgramAttackRangeCoverage(long frameId, string name, int subjectSeat, int beforeCount, int afterCount) => this._targetFactory().SetProgramAttackRangeCoverage(frameId, name, subjectSeat, beforeCount, afterCount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime))]
internal sealed class CoreRuntimeCharacterLuMengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCharacterLuMengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouRuntime> _targetFactory = targetFactory;
    public bool IsAiYingboPending() => this._targetFactory().IsAiYingboPending();
    public void ResolvePendingAiYingbo() => this._targetFactory().ResolvePendingAiYingbo();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreRuntimeCoreProgramRuntimeHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreProgramRuntimeHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CoreRuntimeCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public bool TryAdvanceHuanpeiAi() => this._targetFactory().TryAdvanceHuanpeiAi();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime))]
internal sealed class CoreRuntimeCoreDeferredHandAlignmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreDeferredHandAlignmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredHandAlignmentsRuntime> _targetFactory = targetFactory;
    public bool CompleteDeferredTurnEndBinding(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CompleteDeferredTurnEndBinding(frame, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime))]
internal sealed class CoreRuntimeCoreMatchParticipantSetupAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCoreMatchParticipantSetupOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime> _targetFactory = targetFactory;
    public void RunOneSetupStep() => this._targetFactory().RunOneSetupStep();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime))]
internal sealed class CoreRuntimeCharacterCaoPiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCharacterCaoPiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool TryCompleteKemingProgram(global::CardGame.Core.ProgramSkillFrame f, bool completed) => this._targetFactory().TryCompleteKemingProgram(f, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime))]
internal sealed class CoreRuntimeCharacterLiuFengBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreRuntimeCharacterLiuFengBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> _targetFactory = targetFactory;
    public bool TryAdvanceBoundaryLiuFengAi() => this._targetFactory().TryAdvanceBoundaryLiuFengAi();
}
