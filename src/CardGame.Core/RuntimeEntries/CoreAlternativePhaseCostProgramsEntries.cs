#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreAlternativePhaseCostProgramsRuntime))]
internal sealed partial class CoreAlternativePhaseCostProgramsEntries
{
    private readonly Func<EngineOwners.CoreAlternativePhaseCostProgramsRuntime> _resolve;
    private CoreAlternativePhaseCostProgramsEntries(Func<EngineOwners.CoreAlternativePhaseCostProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreAlternativePhaseCostProgramsEntries Create(Func<EngineOwners.CoreAlternativePhaseCostProgramsRuntime> resolve) => new(resolve);
}
