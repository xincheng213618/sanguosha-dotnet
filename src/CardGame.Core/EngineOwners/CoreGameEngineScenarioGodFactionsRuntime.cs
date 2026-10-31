#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineScenarioGodFactionsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineModeRulesEntries _coreModeRulesOperations;
    internal CoreGameEngineScenarioGodFactionsRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineModeRulesEntries coreModeRulesOperations)
    {
        this._coreModeRulesOperations = coreModeRulesOperations;
    }
}
