#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDrawDiscardCategoryRefundRuntime))]
internal sealed partial class CoreGameEngineDrawDiscardCategoryRefundEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDrawDiscardCategoryRefundRuntime> _resolve;
    private CoreGameEngineDrawDiscardCategoryRefundEntries(Func<EngineOwners.CoreGameEngineDrawDiscardCategoryRefundRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawDiscardCategoryRefundEntries Create(Func<EngineOwners.CoreGameEngineDrawDiscardCategoryRefundRuntime> resolve) => new(resolve);
}
