#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreDamageObserverCursorQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime> _resolve;
    private CoreDamageObserverCursorQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDamageObserverCursorQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime> resolve) => new(resolve);
}
