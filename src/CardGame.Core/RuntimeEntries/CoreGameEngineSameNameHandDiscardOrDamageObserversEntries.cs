#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime))]
internal sealed partial class CoreGameEngineSameNameHandDiscardOrDamageObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime> _resolve;
    private CoreGameEngineSameNameHandDiscardOrDamageObserversEntries(Func<EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSameNameHandDiscardOrDamageObserversEntries Create(Func<EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime> resolve) => new(resolve);
}
