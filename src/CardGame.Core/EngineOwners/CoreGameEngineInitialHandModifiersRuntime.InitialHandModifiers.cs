#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineInitialHandModifiersRuntime
{
    internal int GetInitialHandSize(CharacterState player) => _characterZhouYuLeOperations.ApplyGuYinInitialHandSize(player, CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(EvaluateInitialHandSize(player)));
    internal RuleQueryEvaluation EvaluateInitialHandSize(CharacterState player) => RuleQueryService.Evaluate(SkillRuleQuery.InitialHandSize, new RuleQueryBounds(0, int.MaxValue), [new RuleQueryBaseTerm($"mode:{(_coreMatchDefinitionInitialHandSizeModeDefinitionAccess._modeDefinition.Id)}:initial-hand-size", _modeRuntimeOperations._modeRuntime.InitialHandSizeOverride(player.Seat) ?? _modeRulesOperations._modeRules.InitialHandSizeOverride ?? _coreMatchDefinitionInitialHandSizeModeDefinitionAccess._initialHandSize)], _coreRuleQueriesOperations.CollectNumericRuleContributions(player, SkillRuleQuery.InitialHandSize));
}
