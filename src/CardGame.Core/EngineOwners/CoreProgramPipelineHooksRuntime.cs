#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramPipelineHooksRuntime
{
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    internal CoreProgramPipelineHooksRuntime(IKernelProgramPipelineHooksAccess programPipelineHooksOperations)
    {
        this._programPipelineHooksOperations = programPipelineHooksOperations;
    }
}
