#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTurnDefaultStatsValidationRuntime))]
internal sealed partial class CoreGameEngineTurnDefaultStatsValidationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTurnDefaultStatsValidationRuntime> _resolve;
    private CoreGameEngineTurnDefaultStatsValidationEntries(Func<EngineOwners.CoreGameEngineTurnDefaultStatsValidationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnDefaultStatsValidationEntries Create(Func<EngineOwners.CoreGameEngineTurnDefaultStatsValidationRuntime> resolve) => new(resolve);
}
