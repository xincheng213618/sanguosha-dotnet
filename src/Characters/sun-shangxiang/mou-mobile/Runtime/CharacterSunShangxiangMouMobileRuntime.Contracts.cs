#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterSunShangxiangMouMobileMouSunShangxiangOperations : IProgramRuntimeOperations
{
    MouSunShangxiangRuntime Get();
}

internal interface ICharacterSunShangxiangMouMobileCoreWushuangModeProjectionOperations : IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}

internal interface ICharacterSunShangxiangMouMobileCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
