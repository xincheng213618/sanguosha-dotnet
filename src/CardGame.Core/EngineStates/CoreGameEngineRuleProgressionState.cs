#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineRuleProgressionState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess
{
    private global::CardGame.Core.RuleProgressionRuntime? _ruleProgression = default !;
    internal ref global::CardGame.Core.RuleProgressionRuntime? ReferenceToRuleProgression => ref _ruleProgression;

    ref global::CardGame.Core.RuleProgressionRuntime? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess._ruleProgression => ref _ruleProgression;
}
