#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime))]
internal sealed class CoreJudgmentProgramsCoreJudgmentCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreJudgmentCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentFrame? ActiveJudgment { get => this._targetFactory().ActiveJudgment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreJudgmentProgramsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetJudgmentCard(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime))]
internal sealed class CoreJudgmentProgramsCharacterZhangQiYingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCharacterZhangQiYingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime> _targetFactory = targetFactory;
    public int EffectiveZhenyiJudgmentRank(global::CardGame.Core.JudgmentFrame frame, int fallback) => this._targetFactory().EffectiveZhenyiJudgmentRank(frame, fallback);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreJudgmentProgramsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int RelativeTableOrder(int from, int target) => this._targetFactory().RelativeTableOrder(from, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime))]
internal sealed class CoreJudgmentProgramsCoreJudgmentOutcomeDamageEdgesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreJudgmentOutcomeDamageEdgesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentOutcomeDamageEdgesRuntime> _targetFactory = targetFactory;
    public bool IsExactJudgmentOutcomeFinalizedSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int index) => this._targetFactory().IsExactJudgmentOutcomeFinalizedSubtree(window, judgment, index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime))]
internal sealed class CoreJudgmentProgramsCoreNativeJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreNativeJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> _targetFactory = targetFactory;
    public bool? CompleteFinalizedJudgment(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.Card judgmentCard, bool succeeded) => this._targetFactory().CompleteFinalizedJudgment(pending, judgmentCard, succeeded);
    public void ResumeCompletedJudgment(global::CardGame.Core.GameEngine.JudgmentCompletionReceipt receipt) => this._targetFactory().ResumeCompletedJudgment(receipt);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramWindowCursorRuntime))]
internal sealed class CoreJudgmentProgramsProgramWindowCursorAdapter(global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsProgramWindowCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramWindowCursorRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime))]
internal sealed class CoreJudgmentProgramsCoreOwnedDamagePointJudgmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreOwnedDamagePointJudgmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> _targetFactory = targetFactory;
    public bool IsExactOwnedDamagePointJudgmentFinalizedChildSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int windowIndex) => this._targetFactory().IsExactOwnedDamagePointJudgmentFinalizedChildSubtree(window, judgment, windowIndex);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreJudgmentProgramsCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().EnabledUniqueProgramTriggers(player, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreJudgmentProgramsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> GetProgramTargetSeats(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind targetKind, global::CardGame.Core.ProgramSkillWindowContext? windowContext = default, global::CardGame.Core.PlayerMarkerKind? marker = default, global::CardGame.Core.ProgramParticipantReference? actorReference = default) => this._targetFactory().GetProgramTargetSeats(ownerSeat, targetKind, windowContext, marker, actorReference);
    public bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CanRunProgramTrigger(candidate, context);
    public void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().BeginProgramBinding(candidate, context);
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureProgramTriggerFacts(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineYaodaoChitongRuntime))]
internal sealed class CoreJudgmentProgramsCoreYaodaoChitongAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineYaodaoChitongRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreYaodaoChitongOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineYaodaoChitongRuntime> _targetFactory = targetFactory;
    public bool IsExactYaodaoChitongJudgmentFinalizedSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int index) => this._targetFactory().IsExactYaodaoChitongJudgmentFinalizedSubtree(window, judgment, index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime))]
internal sealed class CoreJudgmentProgramsCharacterZhangFeiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCharacterZhangFeiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramJudgmentTriggerCandidate[] PrependShenZfJudgmentGain(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramJudgmentTriggerCandidate[] candidates) => this._targetFactory().PrependShenZfJudgmentGain(j, candidates);
    public bool IsExactShenZfJudgmentGainSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int index) => this._targetFactory().IsExactShenZfJudgmentGainSubtree(window, judgment, index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime))]
internal sealed class CoreJudgmentProgramsCharacterXiZhiCaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCharacterXiZhiCaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime> _targetFactory = targetFactory;
    public bool IsExactChouceFinalizedChildSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame? judgmentFrame, int frameIndex) => this._targetFactory().IsExactChouceFinalizedChildSubtree(frame, judgmentFrame, frameIndex);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime))]
internal sealed class CoreJudgmentProgramsCoreDamageJudgmentSuitPaymentObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreDamageJudgmentSuitPaymentObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentObserversRuntime> _targetFactory = targetFactory;
    public bool IsExactDamageJudgmentSuitPaymentFinalizedSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int index) => this._targetFactory().IsExactDamageJudgmentSuitPaymentFinalizedSubtree(window, judgment, index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreJudgmentProgramsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreJudgmentProgramsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode) => this._targetFactory().ValidateHumanPrompt(actorSeat, expectedKind, promptId, inactiveCode);
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaYuanYiOlRuntime))]
internal sealed class CoreJudgmentProgramsCharacterMaYuanYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaYuanYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCharacterMaYuanYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaYuanYiOlRuntime> _targetFactory = targetFactory;
    public bool IsExactBinghuoFinalizedChildSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame? judgment, int frameIndex) => this._targetFactory().IsExactBinghuoFinalizedChildSubtree(frame, judgment, frameIndex);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreJudgmentProgramsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame => this._targetFactory().AdvanceRuntimeTop<TFrame>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime))]
internal sealed class CoreJudgmentProgramsCoreExactRepeatedJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreExactRepeatedJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime> _targetFactory = targetFactory;
    public bool IsExactRepeatedJudgmentFinalizedChildSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int windowIndex) => this._targetFactory().IsExactRepeatedJudgmentFinalizedChildSubtree(window, judgment, windowIndex);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime))]
internal sealed class CoreJudgmentProgramsCoreZhenyiJudgmentWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreZhenyiJudgmentWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTriggerWindow CurrentZhenyiJudgmentWindowKind(long judgmentId) => this._targetFactory().CurrentZhenyiJudgmentWindowKind(judgmentId);
    public global::CardGame.Core.ProgramJudgmentTriggerWindowFrame RefreshZhenyiBeforeResultWindow(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame pending) => this._targetFactory().RefreshZhenyiBeforeResultWindow(frame, pending);
    public void CompleteZhenyiBeforeResultWindow(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame pending) => this._targetFactory().CompleteZhenyiBeforeResultWindow(frame, pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime))]
internal sealed class CoreJudgmentProgramsCharacterZhouQunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCharacterZhouQunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> _targetFactory = targetFactory;
    public bool IsExactNingwuFinalizedChildSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame? judgment, int index) => this._targetFactory().IsExactNingwuFinalizedChildSubtree(frame, judgment, index);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime))]
internal sealed class CoreJudgmentProgramsCoreNativeSkillContextQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreNativeSkillContextQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false) => this._targetFactory().CreateSkillContext(player, includeHandLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreJudgmentProgramsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreJudgmentProgramsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}
