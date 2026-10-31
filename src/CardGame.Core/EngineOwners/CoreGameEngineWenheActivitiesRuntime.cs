#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWenheActivitiesRuntime
{
    private readonly ICoreWenheActivitiesCoreActivityBattleRulesOperations _coreActivityBattleRulesOperations;
    internal CoreGameEngineWenheActivitiesRuntime(ICoreWenheActivitiesCoreActivityBattleRulesOperations coreActivityBattleRulesOperations)
    {
        this._coreActivityBattleRulesOperations = coreActivityBattleRulesOperations;
    }
}
