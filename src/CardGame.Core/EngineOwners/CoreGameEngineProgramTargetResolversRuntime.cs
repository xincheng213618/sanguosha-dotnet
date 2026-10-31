#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramTargetResolversRuntime
{
    private readonly IKernelProgramTargetResolversAccess _programTargetResolversOperations;
    private readonly ICoreProgramTargetResolversCoreProgramSelectionModulesOperations _coreProgramSelectionModulesOperations;
    internal CoreGameEngineProgramTargetResolversRuntime(IKernelProgramTargetResolversAccess programTargetResolversOperations, ICoreProgramTargetResolversCoreProgramSelectionModulesOperations coreProgramSelectionModulesOperations)
    {
        this._programTargetResolversOperations = programTargetResolversOperations;
        this._coreProgramSelectionModulesOperations = coreProgramSelectionModulesOperations;
    }
}
