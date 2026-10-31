#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramIronChainTargetsCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RuleQueryEvaluation EvaluateCardTargetCount(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind effectiveCardKind, long? declaredUseFrameId = default);
}

internal interface ICoreProgramIronChainTargetsCharacterPangTongMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouPangTongExpandedTargets(global::CardGame.Core.CharacterState owner);
}
