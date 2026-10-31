#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime))]
internal sealed partial class CoreGameEngineCuiYanNativeQueriesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> _resolve;
    private CoreGameEngineCuiYanNativeQueriesEntries(Func<EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCuiYanNativeQueriesEntries Create(Func<EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> resolve) => new(resolve);
}
