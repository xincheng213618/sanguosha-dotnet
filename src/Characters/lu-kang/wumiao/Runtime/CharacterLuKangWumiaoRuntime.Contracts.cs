#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterLuKangWumiaoProgramTargetSelectionOperations : IProgramRuntimeOperations
{
    ProgramTargetSelectionRuntime Get();
}

internal interface ICharacterLuKangWumiaoCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
