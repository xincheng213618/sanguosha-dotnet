#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCappedHandRefreshObserverEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedHandRefreshObserverRuntime> _resolve;
    private CoreGameEngineCappedHandRefreshObserverEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedHandRefreshObserverRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCappedHandRefreshObserverEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedHandRefreshObserverRuntime> resolve) => new(resolve);
}
