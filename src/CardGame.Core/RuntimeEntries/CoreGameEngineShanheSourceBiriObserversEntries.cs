#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceBiriObserversRuntime))]
internal sealed partial class CoreGameEngineShanheSourceBiriObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceBiriObserversRuntime> _resolve;
    private CoreGameEngineShanheSourceBiriObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceBiriObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceBiriObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceBiriObserversRuntime> resolve) => new(resolve);
}
