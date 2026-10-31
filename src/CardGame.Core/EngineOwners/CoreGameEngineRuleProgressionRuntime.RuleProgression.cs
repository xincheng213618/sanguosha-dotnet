#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRuleProgressionRuntime
{
    internal RuleProgressionRuntime RuleProgression => _coreGameEngineRuleProgressionRuleProgressionAccess._ruleProgression ??= new(_programRuleObserversOperations._programRuleObservers, _kernelCoreGameEngineRuleProgressionOperations.CreateRuleProgressionOperations());
}
