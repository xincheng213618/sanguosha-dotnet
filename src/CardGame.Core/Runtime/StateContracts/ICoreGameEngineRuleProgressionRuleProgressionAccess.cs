#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineRuleProgressionRuleProgressionAccess : IProgramRuntimeOperations
{
    ref RuleProgressionRuntime? _ruleProgression { get; }
}
