#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime))]
internal sealed partial class CoreGameEngineHalfHandPhaseDebtDamageObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> _resolve;
    private CoreGameEngineHalfHandPhaseDebtDamageObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHalfHandPhaseDebtDamageObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtDamageObserversRuntime> resolve) => new(resolve);
}
