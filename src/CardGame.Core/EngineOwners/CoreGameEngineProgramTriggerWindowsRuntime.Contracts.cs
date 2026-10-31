#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramTriggerWindowsProgramPhaseBoundaryOperations : IProgramRuntimeOperations
{
    ProgramPhaseBoundaryRuntime Get();
}

internal interface ICoreProgramTriggerWindowsProgramLifecycleWindowOperations : IProgramRuntimeOperations
{
    ProgramLifecycleWindowRuntime Get();
}
