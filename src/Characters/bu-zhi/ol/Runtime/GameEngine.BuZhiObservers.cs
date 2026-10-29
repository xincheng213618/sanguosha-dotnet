namespace CardGame.Core;
public sealed partial class GameEngine
{
    private bool BuZhiFirstChild(ProgramSkillFrame f,ResolutionFrame child)
    {
        if(f.BuZhi is not{}r || !ValidBuZhi(f))return false;
        if(r.Stage=="movement" && f.PendingMovementContinuation is not null && child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId is null && moved.Batch.ParentFrameId==f.Id && moved.Batch.AwaitingProgramFrameId==f.Id &&
                moved.Batch.OriginOwnerSeat==f.OwnerSeat && moved.Batch.OriginSkillId==f.SkillId && moved.Batch.OriginSkillInstanceId==f.SkillInstanceId &&
                moved.Batch.Movements.Count>0 && moved.Batch.Movements.All(m=>_cardMovements.Contains(m) && m.Sequence>r.Before && m.Sequence<=r.After);
        return r.Stage=="damage-issued" && r.DamageIssued && f.AttackAttempt is{}attack && f.AttackReturn is not null &&
            attack.SourceSeat==f.OwnerSeat && attack.TargetSeat==r.Target && !attack.SourceLess && DyingSuitsStructuralEdge(f,child);
    }
    private ProgramSkillFrame? BuZhiObserverRoot()=>FindProgramObserverRoot(static(e,r,c)=>e.BuZhiFirstChild(r,c),static(e,i)=>e.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 10)]
    private bool IsBuZhiDying()=>ActiveDying is{}d && BuZhiObserverRoot() is{}r && _resolutionStack.FindIndex(f=>f.Id==d.Id)>_resolutionStack.FindIndex(f=>f.Id==r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 50)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 160)]
    private bool HasBuZhiDamageObserver(long id)=>_resolutionStack.Any(f=>f.Id==id && f is DamageTriggerWindowFrame) && BuZhiObserverRoot() is not null;
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 270)]
    private bool HasBuZhiBeforeDamageObserver(long id)=>_resolutionStack.Any(f=>f.Id==id && f is BeforeDamageProgramWindowFrame) && BuZhiObserverRoot() is not null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 130)]
    private bool TryAdvanceBuZhiSubtree()=>_pendingDecision is null && BuZhiObserverRoot() is not null && TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 70)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 70)]
    private bool CanContinueBuZhi(ProgramSkillFrame f)=>ValidBuZhi(f) &&
        (f.BuZhi is{Stage:"movement" or "damage-issued"} ||
         f.BuZhi is{Stage:"branch" or "discard" or "damage"} && _players[f.OwnerSeat].IsAlive && BuZhiPaidEquipmentEntitlement(f));
}
