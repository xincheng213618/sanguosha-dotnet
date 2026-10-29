namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool HeQiFirstChild(ProgramSkillFrame root,ResolutionFrame child)
    {
        if(!CanContinueHeQi(root)||root.HeQi is not{}r||child is not CardsMovedTriggerWindowFrame moved)return false;
        return moved.Batch.ParentFrameId==root.Id && moved.Batch.AwaitingProgramFrameId==root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat==root.OwnerSeat && moved.Batch.OriginSkillId==root.SkillId && moved.Batch.OriginSkillInstanceId==root.SkillInstanceId && moved.Batch.Movements.Count>0 && moved.Batch.Movements.All(m=>_cardMovements.Contains(m) && m.Sequence>r.Before && m.Sequence<=r.After && (m.Reason.Value==HeQiReason(r.Stage) && r.Paid.Any(p=>p.CardId==m.CardId && p.From==m.From) || r.Stage==HeQiStage.ObtainChildren && r.Paid.Any(p=>p.PrintedKind==CardKind.WoodenOx && p.From==CardLocation.Equipment(r.TargetSeat)) && m.From==CardLocation.WoodenOxGrain(r.TargetSeat) && m.To==CardLocation.DiscardPile && m.Reason==CardMoveReasons.WoodenOxGrainDiscard));
    }
    private ProgramSkillFrame? HeQiObserverRoot()=>FindProgramObserverRoot(static(e,r,c)=>e.HeQiFirstChild(r,c),static(e,i)=>e.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 90)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 200)]
    private bool HasHeQiDamageObserver(long id)=>HeQiObserverRoot()is{}r && _resolutionStack.Any(f=>f.Id==id && f is DamageTriggerWindowFrame) && _resolutionStack.FindIndex(f=>f.Id==id)>_resolutionStack.FindIndex(f=>f.Id==r.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 50)]
    private bool IsHeQiDying()=>ActiveDying is{}dying && HeQiObserverRoot()is{}r && _resolutionStack.FindIndex(f=>f.Id==dying.Id)>_resolutionStack.FindIndex(f=>f.Id==r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 170)]
    private bool TryAdvanceHeQiSubtree()=>_pendingDecision is null && HeQiObserverRoot()is not null && TryAdvanceObserverSubtree(advanceRecoveryReplacement:true);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 300)]
    private bool HasHeQiBeforeDamageObserver(long id)=>HeQiObserverRoot()is{}r && _resolutionStack.Any(f=>f.Id==id && f is BeforeDamageProgramWindowFrame) && _resolutionStack.FindIndex(f=>f.Id==id)>_resolutionStack.FindIndex(f=>f.Id==r.Id);
}
