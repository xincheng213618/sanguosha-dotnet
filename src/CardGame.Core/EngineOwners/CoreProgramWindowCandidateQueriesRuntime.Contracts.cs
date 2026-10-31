#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramWindowCandidateQueriesProgramTriggerOperations : IProgramRuntimeOperations
{
    ProgramTriggerRuntime Get();
}

internal interface ICoreProgramWindowCandidateQueriesProgramTriggerFactProjectorOperations : IProgramRuntimeOperations
{
    ProgramTriggerFactProjector Get();
}
