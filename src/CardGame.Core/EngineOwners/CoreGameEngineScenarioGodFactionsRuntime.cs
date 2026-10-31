#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineScenarioGodFactionsRuntime
{
    private readonly ICoreScenarioGodFactionsCoreModeRulesOperations _coreModeRulesOperations;
    internal CoreGameEngineScenarioGodFactionsRuntime(ICoreScenarioGodFactionsCoreModeRulesOperations coreModeRulesOperations)
    {
        this._coreModeRulesOperations = coreModeRulesOperations;
    }
}
