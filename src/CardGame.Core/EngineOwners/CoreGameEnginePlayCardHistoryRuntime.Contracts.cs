#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePlayCardHistoryCoreProgramEventHistoryOperations : IProgramRuntimeOperations
{
    ProgramEventHistoryIndex ProgramHistory { get; }
}
