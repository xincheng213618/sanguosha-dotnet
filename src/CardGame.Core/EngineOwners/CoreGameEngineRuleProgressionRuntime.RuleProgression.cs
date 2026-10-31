#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRuleProgressionRuntime
{
    internal RuleProgressionRuntime RuleProgression => _coreGameEngineRuleProgressionRuleProgressionAccess._ruleProgression ??= new(_programRuleObserversOperations._programRuleObservers, _kernelCoreGameEngineRuleProgressionOperations.CreateRuleProgressionOperations());
}
