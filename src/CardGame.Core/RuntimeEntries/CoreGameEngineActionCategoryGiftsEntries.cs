#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActionCategoryGiftsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> _resolve;
    private CoreGameEngineActionCategoryGiftsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActionCategoryGiftsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionCategoryGiftsRuntime> resolve) => new(resolve);
}
