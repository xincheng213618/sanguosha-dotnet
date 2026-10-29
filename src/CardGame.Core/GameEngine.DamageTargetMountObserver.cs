namespace CardGame.Core;
public sealed partial class GameEngine
{
    // This entire capability is opt-in to the exact paid 2300 producer and actual scalar record.
    // No source lifetime check: an already paid tail must also admit legal source loss/death cleanup.
    private ProgramSkillFrame? PaidDamageTargetMountObserverRoot(long damageWindowId)
    {
        for(var i=1;i+1<_resolutionStack.Count;i++)
        {
            if(_resolutionStack[i] is not ProgramSkillFrame root || root.DamageTargetMount is not {Receipt:{ } receipt} draft ||
                !IsValidDamageTargetMount(root) || draft.DamageWindowId!=damageWindowId ||
                _resolutionStack[i-1] is not DamageTriggerWindowFrame damage || damage.Id!=damageWindowId ||
                damage.ParentFrameId!=draft.DamageFrameId || damage.CandidateIndex<0 || damage.CandidateIndex>=damage.Candidates.Count ||
                damage.Candidates[damage.CandidateIndex].ToProgramCandidate() is not { } candidate || !MountObserverCandidateMatches(root,candidate) ||
                _resolutionStack[i+1] is not CardsMovedTriggerWindowFrame first ||
                root.PendingMovementContinuation is not {CoverageResultBind:null} pending || pending.SubjectSeat!=root.OwnerSeat ||
                first.Batch.ParentFrameId!=root.Id || first.Batch.AwaitingProgramFrameId!=root.Id ||
                first.Batch.OriginSkillId!=root.SkillId || first.Batch.OriginSkillInstanceId!=root.SkillInstanceId || first.Batch.OriginOwnerSeat!=root.OwnerSeat ||
                first.Batch.Movements.Count!=1) continue;
            var record=first.Batch.Movements[0];
            if(draft.ClaimIssued
                ? record.Sequence!=draft.ClaimMovementSequence || record.CardId!=receipt.CardId || record.From!=receipt.To || record.To!=CardLocation.Hand(root.OwnerSeat) || record.Reason.Value!="skill-program.damage-target-mount.claim"
                : record.Sequence!=receipt.MovementSequence || record.CardId!=receipt.CardId || record.From!=receipt.From || record.To!=receipt.To || record.Reason.Value!=receipt.Reason) continue;
            if(record.CardKind!=receipt.PrintedKind || !_cardMovements.Contains(record))continue;
            var legal=true;
            for(var n=i+1;n<_resolutionStack.Count;n++)
                if(!ProgramChildResolutionEdge(n)){legal=false;break;}
            if(legal)return root;
        }
        return null;
    }
    private static bool MountObserverCandidateMatches(ProgramSkillFrame p,ProgramTriggerCandidate c) =>
        global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(p,c);
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 30)]
    private bool HasPaidDamageTargetMountObserver(long damageWindowId)=>PaidDamageTargetMountObserverRoot(damageWindowId) is not null;
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 300, group: 10)]
    private bool IsPaidDamageTargetMountDying()=>ActiveDamageTrigger is{ } damage && ActiveDying is{ResumesProgramSkill:true} dying &&
        PaidDamageTargetMountObserverRoot(damage.Id) is not null &&
        _resolutionStack.OfType<DyingFrame>().Any(f=>f.Id==dying.FrameId && f.ParentFrameId==dying.ParentFrameId);
}
