#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> _resolve;
    private CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> resolve) => new(resolve);
}
