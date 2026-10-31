#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketRuntime))]
internal sealed partial class CoreGameEngineShanheMarketEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketRuntime> _resolve;
    private CoreGameEngineShanheMarketEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheMarketEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheMarketRuntime> resolve) => new(resolve);
}
