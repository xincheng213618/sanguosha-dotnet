#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndRefillNextTurnObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndRefillNextTurnObserversRuntime> _resolve;
    private CoreGameEngineEndRefillNextTurnObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndRefillNextTurnObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEndRefillNextTurnObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndRefillNextTurnObserversRuntime> resolve) => new(resolve);
}
