#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDamageAdjacencyServicesCoreProgramRuntimeHooksOperations : IProgramRuntimeOperations
{
    ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreDamageAdjacencyServicesDamageAdjacencyOperations : IProgramRuntimeOperations
{
    DamageAdjacencyRuntime Get();
}
