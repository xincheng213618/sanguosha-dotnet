#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandPhaseDebtProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> _resolve;
    private CoreGameEngineHalfHandPhaseDebtProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHalfHandPhaseDebtProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> resolve) => new(resolve);
}
