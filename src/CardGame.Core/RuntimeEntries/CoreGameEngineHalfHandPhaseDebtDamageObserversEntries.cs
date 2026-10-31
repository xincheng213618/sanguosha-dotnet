#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime))]
internal sealed partial class CoreGameEngineHalfHandPhaseDebtDamageObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> _resolve;
    private CoreGameEngineHalfHandPhaseDebtDamageObserversEntries(Func<EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHalfHandPhaseDebtDamageObserversEntries Create(Func<EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> resolve) => new(resolve);
}
