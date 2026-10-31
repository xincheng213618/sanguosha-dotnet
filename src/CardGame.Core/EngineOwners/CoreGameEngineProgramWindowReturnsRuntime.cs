#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramWindowReturnsRuntime
{
    private readonly ICoreProgramWindowReturnsProgramWindowReturnOperations _programWindowReturnOperations;
    internal CoreGameEngineProgramWindowReturnsRuntime(ICoreProgramWindowReturnsProgramWindowReturnOperations programWindowReturnOperations)
    {
        this._programWindowReturnOperations = programWindowReturnOperations;
    }
}
