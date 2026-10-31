#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterLuSuMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterLuSuMouMouLuSuAnniversaryOperations : IProgramRuntimeOperations
{
    MouLuSuAnniversaryRuntime Get();
}
