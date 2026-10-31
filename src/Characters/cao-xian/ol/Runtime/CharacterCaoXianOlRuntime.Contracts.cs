#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCaoXianOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterCaoXianOlCaoXianOperations : IProgramRuntimeOperations
{
    CaoXianRuntime Get();
}
