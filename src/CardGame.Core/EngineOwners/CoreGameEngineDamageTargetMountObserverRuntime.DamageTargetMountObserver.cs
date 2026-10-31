#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageTargetMountObserverRuntime
{
    internal ProgramSkillFrame? PaidDamageTargetMountObserverRoot(long damageWindowId)
    {
        for (var i = 1; i + 1 < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            if (_resolutionStackOperations._resolutionStack[i] is not ProgramSkillFrame root || root.DamageTargetMount is not { Receipt: { } receipt } draft || !_coreDamageTargetMountProgramsOperations.IsValidDamageTargetMount(root) || draft.DamageWindowId != damageWindowId || _resolutionStackOperations._resolutionStack[i - 1] is not DamageTriggerWindowFrame damage || damage.Id != damageWindowId || damage.ParentFrameId != draft.DamageFrameId || damage.CandidateIndex < 0 || damage.CandidateIndex >= damage.Candidates.Count || damage.Candidates[damage.CandidateIndex].ToProgramCandidate()is not { } candidate || !MountObserverCandidateMatches(root, candidate) || _resolutionStackOperations._resolutionStack[i + 1] is not CardsMovedTriggerWindowFrame first || root.PendingMovementContinuation is not { CoverageResultBind: null } pending || pending.SubjectSeat != root.OwnerSeat || first.Batch.ParentFrameId != root.Id || first.Batch.AwaitingProgramFrameId != root.Id || first.Batch.OriginSkillId != root.SkillId || first.Batch.OriginSkillInstanceId != root.SkillInstanceId || first.Batch.OriginOwnerSeat != root.OwnerSeat || first.Batch.Movements.Count != 1)
                continue;
            var record = first.Batch.Movements[0];
            if (draft.ClaimIssued ? record.Sequence != draft.ClaimMovementSequence || record.CardId != receipt.CardId || record.From != receipt.To || record.To != CardLocation.Hand(root.OwnerSeat) || record.Reason.Value != "skill-program.damage-target-mount.claim" : record.Sequence != receipt.MovementSequence || record.CardId != receipt.CardId || record.From != receipt.From || record.To != receipt.To || record.Reason.Value != receipt.Reason)
                continue;
            if (record.CardKind != receipt.PrintedKind || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(record))
                continue;
            var legal = true;
            for (var n = i + 1; n < _resolutionStackOperations._resolutionStack.Count; n++)
                if (!_coreProgramChildResolutionOperations.ProgramChildResolutionEdge(n))
                {
                    legal = false;
                    break;
                }

            if (legal)
                return root;
        }

        return null;
    }

    internal static bool MountObserverCandidateMatches(ProgramSkillFrame p, ProgramTriggerCandidate c) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(p, c);
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 30)]
    internal bool HasPaidDamageTargetMountObserver(long damageWindowId) => PaidDamageTargetMountObserverRoot(damageWindowId)is not null;
}
