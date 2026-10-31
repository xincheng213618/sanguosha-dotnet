#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterSimaShiMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterSimaShiMouMouSimaShiOperations : IProgramRuntimeOperations
{
    MouSimaShiRuntime Get();
}
