#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCaoShuangMouMouCaoShuangAnniversaryOperations : IProgramRuntimeOperations
{
    MouCaoShuangAnniversaryRuntime Get();
}

internal interface ICharacterCaoShuangMouCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
