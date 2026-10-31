#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheMarketApplicationRuntime))]
internal sealed partial class CoreGameEngineShanheMarketApplicationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheMarketApplicationRuntime> _resolve;
    private CoreGameEngineShanheMarketApplicationEntries(Func<EngineOwners.CoreGameEngineShanheMarketApplicationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheMarketApplicationEntries Create(Func<EngineOwners.CoreGameEngineShanheMarketApplicationRuntime> resolve) => new(resolve);
}
