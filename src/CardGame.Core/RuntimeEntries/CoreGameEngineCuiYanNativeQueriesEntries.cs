#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCuiYanNativeQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> _resolve;
    private CoreGameEngineCuiYanNativeQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCuiYanNativeQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> resolve) => new(resolve);
}
