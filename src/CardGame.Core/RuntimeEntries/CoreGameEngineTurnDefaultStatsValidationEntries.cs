#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnDefaultStatsValidationRuntime))]
internal sealed partial class CoreGameEngineTurnDefaultStatsValidationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDefaultStatsValidationRuntime> _resolve;
    private CoreGameEngineTurnDefaultStatsValidationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDefaultStatsValidationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnDefaultStatsValidationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDefaultStatsValidationRuntime> resolve) => new(resolve);
}
