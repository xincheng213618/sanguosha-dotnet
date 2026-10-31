#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConditionalDiscardDuelObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineConditionalDiscardDuelObserversRuntime> _resolve;
    private CoreGameEngineConditionalDiscardDuelObserversEntries(Func<EngineOwners.CoreGameEngineConditionalDiscardDuelObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConditionalDiscardDuelObserversEntries Create(Func<EngineOwners.CoreGameEngineConditionalDiscardDuelObserversRuntime> resolve) => new(resolve);
}
