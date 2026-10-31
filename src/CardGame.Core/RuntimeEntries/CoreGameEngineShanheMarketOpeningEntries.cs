#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMarketOpeningEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketOpeningRuntime> _resolve;
    private CoreGameEngineShanheMarketOpeningEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketOpeningRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheMarketOpeningEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketOpeningRuntime> resolve) => new(resolve);
}
