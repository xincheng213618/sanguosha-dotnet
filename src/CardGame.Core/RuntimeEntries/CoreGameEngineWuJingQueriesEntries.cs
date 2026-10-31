#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWuJingQueriesRuntime))]
internal sealed partial class CoreGameEngineWuJingQueriesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWuJingQueriesRuntime> _resolve;
    private CoreGameEngineWuJingQueriesEntries(Func<EngineOwners.CoreGameEngineWuJingQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWuJingQueriesEntries Create(Func<EngineOwners.CoreGameEngineWuJingQueriesRuntime> resolve) => new(resolve);
}
