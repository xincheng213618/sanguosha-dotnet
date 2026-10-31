#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed class CoreProgramIronChainTargetsCoreRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramIronChainTargetsCoreRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.RuleQueryEvaluation EvaluateCardTargetCount(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind effectiveCardKind, long? declaredUseFrameId = default) => this._targetFactory().EvaluateCardTargetCount(player, effectiveCardKind, declaredUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangTongMouMobileRuntime))]
internal sealed class CoreProgramIronChainTargetsCharacterPangTongMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramIronChainTargetsCharacterPangTongMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasMouPangTongExpandedTargets(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasMouPangTongExpandedTargets(owner);
}
