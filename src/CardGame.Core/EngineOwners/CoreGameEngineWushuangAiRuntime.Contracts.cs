#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreWushuangAiCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreWushuangAiCoreWushuangModeProjectionOperations : IProgramRuntimeOperations
{
    WushuangSnapshot? CreateWushuangSnapshot(int viewerSeat);
}
