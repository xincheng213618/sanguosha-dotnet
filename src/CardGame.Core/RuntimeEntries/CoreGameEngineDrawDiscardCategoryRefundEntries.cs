#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawDiscardCategoryRefundEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawDiscardCategoryRefundRuntime> _resolve;
    private CoreGameEngineDrawDiscardCategoryRefundEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawDiscardCategoryRefundRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawDiscardCategoryRefundEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawDiscardCategoryRefundRuntime> resolve) => new(resolve);
}
