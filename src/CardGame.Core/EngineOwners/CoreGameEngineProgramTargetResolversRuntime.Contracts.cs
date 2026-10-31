#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramTargetResolversCoreProgramSelectionModulesOperations : IProgramRuntimeOperations
{
    IProgramSelectionQueries ProgramSelectionQueries { get; }
}
