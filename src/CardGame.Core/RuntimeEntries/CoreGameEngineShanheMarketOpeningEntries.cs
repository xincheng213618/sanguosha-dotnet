#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheMarketOpeningRuntime))]
internal sealed partial class CoreGameEngineShanheMarketOpeningEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheMarketOpeningRuntime> _resolve;
    private CoreGameEngineShanheMarketOpeningEntries(Func<EngineOwners.CoreGameEngineShanheMarketOpeningRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheMarketOpeningEntries Create(Func<EngineOwners.CoreGameEngineShanheMarketOpeningRuntime> resolve) => new(resolve);
}
