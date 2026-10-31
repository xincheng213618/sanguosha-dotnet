#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConditionalDiscardDuelObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelObserversRuntime> _resolve;
    private CoreGameEngineConditionalDiscardDuelObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConditionalDiscardDuelObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelObserversRuntime> resolve) => new(resolve);
}
