#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreAlternativePhaseCostProgramsRuntime))]
internal sealed partial class CoreAlternativePhaseCostProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreAlternativePhaseCostProgramsRuntime> _resolve;
    private CoreAlternativePhaseCostProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreAlternativePhaseCostProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreAlternativePhaseCostProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreAlternativePhaseCostProgramsRuntime> resolve) => new(resolve);
}
