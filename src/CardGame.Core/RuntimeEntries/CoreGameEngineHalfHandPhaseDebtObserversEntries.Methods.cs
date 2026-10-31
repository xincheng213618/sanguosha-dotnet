#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandPhaseDebtObserversEntries
{
    internal ProgramSkillFrame? HalfHandPhaseDebtObserverRoot() => _resolve().HalfHandPhaseDebtObserverRoot();
    internal bool HalfHandPhaseFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HalfHandPhaseFirstChild(f, child);
    internal bool HalfHandSupportObserverRootMatches(ProgramSkillFrame f, ActualUseTargetWindowFrame parent) => _resolve().HalfHandSupportObserverRootMatches(f, parent);
    internal bool IsHalfHandPhaseDebtProgramDying() => _resolve().IsHalfHandPhaseDebtProgramDying();
}
