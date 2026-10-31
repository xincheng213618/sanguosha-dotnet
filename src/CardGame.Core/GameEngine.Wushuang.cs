namespace CardGame.Core;

public sealed partial class GameEngine
{
    public bool IsWushuangMode =>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime>().IsWushuangMode;
    public bool IsWushuangClockRunning =>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime>().IsWushuangClockRunning;
    public int? WushuangRemainingMilliseconds =>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime>().WushuangRemainingMilliseconds;
    public WushuangSnapshot? CreateWushuangSnapshot(int viewerSeat) =>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime>().CreateWushuangSnapshot(viewerSeat);
}
