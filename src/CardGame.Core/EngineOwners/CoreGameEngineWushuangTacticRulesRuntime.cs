#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWushuangTacticRulesRuntime
{
    private readonly RuntimeEntries.CoreGameEngineWushuangEntries _coreWushuangOperations;
    internal CoreGameEngineWushuangTacticRulesRuntime(RuntimeEntries.CoreGameEngineWushuangEntries coreWushuangOperations)
    {
        this._coreWushuangOperations = coreWushuangOperations;
    }
}
