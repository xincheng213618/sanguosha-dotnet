#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHealthQueriesRuntime))]
internal sealed partial class CoreGameEngineHealthQueriesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHealthQueriesRuntime> _resolve;
    private CoreGameEngineHealthQueriesEntries(Func<EngineOwners.CoreGameEngineHealthQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHealthQueriesEntries Create(Func<EngineOwners.CoreGameEngineHealthQueriesRuntime> resolve) => new(resolve);
}
