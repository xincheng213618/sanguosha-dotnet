#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramContinuationsRuntime
{
    private readonly ICoreProgramContinuationsProgramChildWindowOperations _programChildWindowOperations;
    internal CoreGameEngineProgramContinuationsRuntime(ICoreProgramContinuationsProgramChildWindowOperations programChildWindowOperations)
    {
        this._programChildWindowOperations = programChildWindowOperations;
    }
}
