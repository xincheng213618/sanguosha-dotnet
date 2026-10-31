#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandPhaseDebtObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtObserversRuntime> _resolve;
    private CoreGameEngineHalfHandPhaseDebtObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHalfHandPhaseDebtObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtObserversRuntime> resolve) => new(resolve);
}
