#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterQinLangOlCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
