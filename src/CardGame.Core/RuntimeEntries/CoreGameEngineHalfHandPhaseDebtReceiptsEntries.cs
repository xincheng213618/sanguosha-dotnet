#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime))]
internal sealed partial class CoreGameEngineHalfHandPhaseDebtReceiptsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime> _resolve;
    private CoreGameEngineHalfHandPhaseDebtReceiptsEntries(Func<EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHalfHandPhaseDebtReceiptsEntries Create(Func<EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime> resolve) => new(resolve);
}
