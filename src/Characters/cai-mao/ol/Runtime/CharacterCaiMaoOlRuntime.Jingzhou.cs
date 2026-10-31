#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiMaoOlRuntime
{
    internal bool CaiMaoDamageParentMatches(ProgramSkillFrame f, ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.BeforeDamageApplied && context.OwnerSeat == f.OwnerSeat && context.TargetSeat == f.OwnerSeat && context.Amount > 0 && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } parent && parent.TargetSeat == f.OwnerSeat && parent.Amount == context.Amount && !parent.Prevented && parent.RedirectedTargetSeat is null && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex].Candidate);
}
