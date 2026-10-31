#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeDrawThresholdEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> _resolve;
    private CoreGameEngineNativeDrawThresholdEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeDrawThresholdEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> resolve) => new(resolve);
}
