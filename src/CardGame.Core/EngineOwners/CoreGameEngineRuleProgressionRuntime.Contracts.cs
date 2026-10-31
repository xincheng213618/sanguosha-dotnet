#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRuleProgressionProgramRuleObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramRuleObservers _programRuleObservers { get; }
}

internal interface ICoreRuleProgressionKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.RuleProgressionOperations CreateRuleProgressionOperations();
}
