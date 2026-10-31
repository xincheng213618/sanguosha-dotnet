#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime))]
internal sealed partial class CoreGameEngineNativeDrawThresholdDyingEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime> _resolve;
    private CoreGameEngineNativeDrawThresholdDyingEntries(Func<EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeDrawThresholdDyingEntries Create(Func<EngineOwners.CoreGameEngineNativeDrawThresholdDyingRuntime> resolve) => new(resolve);
}
