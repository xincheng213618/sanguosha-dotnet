#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramSelectionModulesRuntime
{
    private readonly IKernelProgramSelectionQueriesAccess _programSelectionQueriesOperations;
    private readonly IKernelProgramSelectionsAccess _programSelectionsOperations;
    private readonly IKernelCreateSelectionQueriesOperations _kernelCoreProgramSelectionModulesOperations;
    internal CoreProgramSelectionModulesRuntime(IKernelProgramSelectionQueriesAccess programSelectionQueriesOperations, IKernelProgramSelectionsAccess programSelectionsOperations, IKernelCreateSelectionQueriesOperations kernelCoreProgramSelectionModulesOperations)
    {
        this._programSelectionQueriesOperations = programSelectionQueriesOperations;
        this._programSelectionsOperations = programSelectionsOperations;
        this._kernelCoreProgramSelectionModulesOperations = kernelCoreProgramSelectionModulesOperations;
    }
}
