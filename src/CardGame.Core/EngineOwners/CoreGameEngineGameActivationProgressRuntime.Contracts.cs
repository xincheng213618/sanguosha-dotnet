#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreGameActivationProgressCoreProgramPhaseUsedProjectionsOperations : IProgramRuntimeOperations
{
    ProgramPhaseUsedProjections PhaseUsedProjections { get; }
}

internal interface ICoreGameActivationProgressCoreProgramSelectionModulesOperations : IProgramRuntimeOperations
{
    ProgramSelectionRegistry ProgramSelections { get; }
}
