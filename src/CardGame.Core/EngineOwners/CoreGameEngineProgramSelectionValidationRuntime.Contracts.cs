#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramSelectionValidationCoreProgramSelectionModulesOperations : IProgramRuntimeOperations
{
    IProgramSelectionQueries ProgramSelectionQueries { get; }
}
