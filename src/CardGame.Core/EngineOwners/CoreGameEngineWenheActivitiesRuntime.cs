#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWenheActivitiesRuntime
{
    private readonly RuntimeEntries.CoreGameEngineActivityBattleRulesEntries _coreActivityBattleRulesOperations;
    internal CoreGameEngineWenheActivitiesRuntime(RuntimeEntries.CoreGameEngineActivityBattleRulesEntries coreActivityBattleRulesOperations)
    {
        this._coreActivityBattleRulesOperations = coreActivityBattleRulesOperations;
    }
}
