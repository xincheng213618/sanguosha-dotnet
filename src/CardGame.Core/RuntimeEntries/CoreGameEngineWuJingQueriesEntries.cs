#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWuJingQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWuJingQueriesRuntime> _resolve;
    private CoreGameEngineWuJingQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWuJingQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWuJingQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWuJingQueriesRuntime> resolve) => new(resolve);
}
