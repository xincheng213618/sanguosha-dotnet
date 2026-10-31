#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheMarketRuntime))]
internal sealed partial class CoreGameEngineShanheMarketEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheMarketRuntime> _resolve;
    private CoreGameEngineShanheMarketEntries(Func<EngineOwners.CoreGameEngineShanheMarketRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheMarketEntries Create(Func<EngineOwners.CoreGameEngineShanheMarketRuntime> resolve) => new(resolve);
}
