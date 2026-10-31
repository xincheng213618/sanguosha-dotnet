#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreCommandSessionQueriesEntries
{
    private readonly Func<EngineOwners.CoreCommandSessionQueriesRuntime> _resolve;
    private CoreCommandSessionQueriesEntries(Func<EngineOwners.CoreCommandSessionQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreCommandSessionQueriesEntries Create(Func<EngineOwners.CoreCommandSessionQueriesRuntime> resolve) => new(resolve);
}
