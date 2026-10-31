#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterBuZhiOlCoreProgramSelectionModulesOperations : IProgramRuntimeOperations
{
    IProgramSelectionQueries ProgramSelectionQueries { get; }
}

internal interface ICharacterBuZhiOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
