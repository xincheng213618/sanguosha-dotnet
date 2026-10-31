#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime))]
internal sealed partial class CoreGameEngineActionDiscardColorsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> _resolve;
    private CoreGameEngineActionDiscardColorsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActionDiscardColorsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> resolve) => new(resolve);
}
