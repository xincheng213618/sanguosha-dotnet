#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeActionDispatchCoreProgramActionModulesOperations : IProgramRuntimeOperations
{
    ProgramActionRouter ProgramActions { get; }
}
