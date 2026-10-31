#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePaidOwnTargetObserversEntries
{
    internal global::CardGame.Core.ProgramSkillFrame? ActualUseTargetObserverRoot(long useId) => _resolve().ActualUseTargetObserverRoot(useId);
    internal void ApplyPaidOwnTarget(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ApplyPaidOwnTarget(frame);
    internal void AssertActualUseTargetPrograms() => _resolve().AssertActualUseTargetPrograms();
    internal void FinishPaidOwnTargetBeforeProgramCompletion(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishPaidOwnTargetBeforeProgramCompletion(frame);
    internal bool HasActualUseTargetObserver(long useId) => _resolve().HasActualUseTargetObserver(useId);
    internal bool IsPaidOwnTargetProgramDying() => _resolve().IsPaidOwnTargetProgramDying();
    internal bool PaidOwnTargetMatches(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ActualUseTargetWindowFrame parent, bool requireCost) => _resolve().PaidOwnTargetMatches(root, parent, requireCost);
    internal global::CardGame.Core.SkillProgramStepOutcome PayOwnActualUseTarget(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PayOwnActualUseTarget(frame);
}
