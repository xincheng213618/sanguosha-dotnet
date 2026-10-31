#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterSimaYiMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterSimaYiMouMouSimaYiOperations : IProgramRuntimeOperations
{
    MouSimaYiRuntime Get();
}
