#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLastDamageSourceReciprocityObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityObserversRuntime> _resolve;
    private CoreGameEngineLastDamageSourceReciprocityObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLastDamageSourceReciprocityObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityObserversRuntime> resolve) => new(resolve);
}
