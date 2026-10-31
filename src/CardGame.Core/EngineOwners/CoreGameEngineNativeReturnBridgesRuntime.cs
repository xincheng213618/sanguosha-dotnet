#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeReturnBridgesRuntime
{
    private readonly RuntimeEntries.CoreGameEngineQinglongInsertionEntries _coreQinglongInsertionOperations;
    private readonly RuntimeEntries.CoreNativeDyingEntries _coreNativeDyingOperations;
    internal CoreGameEngineNativeReturnBridgesRuntime(RuntimeEntries.CoreGameEngineQinglongInsertionEntries coreQinglongInsertionOperations, RuntimeEntries.CoreNativeDyingEntries coreNativeDyingOperations)
    {
        this._coreQinglongInsertionOperations = coreQinglongInsertionOperations;
        this._coreNativeDyingOperations = coreNativeDyingOperations;
    }
}
