#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePaidOwnTargetObserversEntries
{
    internal ProgramSkillFrame? ActualUseTargetObserverRoot(long useId) => _resolve().ActualUseTargetObserverRoot(useId);
    internal void ApplyPaidOwnTarget(ProgramSkillFrame frame) => _resolve().ApplyPaidOwnTarget(frame);
    internal void FinishPaidOwnTargetBeforeProgramCompletion(ProgramSkillFrame frame) => _resolve().FinishPaidOwnTargetBeforeProgramCompletion(frame);
    internal bool IsPaidOwnTargetProgramDying() => _resolve().IsPaidOwnTargetProgramDying();
    internal bool PaidOwnTargetMatches(ProgramSkillFrame root, ActualUseTargetWindowFrame parent, bool requireCost) => _resolve().PaidOwnTargetMatches(root, parent, requireCost);
    internal SkillProgramStepOutcome PayOwnActualUseTarget(ProgramSkillFrame frame) => _resolve().PayOwnActualUseTarget(frame);
}
