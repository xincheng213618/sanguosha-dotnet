#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlayerViewProjectionQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayerViewProjectionQueriesRuntime> _resolve;
    private CoreGameEnginePlayerViewProjectionQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayerViewProjectionQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlayerViewProjectionQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayerViewProjectionQueriesRuntime> resolve) => new(resolve);
}
