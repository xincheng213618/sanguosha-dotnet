#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeDrawThresholdDyingEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime> _resolve;
    private CoreGameEngineNativeDrawThresholdDyingEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeDrawThresholdDyingEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime> resolve) => new(resolve);
}
