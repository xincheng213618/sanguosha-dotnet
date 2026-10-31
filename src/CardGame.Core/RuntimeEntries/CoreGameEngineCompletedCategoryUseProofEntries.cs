#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedCategoryUseProofEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime> _resolve;
    private CoreGameEngineCompletedCategoryUseProofEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedCategoryUseProofEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime> resolve) => new(resolve);
}
