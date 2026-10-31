#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreActualTurnsCoreProgramRuntimeHooksOperations : IProgramRuntimeOperations
{
    ProgramRuntimeHooks RuntimeHooks { get; }
}
