#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime))]
internal sealed partial class CoreGameEngineTurnHandCategoryRestrictionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _resolve;
    private CoreGameEngineTurnHandCategoryRestrictionsEntries(Func<EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnHandCategoryRestrictionsEntries Create(Func<EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> resolve) => new(resolve);
}
