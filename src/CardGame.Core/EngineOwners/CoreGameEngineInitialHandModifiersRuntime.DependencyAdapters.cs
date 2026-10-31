#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuLeRuntime))]
internal sealed class CoreInitialHandModifiersCharacterZhouYuLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreInitialHandModifiersCharacterZhouYuLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuLeRuntime> _targetFactory = targetFactory;
    public int ApplyGuYinInitialHandSize(global::CardGame.Core.CharacterState player, int ordinaryCount) => this._targetFactory().ApplyGuYinInitialHandSize(player, ordinaryCount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed class CoreInitialHandModifiersCoreRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreInitialHandModifiersCoreRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RuleQueryContribution> CollectNumericRuleContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query, global::CardGame.Core.CardKind? effectiveCardKind = default) => this._targetFactory().CollectNumericRuleContributions(player, query, effectiveCardKind);
}
