#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreModeNativeFrameRuntimeEntries
{
    private readonly Func<EngineOwners.CoreModeNativeFrameRuntimeRuntime> _resolve;
    private CoreModeNativeFrameRuntimeEntries(Func<EngineOwners.CoreModeNativeFrameRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreModeNativeFrameRuntimeEntries Create(Func<EngineOwners.CoreModeNativeFrameRuntimeRuntime> resolve) => new(resolve);
}
