#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnHandCategoryRestrictionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _resolve;
    private CoreGameEngineTurnHandCategoryRestrictionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnHandCategoryRestrictionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> resolve) => new(resolve);
}
