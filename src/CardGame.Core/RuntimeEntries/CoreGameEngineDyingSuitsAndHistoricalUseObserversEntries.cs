#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime))]
internal sealed partial class CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> _resolve;
    private CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries(Func<EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries Create(Func<EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> resolve) => new(resolve);
}
