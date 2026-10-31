#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCaoPiMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
