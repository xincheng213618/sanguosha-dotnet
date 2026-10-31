#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFulueQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFulueQueriesRuntime> _resolve;
    private CoreGameEngineFulueQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFulueQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFulueQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFulueQueriesRuntime> resolve) => new(resolve);
}
