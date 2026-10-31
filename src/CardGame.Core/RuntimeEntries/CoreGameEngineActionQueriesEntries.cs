#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActionQueriesRuntime))]
internal sealed partial class CoreGameEngineActionQueriesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActionQueriesRuntime> _resolve;
    private CoreGameEngineActionQueriesEntries(Func<EngineOwners.CoreGameEngineActionQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActionQueriesEntries Create(Func<EngineOwners.CoreGameEngineActionQueriesRuntime> resolve) => new(resolve);
}
