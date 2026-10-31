#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterQingheGongzhuOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
