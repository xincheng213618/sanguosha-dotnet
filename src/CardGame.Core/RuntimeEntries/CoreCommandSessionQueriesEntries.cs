#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreCommandSessionQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreCommandSessionQueriesRuntime> _resolve;
    private CoreCommandSessionQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreCommandSessionQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreCommandSessionQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreCommandSessionQueriesRuntime> resolve) => new(resolve);
}
