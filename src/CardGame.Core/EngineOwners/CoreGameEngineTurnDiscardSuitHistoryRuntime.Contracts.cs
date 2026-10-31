#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreTurnDiscardSuitHistoryProgramTriggerOperations : IProgramRuntimeOperations
{
    ProgramTriggerRuntime Get();
}
