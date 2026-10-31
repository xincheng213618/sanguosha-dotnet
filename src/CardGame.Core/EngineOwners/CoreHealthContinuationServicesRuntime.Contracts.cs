#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreHealthContinuationServicesCoreProgramRuntimeHooksOperations : IProgramRuntimeOperations
{
    ProgramRuntimeHooks RuntimeHooks { get; }
}
