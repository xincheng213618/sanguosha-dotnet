#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime))]
internal sealed partial class CoreGameEngineHalfHandPhaseDebtReceiptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime> _resolve;
    private CoreGameEngineHalfHandPhaseDebtReceiptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHalfHandPhaseDebtReceiptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtReceiptsRuntime> resolve) => new(resolve);
}
