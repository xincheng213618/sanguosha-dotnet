#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime))]
internal sealed partial class CoreGameEngineHalfHandPhaseDebtProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> _resolve;
    private CoreGameEngineHalfHandPhaseDebtProgramsEntries(Func<EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHalfHandPhaseDebtProgramsEntries Create(Func<EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> resolve) => new(resolve);
}
