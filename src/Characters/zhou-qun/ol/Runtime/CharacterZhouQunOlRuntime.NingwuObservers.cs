#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouQunOlRuntime
{
    internal bool IsExactNingwuFinalizedChildSubtree(ProgramJudgmentTriggerWindowFrame frame, JudgmentFrame? judgment, int index)
    {
        if (judgment is null || !frame.Activated || frame.ZhenyiBeforeResult == true || index < 2 || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index] != frame || _resolutionStackOperations._resolutionStack[index - 1] != judgment || _resolutionStackOperations._resolutionStack[index - 2] is not ProgramSkillFrame { ZhouQun: not null } root || !IsNingwuJudgment(judgment, root) || !_coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(judgment, frame) || frame.CandidateIndex < 0 || frame.CandidateIndex >= frame.Candidates.Count || _resolutionStackOperations._resolutionStack[index + 1] is not ProgramSkillFrame child || child.WindowContext is not { Window: SkillProgramTriggerWindow.JudgmentFinalized, Judgment: { } final } c || c.ParentFrameId != frame.Id || final != frame.Judgment)
            return false;
        var candidate = frame.Candidates[frame.CandidateIndex];
        return candidate.OwnerSeat == child.OwnerSeat && candidate.SkillId == child.SkillId && candidate.TriggerId == child.TriggerId && candidate.SkillInstanceId == child.SkillInstanceId && candidate.GameplayHash == child.GameplayHash && _coreObserverSubtreesOperations.FindProgramObserverRoot((f, p) => _characterZhouQunOlOperations.ZhouQunFirstChild(f, p), (i) => _coreSameNameHandDiscardOrDamageObserversOperations2.SameNameHandObserverSuffix(i), root.Id)is not null;
    }
}
