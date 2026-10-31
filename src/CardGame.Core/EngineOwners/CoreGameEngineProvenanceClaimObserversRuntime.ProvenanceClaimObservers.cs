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

    internal ProgramSkillFrame? ProvenanceClaimDamageObserverRoot(long damageId)
    {
        var start = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == damageId);
        if (start < 0 || start + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[start] is not DamageTriggerWindowFrame damage || damage.CandidateIndex < 0 || damage.CandidateIndex >= damage.Candidates.Count || _resolutionStackOperations._resolutionStack[start + 1] is not ProgramSkillFrame original || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(original, damage.Candidates[damage.CandidateIndex].ToProgramCandidate()) || original.WindowContext?.ParentFrameId != damage.Id)
            return null;
        ProgramSkillFrame? paid = null;
        for (var i = start + 2; i < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            if (_resolutionStackOperations._resolutionStack[i] is ProgramSkillFrame { ProvenanceClaim: { } } claim)
            {
                _coreDiscardedProvenanceOperations.AssertProvenanceClaim(claim);
                if (_resolutionStackOperations._resolutionStack[i - 1] is not CardsMovedTriggerWindowFrame movement || movement.CandidateIndex < 0 || movement.CandidateIndex >= movement.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(claim, movement.Candidates[movement.CandidateIndex]) || claim.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPileReceived } c || c.ParentFrameId != movement.Id)
                    return null;
                paid = claim;
            }

            if (_resolutionStackOperations._resolutionStack[i - 1] is DyingFrame dying && _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(i - 1, dying))
                break;
            if (!_coreProgramChildResolutionOperations.ProgramChildResolutionEdge(i))
                return null;
        }

        return paid;
    }

    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1720)]
    internal bool HasProvenanceClaimObserver(long damageId) => ProvenanceClaimDamageObserverRoot(damageId)is not null;
}
