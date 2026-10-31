#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOpeningCustomizationRuntime))]
internal sealed partial class CoreGameEngineOpeningCustomizationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOpeningCustomizationRuntime> _resolve;
    private CoreGameEngineOpeningCustomizationEntries(Func<EngineOwners.CoreGameEngineOpeningCustomizationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOpeningCustomizationEntries Create(Func<EngineOwners.CoreGameEngineOpeningCustomizationRuntime> resolve) => new(resolve);
}
