namespace CardGame.Core;
internal sealed partial class QuHuangRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 360)]
    private bool IsQuHuangMovement(ProgramSkillFrame f,SkillProgramEffect? e,ProgramMovementContinuation p)=>f.QuHuang is{Paid:true}r&&e?.Op==r.Op&&p==f.PendingMovementContinuation&&p.SubjectSeat==r.TargetSeat&&p.BeforeCount==0&&p.CoverageResultBind is null;
    [ProgramViewContributor(ProgramViewContribution.PublicProcessingExclusion, 10)]
    private bool IsNishouHeldProcessingCard(int cardId)=>HasQuHuangCapability&&_cardZones.GetLocation(cardId)==CardLocation.Processing&&_resolutionStack.OfType<ProgramSkillFrame>().Any(f=>f.QuHuang is{Op:SkillProgramEffectOp.ResolveNishouIssuedCurrentPhaseSwap,Paid:true}r&&r.OwnerCards.Concat(r.OtherCards).Any(c=>c.CardId==cardId)&&ProgramEventHistory<QuHuangEntityMovedEvent>().Any(e=>e.FrameId==f.Id&&e.CardId==cardId&&e.To==CardLocation.Processing&&e.Stage is QuHuangStage.HoldOwner or QuHuangStage.HoldOther&&_cardMovements.LastOrDefault(m=>m.CardId==cardId)is{}last&&last.Sequence==e.Sequence&&last.From==e.From&&last.To==e.To&&last.Reason.Value==NishouHoldReason));
    [ProgramViewContributor(ProgramViewContribution.InvariantProcessing, 10)]
    private IReadOnlyList<Card> WithoutNishouHeldProcessingCards(IReadOnlyList<Card> processing)=>!HasQuHuangCapability?processing:Array.AsReadOnly(processing.Where(c=>!IsNishouHeldProcessingCard(c.Id)).ToArray());
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 70)]
    private IReadOnlyList<CardSnapshot> NishouPrivatelyHeldCards(int viewerSeat)=>!HasQuHuangCapability?[]:Array.AsReadOnly(_resolutionStack.OfType<ProgramSkillFrame>().Where(f=>f.QuHuang?.Op==SkillProgramEffectOp.ResolveNishouIssuedCurrentPhaseSwap).SelectMany(f=>f.QuHuang!.OwnerCards.Concat(f.QuHuang.OtherCards)).Where(c=>c.From==CardLocation.Hand(viewerSeat)&&IsNishouHeldProcessingCard(c.CardId)).Select(c=>snapshots.Snapshot(GetAdvancedCard(c.CardId))).ToArray());
    private bool QuHuangMovementMatches(ProgramSkillFrame f,CardMovementRecord m)
    {
        var r=f.QuHuang!;if(!_cardMovements.Contains(m)||m.Sequence<=r.Before||m.Sequence>r.After)return false;
        if(r.Stage is QuHuangStage.DrawOwner or QuHuangStage.DrawOther)return m.From==CardLocation.DrawPile&&m.To==CardLocation.Hand(r.Stage==QuHuangStage.DrawOwner?f.OwnerSeat:r.TargetSeat)&&m.Reason.Value==QiejianDrawReason||m.From==CardLocation.DiscardPile&&m.To==CardLocation.DrawPile&&m.Reason==CardMoveReasons.Reshuffle;
        if(ProgramEventHistory<QuHuangEntityMovedEvent>().Any(e=>e.FrameId==f.Id&&e.Step==r.Step&&e.Sequence==m.Sequence&&e.CardId==m.CardId&&e.From==m.From&&e.To==m.To))return true;
        return r.Grain.Contains(m.CardId)&&m.From.Zone==CardZoneKind.WoodenOxGrain&&m.To==CardLocation.DiscardPile&&m.Reason==CardMoveReasons.WoodenOxGrainDiscard;
    }
    private bool QuHuangLion(ProgramSkillFrame f,int seat)=>f.QuHuang is{Stage:QuHuangStage.FieldChildren}r&&r.Entities.Any(c=>c.Kind==CardKind.SilverLion&&c.From==CardLocation.Equipment(seat));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 160)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 150)]
    private bool IsQuHuangRecoveryParent(ProgramSkillFrame f,RecoveryReplacementFrame recovery)=>QuHuangLion(f,recovery.Attempt.TargetSeat)&&native.RecoveryRidesOn(recovery,f)&&recovery.Return.Continuation==PostEventContinuation.AwaitedProgramMovement&&recovery.Attempt.Amount==1&&recovery.Attempt.SourceSeat==recovery.Attempt.TargetSeat&&recovery.Attempt.Completion.Producer==RecoveryAttemptProducer.SilverLion&&recovery.Attempt.Completion.MoveReason?.Value==QiejianDiscardReason;
    private bool QuHuangFirstChild(ProgramSkillFrame f,ResolutionFrame child)
    {
        if(f.QuHuang is not{Paid:true}r)return false;        if(child is CardUseFrame use)return use.QuHuangNishou is{}o&&MatchesQuHuangNishou(f,o)&&IsQuHuangNishouUse(use.Id);
        if(child is CardsMovedTriggerWindowFrame moved)return r.Stage==QuHuangStage.LightningChildren?moved.ResumeProgramFrameId==f.Id&&IsQuHuangNativeSettlementBatch(f,moved.Batch):f.PendingMovementContinuation is not null&&native.MovementReturnMatches(f,moved)&&moved.Batch.Movements.All(m=>QuHuangMovementMatches(f,m));
        if(child is HpChangedTriggerWindowFrame hp)return hp.ResumeFrameId==f.Id&&hp.Change.ParentFrameId==f.Id&&hp.Continuation==PostEventContinuation.AwaitedProgramMovement&&hp.Change.Kind==HpChangeKind.Recovery&&hp.Change.SourceSeat==hp.Change.TargetSeat&&QuHuangLion(f,hp.Change.TargetSeat);
        if(child is RecoveryReplacementFrame recovery)return IsQuHuangRecoveryParent(f,recovery);
        return child is ProgramLifecycleTriggerWindowFrame{Window:SkillProgramTriggerWindow.SkillsChanged}skills&&skills.ResumeProgramFrameId==f.Id&&skills.Continuation==ProgramLifecycleContinuation.ResumeParentProgram;
    }
    private ProgramSkillFrame? QuHuangObserverRoot()
    {
        var stack=_resolutionStack;
        for(var index=0;index+1<stack.Count;index++)
            if(stack[index]is ProgramSkillFrame root&&QuHuangFirstChild(root,stack[index+1])&&QuHuangObserverSuffix(stack,index))return root;
        return null;
    }
    private bool QuHuangObserverSuffix(IReadOnlyList<ResolutionFrame> stack,int rootIndex)
    {
        for(var child=rootIndex+2;child<stack.Count;child++)
        {
            if(!native.IsMovementObserverEdge(stack[child].Id)&&!native.RecoveryRidesOn(stack[child],stack[child-1]))return false;
            if(stack[child]is DyingFrame dying&&child+1<stack.Count&&native.IsSuspendedOrPaidRescueSuffix(stack[child].Id,dying))return true;
        }
        return true;
    }
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 870)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 980)]
    private bool HasQuHuangDamageObserver(long id)=>QuHuangObserverRoot()is{}root&&FrameIndex(f=>f.Id==id&&f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame)>FrameIndex(f=>f.Id==root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1010)]
    private bool HasQuHuangBeforeDamageObserver(long id)=>HasQuHuangDamageObserver(id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth,27420)]private bool AdvanceQuHuangChildren()=>prompts.Current is null&&QuHuangObserverRoot()is not null&&children.AdvanceObserverSubtree();
}
