#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreTurnEndingOrderProgramPhaseBoundaryOperations : IProgramRuntimeOperations
{
    ProgramPhaseBoundaryRuntime Get();
}
