#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLiYiNativeQueriesRuntime))]
internal sealed partial class CoreGameEngineLiYiNativeQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiYiNativeQueriesRuntime> _resolve;
    private CoreGameEngineLiYiNativeQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiYiNativeQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLiYiNativeQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiYiNativeQueriesRuntime> resolve) => new(resolve);
}
