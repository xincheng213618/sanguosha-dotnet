#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterKongRongMouOlMouKongRongOperations : IProgramRuntimeOperations
{
    MouKongRongRuntime Get();
}

internal interface ICharacterKongRongMouOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
