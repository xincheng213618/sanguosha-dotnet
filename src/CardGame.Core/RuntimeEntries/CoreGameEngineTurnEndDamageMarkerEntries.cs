#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnEndDamageMarkerEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTurnEndDamageMarkerRuntime> _resolve;
    private CoreGameEngineTurnEndDamageMarkerEntries(Func<EngineOwners.CoreGameEngineTurnEndDamageMarkerRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnEndDamageMarkerEntries Create(Func<EngineOwners.CoreGameEngineTurnEndDamageMarkerRuntime> resolve) => new(resolve);
}
