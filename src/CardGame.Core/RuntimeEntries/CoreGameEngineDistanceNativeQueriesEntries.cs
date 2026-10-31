#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDistanceNativeQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDistanceNativeQueriesRuntime> _resolve;
    private CoreGameEngineDistanceNativeQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDistanceNativeQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDistanceNativeQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDistanceNativeQueriesRuntime> resolve) => new(resolve);
}
