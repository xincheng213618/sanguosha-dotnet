#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeferredCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredCardsRuntime> _resolve;
    private CoreGameEngineDeferredCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeferredCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredCardsRuntime> resolve) => new(resolve);
}
