#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CorePlannedPhaseQuantitiesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlannedPhaseQuantitiesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public int GetProgramPlannedNormalDrawCount(global::CardGame.Core.ProgramSkillFrame frame, bool allowReplacedNoOp = false) => this._targetFactory().GetProgramPlannedNormalDrawCount(frame, allowReplacedNoOp);
    public void AdjustProgramNormalDraw(global::CardGame.Core.ProgramSkillFrame frame, int amount) => this._targetFactory().AdjustProgramNormalDraw(frame, amount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed class CorePlannedPhaseQuantitiesCoreRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlannedPhaseQuantitiesCoreRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RuleQueryContribution> CollectNumericRuleContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query, global::CardGame.Core.CardKind? effectiveCardKind = default) => this._targetFactory().CollectNumericRuleContributions(player, query, effectiveCardKind);
}
