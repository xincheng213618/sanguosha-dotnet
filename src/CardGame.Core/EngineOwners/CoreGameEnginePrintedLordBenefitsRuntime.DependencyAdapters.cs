#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CorePrintedLordBenefitsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordBenefitsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBlackGiftContestsRuntime))]
internal sealed class CorePrintedLordBenefitsCoreBlackGiftContestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBlackGiftContestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordBenefitsCoreBlackGiftContestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBlackGiftContestsRuntime> _targetFactory = targetFactory;
    public bool DrainRecipientConsequences(global::CardGame.Core.ProgramSkillFrame f, bool movement) => this._targetFactory().DrainRecipientConsequences(f, movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CorePrintedLordBenefitsCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordBenefitsCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().FinishProgramSkill(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime))]
internal sealed class CorePrintedLordBenefitsCorePrintedLordSkillQualificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordBenefitsCorePrintedLordSkillQualificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PrintedLordSkillQualification> CapturePrintedLordQualifications(global::CardGame.Core.ProgramSkillFrame f, int seat) => this._targetFactory().CapturePrintedLordQualifications(f, seat);
    public void IssueCapturedPrintedLordQualifications(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PrintedLordBenefitReceipt r) => this._targetFactory().IssueCapturedPrintedLordQualifications(f, r);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CorePrintedLordBenefitsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordBenefitsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramTrigger(frame);
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CorePrintedLordBenefitsCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordBenefitsCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime))]
internal sealed class CorePrintedLordBenefitsCoreRecipientConsequencesReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordBenefitsCoreRecipientConsequencesReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime> _targetFactory = targetFactory;
    public bool RecipientConsequencesGameEnded() => this._targetFactory().RecipientConsequencesGameEnded();
    public void AssertRecipientConsequences(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().AssertRecipientConsequences(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CorePrintedLordBenefitsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordBenefitsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CorePrintedLordBenefitsCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordBenefitsCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAdvancedCard(int id) => this._targetFactory().GetAdvancedCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CorePrintedLordBenefitsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordBenefitsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CorePrintedLordBenefitsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePrintedLordBenefitsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}
