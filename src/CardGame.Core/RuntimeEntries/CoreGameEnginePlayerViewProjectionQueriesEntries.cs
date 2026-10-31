#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePlayerViewProjectionQueriesRuntime))]
internal sealed partial class CoreGameEnginePlayerViewProjectionQueriesEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePlayerViewProjectionQueriesRuntime> _resolve;
    private CoreGameEnginePlayerViewProjectionQueriesEntries(Func<EngineOwners.CoreGameEnginePlayerViewProjectionQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePlayerViewProjectionQueriesEntries Create(Func<EngineOwners.CoreGameEnginePlayerViewProjectionQueriesRuntime> resolve) => new(resolve);
}
