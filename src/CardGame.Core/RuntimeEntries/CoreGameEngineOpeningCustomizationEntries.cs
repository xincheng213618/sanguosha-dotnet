#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOpeningCustomizationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOpeningCustomizationRuntime> _resolve;
    private CoreGameEngineOpeningCustomizationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOpeningCustomizationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOpeningCustomizationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOpeningCustomizationRuntime> resolve) => new(resolve);
}
