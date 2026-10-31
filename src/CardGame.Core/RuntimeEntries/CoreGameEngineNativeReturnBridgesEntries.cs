#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeReturnBridgesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativeReturnBridgesRuntime> _resolve;
    private CoreGameEngineNativeReturnBridgesEntries(Func<EngineOwners.CoreGameEngineNativeReturnBridgesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeReturnBridgesEntries Create(Func<EngineOwners.CoreGameEngineNativeReturnBridgesRuntime> resolve) => new(resolve);
}
