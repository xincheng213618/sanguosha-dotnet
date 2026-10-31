#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterJiangWeiMouMouJiangWeiAnniversaryOperations : IProgramRuntimeOperations
{
    MouJiangWeiAnniversaryRuntime Get();
}

internal interface ICharacterJiangWeiMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
