#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime))]
internal sealed class CoreShanheAlliedTricksCoreNativeHarvestAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheAlliedTricksCoreNativeHarvestOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> _targetFactory = targetFactory;
    public void BeginHarvestSelection(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().BeginHarvestSelection(group);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreShanheAlliedTricksCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheAlliedTricksCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreShanheAlliedTricksCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheAlliedTricksCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreShanheAlliedTricksCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheAlliedTricksCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime))]
internal sealed class CoreShanheAlliedTricksCoreGroupTargetNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheAlliedTricksCoreGroupTargetNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind GroupNullificationCardKind(global::CardGame.Core.NativeOwners.GroupCardHandle group) => this._targetFactory().GroupNullificationCardKind(group);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuideSkillsRuntime))]
internal sealed class CoreShanheAlliedTricksCoreShanheGuideSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuideSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheAlliedTricksCoreShanheGuideSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuideSkillsRuntime> _targetFactory = targetFactory;
    public bool IsShanheFriendlySeat(int owner, int seat) => this._targetFactory().IsShanheFriendlySeat(owner, seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreShanheAlliedTricksCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheAlliedTricksCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}
