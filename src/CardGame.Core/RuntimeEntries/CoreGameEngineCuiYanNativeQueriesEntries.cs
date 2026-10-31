#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime))]
internal sealed partial class CoreGameEngineCuiYanNativeQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> _resolve;
    private CoreGameEngineCuiYanNativeQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCuiYanNativeQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> resolve) => new(resolve);
}
