#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMarketApplicationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketApplicationRuntime> _resolve;
    private CoreGameEngineShanheMarketApplicationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketApplicationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheMarketApplicationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketApplicationRuntime> resolve) => new(resolve);
}
