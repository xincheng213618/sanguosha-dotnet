#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnEndDamageMarkerEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnEndDamageMarkerRuntime> _resolve;
    private CoreGameEngineTurnEndDamageMarkerEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnEndDamageMarkerRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnEndDamageMarkerEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnEndDamageMarkerRuntime> resolve) => new(resolve);
}
