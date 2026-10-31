#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDamagePreventionProgramsCoreProgramRuntimeHooksOperations : IProgramRuntimeOperations
{
    ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreDamagePreventionProgramsProgramWindowCursorOperations : IProgramRuntimeOperations
{
    ProgramWindowCursorRuntime Get();
}
