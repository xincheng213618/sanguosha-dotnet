#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreDelegatedJudgmentChoicesCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDelegatedJudgmentChoicesCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetJudgmentCard(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime))]
internal sealed class CoreDelegatedJudgmentChoicesCoreJudgmentCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDelegatedJudgmentChoicesCoreJudgmentCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentFrame? ActiveJudgment { get => this._targetFactory().ActiveJudgment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime))]
internal sealed class CoreDelegatedJudgmentChoicesCoreNativeJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDelegatedJudgmentChoicesCoreNativeJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentTriggerCandidate? CurrentJudgmentCandidate(global::CardGame.Core.JudgmentFrame pending) => this._targetFactory().CurrentJudgmentCandidate(pending);
    public void SetJudgmentFrameStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetJudgmentFrameStep(frameId, step);
    public void ReplaceJudgmentFrame(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().ReplaceJudgmentFrame(frame);
    public global::CardGame.Core.JudgmentFrame GetJudgmentFrame(long frameId) => this._targetFactory().GetJudgmentFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreDelegatedJudgmentChoicesCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDelegatedJudgmentChoicesCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreDelegatedJudgmentChoicesCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDelegatedJudgmentChoicesCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreDelegatedJudgmentChoicesCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDelegatedJudgmentChoicesCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreDelegatedJudgmentChoicesCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDelegatedJudgmentChoicesCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreDelegatedJudgmentChoicesCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDelegatedJudgmentChoicesCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CoreDelegatedJudgmentChoicesCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDelegatedJudgmentChoicesCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime))]
internal sealed class CoreDelegatedJudgmentChoicesCoreSnapshotValueProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDelegatedJudgmentChoicesCoreSnapshotValueProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card) => this._targetFactory().ToSnapshot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime))]
internal sealed class CoreDelegatedJudgmentChoicesCoreJudgmentReplacementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDelegatedJudgmentChoicesCoreJudgmentReplacementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> _targetFactory = targetFactory;
    public (global::CardGame.Core.SkillProgram Program, global::CardGame.Core.SkillProgramTrigger Trigger) GetProgramJudgmentReplacement(global::CardGame.Core.JudgmentTriggerCandidate candidate) => this._targetFactory().GetProgramJudgmentReplacement(candidate);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetProgramJudgmentReplacementCards(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger, int judgmentSubjectSeat, string judgmentReason, bool ignoreCurrentColor = false) => this._targetFactory().GetProgramJudgmentReplacementCards(owner, trigger, judgmentSubjectSeat, judgmentReason, ignoreCurrentColor);
    public void ResolveProgramJudgmentReplacementChoice(global::CardGame.Core.PromptChoice selected) => this._targetFactory().ResolveProgramJudgmentReplacementChoice(selected);
}
