#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreHpDamageShieldsCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
