#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTurnEndingOrderRuntime
{
    private readonly ICoreTurnEndingOrderProgramPhaseBoundaryOperations _programPhaseBoundaryOperations;
    internal CoreGameEngineTurnEndingOrderRuntime(ICoreTurnEndingOrderProgramPhaseBoundaryOperations programPhaseBoundaryOperations)
    {
        this._programPhaseBoundaryOperations = programPhaseBoundaryOperations;
    }
}
