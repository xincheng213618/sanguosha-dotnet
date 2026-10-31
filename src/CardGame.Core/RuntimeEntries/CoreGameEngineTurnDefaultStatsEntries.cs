#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTurnDefaultStatsRuntime))]
internal sealed partial class CoreGameEngineTurnDefaultStatsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTurnDefaultStatsRuntime> _resolve;
    private CoreGameEngineTurnDefaultStatsEntries(Func<EngineOwners.CoreGameEngineTurnDefaultStatsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnDefaultStatsEntries Create(Func<EngineOwners.CoreGameEngineTurnDefaultStatsRuntime> resolve) => new(resolve);
}
