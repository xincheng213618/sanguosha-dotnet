#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketApplicationRuntime))]
internal sealed partial class CoreGameEngineShanheMarketApplicationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketApplicationRuntime> _resolve;
    private CoreGameEngineShanheMarketApplicationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketApplicationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheMarketApplicationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketApplicationRuntime> resolve) => new(resolve);
}
