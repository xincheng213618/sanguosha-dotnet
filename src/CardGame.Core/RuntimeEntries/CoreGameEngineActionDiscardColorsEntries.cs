#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActionDiscardColorsRuntime))]
internal sealed partial class CoreGameEngineActionDiscardColorsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActionDiscardColorsRuntime> _resolve;
    private CoreGameEngineActionDiscardColorsEntries(Func<EngineOwners.CoreGameEngineActionDiscardColorsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActionDiscardColorsEntries Create(Func<EngineOwners.CoreGameEngineActionDiscardColorsRuntime> resolve) => new(resolve);
}
