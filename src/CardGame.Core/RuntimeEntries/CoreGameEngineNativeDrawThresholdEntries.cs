#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNativeDrawThresholdRuntime))]
internal sealed partial class CoreGameEngineNativeDrawThresholdEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> _resolve;
    private CoreGameEngineNativeDrawThresholdEntries(Func<EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeDrawThresholdEntries Create(Func<EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> resolve) => new(resolve);
}
