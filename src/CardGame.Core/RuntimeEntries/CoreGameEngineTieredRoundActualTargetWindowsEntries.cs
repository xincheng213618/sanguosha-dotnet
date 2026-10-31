#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundActualTargetWindowsRuntime))]
internal sealed partial class CoreGameEngineTieredRoundActualTargetWindowsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundActualTargetWindowsRuntime> _resolve;
    private CoreGameEngineTieredRoundActualTargetWindowsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundActualTargetWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTieredRoundActualTargetWindowsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundActualTargetWindowsRuntime> resolve) => new(resolve);
}
