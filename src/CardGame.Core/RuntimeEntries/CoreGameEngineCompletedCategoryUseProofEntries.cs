#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime))]
internal sealed partial class CoreGameEngineCompletedCategoryUseProofEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime> _resolve;
    private CoreGameEngineCompletedCategoryUseProofEntries(Func<EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedCategoryUseProofEntries Create(Func<EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime> resolve) => new(resolve);
}
