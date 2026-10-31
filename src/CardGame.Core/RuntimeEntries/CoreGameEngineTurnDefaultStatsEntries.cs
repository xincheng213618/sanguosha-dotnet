#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDefaultStatsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDefaultStatsRuntime> _resolve;
    private CoreGameEngineTurnDefaultStatsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDefaultStatsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnDefaultStatsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnDefaultStatsRuntime> resolve) => new(resolve);
}
