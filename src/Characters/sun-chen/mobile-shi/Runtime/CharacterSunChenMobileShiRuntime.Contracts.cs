#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterSunChenMobileShiCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
