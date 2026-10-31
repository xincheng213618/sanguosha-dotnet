#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime))]
internal sealed partial class CoreGameEngineNativeDrawThresholdEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> _resolve;
    private CoreGameEngineNativeDrawThresholdEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeDrawThresholdEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> resolve) => new(resolve);
}
