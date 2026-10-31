#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHalfHandPhaseDebtObserversRuntime))]
internal sealed partial class CoreGameEngineHalfHandPhaseDebtObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHalfHandPhaseDebtObserversRuntime> _resolve;
    private CoreGameEngineHalfHandPhaseDebtObserversEntries(Func<EngineOwners.CoreGameEngineHalfHandPhaseDebtObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHalfHandPhaseDebtObserversEntries Create(Func<EngineOwners.CoreGameEngineHalfHandPhaseDebtObserversRuntime> resolve) => new(resolve);
}
