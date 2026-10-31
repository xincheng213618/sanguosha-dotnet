#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActionCategoryGiftsRuntime))]
internal sealed partial class CoreGameEngineActionCategoryGiftsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> _resolve;
    private CoreGameEngineActionCategoryGiftsEntries(Func<EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActionCategoryGiftsEntries Create(Func<EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> resolve) => new(resolve);
}
