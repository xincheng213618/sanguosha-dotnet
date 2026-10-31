#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOffTurnUsedCardObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> _resolve;
    private CoreGameEngineOffTurnUsedCardObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOffTurnUsedCardObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardObserversRuntime> resolve) => new(resolve);
}
