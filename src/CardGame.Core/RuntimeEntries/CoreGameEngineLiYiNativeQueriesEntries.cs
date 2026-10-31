#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineLiYiNativeQueriesRuntime))]
internal sealed partial class CoreGameEngineLiYiNativeQueriesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLiYiNativeQueriesRuntime> _resolve;
    private CoreGameEngineLiYiNativeQueriesEntries(Func<EngineOwners.CoreGameEngineLiYiNativeQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLiYiNativeQueriesEntries Create(Func<EngineOwners.CoreGameEngineLiYiNativeQueriesRuntime> resolve) => new(resolve);
}
