#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandPhaseDebtObserversEntries
{
    internal global::CardGame.Core.ProgramSkillFrame? HalfHandPhaseDebtObserverRoot() => _resolve().HalfHandPhaseDebtObserverRoot();
    internal bool HalfHandPhaseFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HalfHandPhaseFirstChild(f, child);
    internal bool HalfHandSupportObserverRootMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ActualUseTargetWindowFrame parent) => _resolve().HalfHandSupportObserverRootMatches(f, parent);
    internal bool IsHalfHandPhaseDebtProgramDying() => _resolve().IsHalfHandPhaseDebtProgramDying();
}
