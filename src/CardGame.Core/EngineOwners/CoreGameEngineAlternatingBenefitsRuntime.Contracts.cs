#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreAlternatingBenefitsCoreProgramEventHistoryOperations : IProgramRuntimeOperations
{
    ProgramEventHistoryIndex ProgramHistory { get; }

    IReadOnlyList<T> ProgramEventHistory<T>()
        where T : IGameEvent;
}
