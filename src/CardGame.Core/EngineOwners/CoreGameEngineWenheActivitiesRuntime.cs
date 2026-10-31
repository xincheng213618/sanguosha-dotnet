#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWenheActivitiesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityBattleRulesEntries _coreActivityBattleRulesOperations;
    internal CoreGameEngineWenheActivitiesRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityBattleRulesEntries coreActivityBattleRulesOperations)
    {
        this._coreActivityBattleRulesOperations = coreActivityBattleRulesOperations;
    }
}
