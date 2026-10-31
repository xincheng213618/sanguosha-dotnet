#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeReturnBridgesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeReturnBridgesRuntime> _resolve;
    private CoreGameEngineNativeReturnBridgesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeReturnBridgesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeReturnBridgesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeReturnBridgesRuntime> resolve) => new(resolve);
}
