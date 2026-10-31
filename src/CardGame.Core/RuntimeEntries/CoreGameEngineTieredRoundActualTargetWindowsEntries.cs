#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTieredRoundActualTargetWindowsRuntime))]
internal sealed partial class CoreGameEngineTieredRoundActualTargetWindowsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTieredRoundActualTargetWindowsRuntime> _resolve;
    private CoreGameEngineTieredRoundActualTargetWindowsEntries(Func<EngineOwners.CoreGameEngineTieredRoundActualTargetWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTieredRoundActualTargetWindowsEntries Create(Func<EngineOwners.CoreGameEngineTieredRoundActualTargetWindowsRuntime> resolve) => new(resolve);
}
