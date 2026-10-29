namespace CardGame.Core;
public sealed partial class GameEngine
{
    private bool FrozenHandRosterFirstChild(ProgramSkillFrame f,ResolutionFrame child)
    {
        if(f.FrozenHandRoster is not{}r || !ValidFrozenHandRoster(f))return false;
        if(r.Stage=="movement" && f.PendingMovementContinuation is not null && child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId is null && moved.Batch.ParentFrameId==f.Id && moved.Batch.AwaitingProgramFrameId==f.Id &&
                moved.Batch.OriginOwnerSeat==f.OwnerSeat && moved.Batch.OriginSkillId==f.SkillId && moved.Batch.OriginSkillInstanceId==f.SkillInstanceId &&
                moved.Batch.Movements.Count>0 && moved.Batch.Movements.All(m=>_cardMovements.Contains(m) && m.Sequence>r.Before && m.Sequence<=r.After);
        return r.Stage=="damage-issued" && r.DamageIssued && f.AttackAttempt is{} attack && f.AttackReturn is not null &&
            attack.SourceSeat==f.OwnerSeat && attack.TargetSeat==r.DamageTarget && !attack.SourceLess && DyingSuitsStructuralEdge(f,child);
    }
    private ProgramSkillFrame? FrozenHandRosterObserverRoot()=>FindProgramObserverRoot(
        static(e,r,c)=>e.FrozenHandRosterFirstChild(r,c),static(e,i)=>e.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 80, group: 10)]
    private bool IsFrozenHandRosterDying()=>ActiveDying is{}d && FrozenHandRosterObserverRoot() is{}r && _resolutionStack.FindIndex(f=>f.Id==d.Id)>_resolutionStack.FindIndex(f=>f.Id==r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1740)]
    private bool HasFrozenHandRosterDamageObserver(long id)=>_resolutionStack.Any(f=>f.Id==id && f is DamageTriggerWindowFrame) && FrozenHandRosterObserverRoot() is not null;
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 260)]
    private bool HasFrozenHandRosterBeforeDamageObserver(long id)=>_resolutionStack.Any(f=>f.Id==id && f is BeforeDamageProgramWindowFrame) && FrozenHandRosterObserverRoot() is not null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 520)]
    private bool TryAdvanceFrozenHandRosterSubtree()=>_pendingDecision is null && FrozenHandRosterObserverRoot() is not null && TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 530)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 550)]
    private bool CanContinueFrozenHandRoster(ProgramSkillFrame f)=>f.FrozenHandRoster is{Stage:"movement" or "damage-issued"} && ValidFrozenHandRoster(f);
}
