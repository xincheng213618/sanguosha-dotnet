#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreInitialHandModifiersModeRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}

internal interface ICoreInitialHandModifiersCharacterZhouYuLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ApplyGuYinInitialHandSize(global::CardGame.Core.CharacterState player, int ordinaryCount);
}

internal interface ICoreInitialHandModifiersModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface ICoreInitialHandModifiersCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RuleQueryContribution> CollectNumericRuleContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query, global::CardGame.Core.CardKind? effectiveCardKind = default);
}
