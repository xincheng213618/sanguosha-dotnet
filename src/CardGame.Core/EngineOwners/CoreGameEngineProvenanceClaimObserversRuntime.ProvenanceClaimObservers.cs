#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProvenanceClaimObserversRuntime
{
    internal bool ProvenanceClaimTurnedOverEdge(ResolutionFrame child, ResolutionFrame parent)
    {
        if (child is ProgramLifecycleTriggerWindowFrame window && parent is ProgramSkillFrame { ProvenanceClaim: { Stage: ProgramProvenanceClaimStage.Flipping } } root)
            return window.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && window.CharacterStateContinuation == CharacterStateContinuation.Program && window.ResumeProgramFrameId == root.Id && window.OwnerSeat == root.OwnerSeat && window.Window is SkillProgramTriggerWindow.CharacterTurnedOver or SkillProgramTriggerWindow.CharacterTurnedFaceUp;
        if (child is not ProgramSkillFrame p || parent is not ProgramLifecycleTriggerWindowFrame lifecycle || lifecycle.Continuation != ProgramLifecycleContinuation.ResumeCharacterStateChange || lifecycle.CharacterStateContinuation != CharacterStateContinuation.Program || lifecycle.Window is not (SkillProgramTriggerWindow.CharacterTurnedOver or SkillProgramTriggerWindow.CharacterTurnedFaceUp) || lifecycle.CandidateIndex < 0 || lifecycle.CandidateIndex >= lifecycle.Candidates.Count || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == lifecycle.ResumeProgramFrameId)is not { ProvenanceClaim: { Stage: ProgramProvenanceClaimStage.Flipping } } original)
            return false;
        return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(p, lifecycle.Candidates[lifecycle.CandidateIndex]) && p.WindowContext is { } context && context.ParentFrameId == lifecycle.Id && context.Window == lifecycle.Window && context.TargetSeat == original.OwnerSeat;
    }
}
