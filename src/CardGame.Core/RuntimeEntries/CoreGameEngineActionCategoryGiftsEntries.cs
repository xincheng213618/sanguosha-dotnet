#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime))]
internal sealed partial class CoreGameEngineActionCategoryGiftsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> _resolve;
    private CoreGameEngineActionCategoryGiftsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActionCategoryGiftsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> resolve) => new(resolve);
}
