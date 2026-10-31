#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramContinuationPermissionsRuntime
{
    private readonly ICoreProgramContinuationPermissionsCoreProgramRuntimeHooksOperations _coreProgramRuntimeHooksOperations;
    internal CoreGameEngineProgramContinuationPermissionsRuntime(ICoreProgramContinuationPermissionsCoreProgramRuntimeHooksOperations coreProgramRuntimeHooksOperations)
    {
        this._coreProgramRuntimeHooksOperations = coreProgramRuntimeHooksOperations;
    }
}
