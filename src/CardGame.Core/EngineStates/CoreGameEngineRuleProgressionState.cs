#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineRuleProgressionState : Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess
{
    private RuleProgressionRuntime? _ruleProgression = default !;
    internal ref RuleProgressionRuntime? ReferenceToRuleProgression => ref _ruleProgression;

    ref RuleProgressionRuntime? Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess._ruleProgression => ref _ruleProgression;
}
