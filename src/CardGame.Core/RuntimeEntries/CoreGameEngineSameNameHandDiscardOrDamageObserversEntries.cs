#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSameNameHandDiscardOrDamageObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime> _resolve;
    private CoreGameEngineSameNameHandDiscardOrDamageObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSameNameHandDiscardOrDamageObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageObserversRuntime> resolve) => new(resolve);
}
