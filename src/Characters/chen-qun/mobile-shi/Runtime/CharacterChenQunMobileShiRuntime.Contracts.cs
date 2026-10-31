#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterChenQunMobileShiCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
