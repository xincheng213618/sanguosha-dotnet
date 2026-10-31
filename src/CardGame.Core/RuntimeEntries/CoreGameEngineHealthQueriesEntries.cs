#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHealthQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> _resolve;
    private CoreGameEngineHealthQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHealthQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> resolve) => new(resolve);
}
