#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoYuOlRuntime
{
    internal bool XiangzuSpent(int owner) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoYuXiangzuSpentEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == "ol:xiangzu");
    internal bool CaoYuDyingEntry(ProgramSkillFrame f, out DyingFrame dying)
    {
        dying = null !;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 2 || _resolutionStackOperations._resolutionStack[i - 1] is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DyingEntering, Continuation: ProgramLifecycleContinuation.ResumeDyingEntry } w || _resolutionStackOperations._resolutionStack[i - 2] is not DyingFrame d || w.ResumeDyingFrameId != d.Id || d.VictimSeat != f.OwnerSeat || w.OwnerSeat != f.OwnerSeat || f.WindowContext is not { Window: SkillProgramTriggerWindow.DyingEntering } context || context.ParentFrameId != w.Id || context.TargetSeat != f.OwnerSeat || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]))
            return false;
        dying = d;
        return true;
    }
}
