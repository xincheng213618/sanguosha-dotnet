#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreModeNativeFrameRuntimeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreModeNativeFrameRuntimeRuntime> _resolve;
    private CoreModeNativeFrameRuntimeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreModeNativeFrameRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreModeNativeFrameRuntimeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreModeNativeFrameRuntimeRuntime> resolve) => new(resolve);
}
