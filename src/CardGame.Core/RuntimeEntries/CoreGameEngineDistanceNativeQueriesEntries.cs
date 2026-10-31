#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDistanceNativeQueriesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDistanceNativeQueriesRuntime> _resolve;
    private CoreGameEngineDistanceNativeQueriesEntries(Func<EngineOwners.CoreGameEngineDistanceNativeQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDistanceNativeQueriesEntries Create(Func<EngineOwners.CoreGameEngineDistanceNativeQueriesRuntime> resolve) => new(resolve);
}
