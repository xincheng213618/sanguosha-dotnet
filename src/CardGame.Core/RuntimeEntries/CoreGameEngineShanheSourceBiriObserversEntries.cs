#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceBiriObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceBiriObserversRuntime> _resolve;
    private CoreGameEngineShanheSourceBiriObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceBiriObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceBiriObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceBiriObserversRuntime> resolve) => new(resolve);
}
