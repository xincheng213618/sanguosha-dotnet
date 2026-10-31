#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineScenarioGodFactionsRuntime
{
    private readonly RuntimeEntries.CoreGameEngineModeRulesEntries _coreModeRulesOperations;
    internal CoreGameEngineScenarioGodFactionsRuntime(RuntimeEntries.CoreGameEngineModeRulesEntries coreModeRulesOperations)
    {
        this._coreModeRulesOperations = coreModeRulesOperations;
    }
}
