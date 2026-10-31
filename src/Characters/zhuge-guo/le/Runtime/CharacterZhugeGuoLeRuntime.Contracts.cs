#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhugeGuoLeCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
