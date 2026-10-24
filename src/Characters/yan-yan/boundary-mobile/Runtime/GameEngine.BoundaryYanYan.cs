namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundaryYanYanClaimReason="skill.boundary-yan-yan.chengshi";
    private static SkillPolarity InitialBoundaryYanYanPolarity(CharacterState player,string skillId)=>
        skillId==BoundaryYanYanComposition.Skill&&player.General.FactionId=="shu"?SkillPolarity.Yin:SkillPolarity.Yang;
    private int BoundaryYanYanSlashBonus(int owner)=>ProgramEventHistory<BoundaryYanYanChengshiGrantedEvent>().Count(e=>e.OwnerSeat==owner);
    private long BoundaryYanYanEpoch(int owner)=>ProgramEventHistory<ProgramConversionPolarityCommittedEvent>()
        .LastOrDefault(e=>e.OwnerSeat==owner&&e.SkillId==BoundaryYanYanComposition.Skill)?.FrameId??0;
    [ProgramCapability]private sealed class BoundaryYanYanHost(GameEngine e):IBoundaryYanYanHost
    {public SkillProgramStepOutcome Execute(ProgramSkillFrame f,SkillProgramEffectOp op)=>op switch
        {SkillProgramEffectOp.PrepareBoundaryYanYanJuzhan=>e.PrepareBoundaryYanYan(f),SkillProgramEffectOp.FinishBoundaryYanYanJuzhan=>e.FinishBoundaryYanYan(f),_=>e.ClaimBoundaryYanYan(f)};}
    private bool HasBoundaryYanYanHe(int seat)=>IsValidPlayerSeat(seat)&&_players[seat].IsAlive&&GetHand(_players[seat]).Count+GetEquipment(seat).Count>0;

    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger,54300)]
    private void ObserveBoundaryYanYan(CardActionContext action,SkillProgramTriggerWindow window,List<ProgramCardTriggerCandidate> candidates)
    {
        if(!IsSlashCard(action.EffectiveKind))return;
        if(window==SkillProgramTriggerWindow.CardUseCompleted)
        {
            if(_resolutionStack.LastOrDefault()is not CardUseFrame{BoundaryYanYanClaims:{}claims}use||use.Action?.ActionId!=action.ActionId)return;
            foreach(var claim in claims.Where(c=>!c.Settled&&_players[c.Source.OwnerSeat].IsAlive).DistinctBy(c=>(c.Source.OwnerSeat,c.Source.SkillInstanceId)))
                if(!candidates.Any(c=>c.OwnerSeat==claim.Source.OwnerSeat&&c.SkillId==claim.Source.SkillId&&c.SkillInstanceId==claim.Source.SkillInstanceId&&c.TriggerId=="chengshi-completed-use"))
                    candidates.Add(new(claim.Source.OwnerSeat,-1,claim.Source.SkillId,"chengshi-completed-use",claim.GameplayHash,claim.Source.SkillInstanceId,0,
                        CreateCardActionProgramContext(action,window,0,claim.Source.OwnerSeat,-1,CaptureProgramTriggerFacts(_players[claim.Source.OwnerSeat],action))));
            return;
        }
        var trigger=window==SkillProgramTriggerWindow.CardUseTargetsFinalized?"yin-designated-use":window==SkillProgramTriggerWindow.CardUseBeforeTargetEffects?"yang-became-target":null;
        if(trigger is null)return;
        for(var index=0;index<candidates.Count;index++)
        {
            var c=candidates[index];if(c.SkillId!=BoundaryYanYanComposition.Skill||c.TriggerId!=trigger||c.FrozenContext is not{CardUse:{}info}context)continue;
            var polarity=trigger=="yin-designated-use"?SkillPolarity.Yin:SkillPolarity.Yang;var epoch=BoundaryYanYanEpoch(c.OwnerSeat);
            var target=polarity==SkillPolarity.Yin?-1:context.TargetSeat??-1;
            if(polarity==SkillPolarity.Yang&&!IsValidPlayerSeat(target))throw new InvalidOperationException("Yang Juzhan requires the actual current Slash target occurrence.");
            var records=ProgramEventHistory<BoundaryYanYanOccurrenceRecordedEvent>().Select(e=>e.Occurrence).Where(o=>o.OwnerSeat==c.OwnerSeat&&o.SkillInstanceId==c.SkillInstanceId&&o.Epoch==epoch&&o.Polarity==polarity).ToArray();
            var occurrence=records.SingleOrDefault(o=>o.ActionId==action.ActionId&&o.TargetSeat==target);
            if(occurrence is null)
            {
                var use=_resolutionStack.OfType<CardUseFrame>().Single(u=>u.Action?.ActionId==action.ActionId);
                occurrence=new(c.OwnerSeat,c.SkillInstanceId,c.GameplayHash,epoch,polarity,use.Id,action.ActionId,action.ActorSeat,target,records.Length+1);
                AdvanceEventRulesAndQueueFact(new BoundaryYanYanOccurrenceRecordedEvent(occurrence));
            }
            candidates[index]=c with{FrozenContext=context with{BoundaryYanYanOccurrence=occurrence}};
        }
    }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification,54300)]
    private bool CanOfferBoundaryYanYan(ProgramTriggerLegalityContext gate)
    {
        if(gate.SkillId!=BoundaryYanYanComposition.Skill)return true;
        if(_resolutionStack.OfType<ProgramSkillFrame>().Any(f=>f.OwnerSeat==gate.OwnerSeat&&f.SkillId==gate.SkillId&&f.SkillInstanceId==gate.Candidate.SkillInstanceId&&
            f.TriggerId==gate.Candidate.BindingId&&f.WindowContext==gate.Context&&(ValidBoundaryYanYanPrepared(f)||f.BoundaryYanYanClaim is not null&&
                BoundaryYanYanClaimUse(gate.Candidate,gate.Context,true)is not null)))return true;
        if(gate.Candidate.BindingId=="chengshi-completed-use")return BoundaryYanYanClaimUse(gate.Candidate,gate.Context)is not null;
        if(!ValidBoundaryYanYanOccurrence(gate.OwnerSeat,gate.Candidate.SkillInstanceId,gate.Context,out var occurrence))return false;
        if(occurrence.Ordinal!=3&&(occurrence.Polarity==SkillPolarity.Yin?occurrence.ActorSeat:occurrence.TargetSeat)!=gate.OwnerSeat)return false;
        return occurrence.Polarity!=SkillPolarity.Yin||LifecycleCardUse(occurrence.UseId)?.Action?.EffectiveDesignatedTargetSeats.Any(HasBoundaryYanYanHe)==true;
    }
    private bool ValidBoundaryYanYanOccurrence(int owner,string instance,ProgramSkillWindowContext context,out BoundaryYanYanOccurrence occurrence)
    {
        occurrence=context.BoundaryYanYanOccurrence!;
        var captured=occurrence;
        return occurrence is not null&&occurrence.OwnerSeat==owner&&occurrence.SkillInstanceId==instance&&context.CardUse is{}info&&
            info.ParentCardUseFrameId==occurrence.UseId&&info.CardActionId==occurrence.ActionId&&info.ActorSeat==occurrence.ActorSeat&&
            LifecycleCardUse(occurrence.UseId)is{Action:{}a}&&a.ActionId==occurrence.ActionId&&IsSlashCard(a.EffectiveKind)&&
            (occurrence.Polarity==SkillPolarity.Yin?context.Window==SkillProgramTriggerWindow.CardUseTargetsFinalized:
                context.Window==SkillProgramTriggerWindow.CardUseBeforeTargetEffects&&context.TargetSeat==occurrence.TargetSeat&&a.EffectiveDesignatedTargetSeats.Contains(occurrence.TargetSeat))&&
            ProgramEventHistory<BoundaryYanYanOccurrenceRecordedEvent>().Count(e=>e.Occurrence==captured)==1;
    }
    private bool ValidBoundaryYanYanPrepared(ProgramSkillFrame f)=>f.BoundaryYanYan is{}r&&f.WindowContext is{}w&&
        ValidBoundaryYanYanOccurrence(f.OwnerSeat,f.SkillInstanceId,w,out var o)&&o==r.Occurrence&&r.WindowId==w.ParentFrameId&&
        ProgramEventHistory<BoundaryYanYanPreparedEvent>().Count(e=>e.FrameId==f.Id&&e.WindowId==r.WindowId&&e.Occurrence==o)==1;
    private SkillProgramStepOutcome PrepareBoundaryYanYan(ProgramSkillFrame f)
    {
        if(f.SkillId!=BoundaryYanYanComposition.Skill||f.InstructionIndex!=1||f.BoundaryYanYan is not null||f.WindowContext is not{}context||
            !ValidBoundaryYanYanOccurrence(f.OwnerSeat,f.SkillInstanceId,context,out var o)||o.Epoch!=BoundaryYanYanEpoch(f.OwnerSeat))
            throw new InvalidOperationException("Juzhan must prepare the exact current state occurrence before committing conversion.");
        ReplaceRuntimeFrame(f.Id,f with{BoundaryYanYan=new(o,context.ParentFrameId)});
        AdvanceEventRulesAndQueueFact(new BoundaryYanYanPreparedEvent(f.Id,context.ParentFrameId,o));return SkillProgramStepOutcome.Continue;
    }
    private SkillProgramStepOutcome FinishBoundaryYanYan(ProgramSkillFrame f)
    {
        if(!ValidBoundaryYanYanPrepared(f)||f.ConversionPreviousPolarity!=f.BoundaryYanYan!.Occurrence.Polarity||f.InstructionIndex!=5)
            throw new InvalidOperationException("Juzhan must finish on its original paid conversion frame.");
        var o=f.BoundaryYanYan.Occurrence;var yin=o.Polarity==SkillPolarity.Yin;
        var actor=yin?f.OwnerSeat:o.ActorSeat;var target=yin?f.SelectedTargetSeats.Single():f.OwnerSeat;
        if(actor==target)
        {ValidateProgramTurnEffectGrant(f);AdvanceEventRulesAndQueueFact(new ProgramSelfCardTargetsProhibitedEvent(f.OwnerSeat,f.SkillId,f.SkillInstanceId,_turnNumber,_currentSeat));}
        else GrantProgramDirectedTurnCardPolicy(f,new(yin?ProgramParticipantRef.Owner:ProgramParticipantRef.Actor),
            new(yin?ProgramParticipantRef.SelectedTarget:ProgramParticipantRef.Owner),[],DirectedTurnCardPolicyEffect.ForbidTarget);
        var chengshi=o.Ordinal==3&&(yin?o.ActorSeat:o.TargetSeat)==f.OwnerSeat;
        if(chengshi)
        {
            if(LifecycleCardUse(o.UseId)is not{Action:{}action}use||action.ActionId!=o.ActionId||(use.BoundaryYanYanClaims??[]).Any(c=>c.ProducerFrameId==f.Id))
                throw new InvalidOperationException("Chengshi requires one original, still resolving Slash Use.");
            var claim=new BoundaryYanYanClaim(f.Id,new(f.SkillId,GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId),f.GameplayHash,use.Id,action.ActionId);
            UpdateLifecycleCardUse(use.Id,u=>u with{BoundaryYanYanClaims=(u.BoundaryYanYanClaims??[]).Append(claim).ToArray()});
            AdvanceEventRulesAndQueueFact(new BoundaryYanYanChengshiGrantedEvent(f.Id,f.OwnerSeat,f.SkillInstanceId,use.Id,action.ActionId));
        }
        AdvanceEventRulesAndQueueFact(new BoundaryYanYanFinishedEvent(f.Id,actor,target,chengshi));
        ReplaceRuntimeFrame(f.Id,GetActiveProgramFrame(f.Id) with{BoundaryYanYan=null});return SkillProgramStepOutcome.Continue;
    }
    private CardUseFrame? BoundaryYanYanClaimUse(ProgramTriggerCandidate c,ProgramSkillWindowContext w,bool settled=false)
    {
        if(c.SkillId!=BoundaryYanYanComposition.Skill||c.BindingId!="chengshi-completed-use"||w.Window!=SkillProgramTriggerWindow.CardUseCompleted||w.OwnerSeat!=c.OwnerSeat||
            w.CardUse is not{}info||LifecycleCardUse(info.ParentCardUseFrameId)is not{Step:ResolutionFrameStep.Completed,Action:{}a,BoundaryYanYanClaims:{}claims}u||a.ActionId!=info.CardActionId||
            !claims.Any(p=>p.Source.OwnerSeat==c.OwnerSeat&&p.Source.SkillInstanceId==c.SkillInstanceId&&p.GameplayHash==c.GameplayHash&&(settled||!p.Settled)&&BoundaryYanYanIssuedClaimMatches(u,p))||
            _resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(x=>x.Id==w.ParentFrameId)is not{}window||window.ParentFrameId!=u.Id||window.CandidateIndex<0||window.CandidateIndex>=window.Candidates.Count)return null;
        var actual=window.Candidates[window.CandidateIndex];return actual.OwnerSeat==c.OwnerSeat&&actual.SkillId==c.SkillId&&actual.TriggerId==c.BindingId&&
            actual.SkillInstanceId==c.SkillInstanceId&&actual.GameplayHash==c.GameplayHash&&CreateCardActionProgramContext(window,actual)==w&&
            ProgramEventHistory<CardUseFinishedEvent>().Count(e=>e.ResolutionId==u.Id)==1?u:null;
    }
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate,54300)]
    private bool IssuedBoundaryYanYanClaim(ProgramTriggerCandidate c,ProgramSkillWindowContext w)=>BoundaryYanYanClaimUse(c,w)is not null;
    private SkillProgramStepOutcome ClaimBoundaryYanYan(ProgramSkillFrame f)
    {
        var candidate=new ProgramTriggerCandidate(f.OwnerSeat,f.SkillId,f.TriggerId!,f.SkillInstanceId,f.GameplayHash,0);
        if(f.BoundaryYanYanClaim is not null||f.WindowContext is not{}w||BoundaryYanYanClaimUse(candidate,w)is not{Action:{}a,BoundaryYanYanClaims:{}claims}use)
            throw new InvalidOperationException("Chengshi obtains only its original fully resolved Slash materials once.");
        var pending=claims.Where(c=>c.Source.OwnerSeat==f.OwnerSeat&&c.Source.SkillInstanceId==f.SkillInstanceId&&!c.Settled).ToArray();
        var ids=a.PhysicalCards.Select(c=>c.CardId).Where(id=>_cardZones.GetLocation(id).Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile&&!IsClaimedUseCardEntity(use.Id,id)).ToArray();
        var before=_movementSequence;var draft=new BoundaryYanYanClaimDraft(use.Id,pending[0].ProducerFrameId,before){Cards=ids,From=ids.Select(id=>_cardZones.GetLocation(id)).ToArray(),After=before};
        UpdateLifecycleCardUse(use.Id,u=>u with{BoundaryYanYanClaims=claims.Select(p=>pending.Contains(p)?p with{Settled=true,Before=before,After=before,Cards=ids}:p).ToArray()});
        if(ids.Length==0){AdvanceEventRulesAndQueueFact(new BoundaryYanYanClaimPaidEvent(f.Id,use.Id,f.OwnerSeat,0,before,before,0));return SkillProgramStepOutcome.Continue;}
        ReplaceRuntimeFrame(f.Id,f with{BoundaryYanYanClaim=draft,PendingMovementContinuation=new(f.OwnerSeat,0,null)});
        MoveProgramCardsFromMultipleSources(ids,CardLocation.Hand(f.OwnerSeat),new(BoundaryYanYanClaimReason),(batch,moves)=>
        {
            if(moves.Count!=ids.Length||moves.Any(m=>!ids.Contains(m.CardId)||m.From!=draft.From[Array.IndexOf(ids,m.CardId)]||m.Sequence<=before||m.To!=CardLocation.Hand(f.OwnerSeat)&&m.To!=CardLocation.OutsideGame))
                throw new InvalidOperationException("Chengshi lost its original material movement invoice.");
            var after=moves.Max(m=>m.Sequence);var own=GetActiveProgramFrame(f.Id);ReplaceRuntimeFrame(f.Id,own with{BoundaryYanYanClaim=draft with{After=after,Batch=batch}});
            UpdateLifecycleCardUse(use.Id,u=>u with{BoundaryYanYanClaims=u.BoundaryYanYanClaims!.Select(p=>pending.Any(old=>old.ProducerFrameId==p.ProducerFrameId)?p with{After=after,Batch=batch}:p).ToArray()});
            AdvanceEventRulesAndQueueFact(new BoundaryYanYanClaimPaidEvent(f.Id,use.Id,f.OwnerSeat,ids.Length,before,after,batch));
        });
        var outcome=AwaitProgramBoundCardMovements(f.Id,f.OwnerSeat);if(outcome==SkillProgramStepOutcome.Continue)ReplaceRuntimeFrame(f.Id,GetActiveProgramFrame(f.Id) with{BoundaryYanYanClaim=null,PendingMovementContinuation=null});return outcome;
    }
    private bool BoundaryYanYanIssuedClaimMatches(CardUseFrame use,BoundaryYanYanClaim p)=>use.Action is{}a&&p.UseId==use.Id&&p.ActionId==a.ActionId&&
        p.Source.SkillId==BoundaryYanYanComposition.Skill&&p.Source.BindingId is("yin-designated-use"or"yang-became-target")&&p.ProducerFrameId>0&&
        ProgramEventHistory<BoundaryYanYanChengshiGrantedEvent>().Count(e=>e.FrameId==p.ProducerFrameId&&e.OwnerSeat==p.Source.OwnerSeat&&
            e.SkillInstanceId==p.Source.SkillInstanceId&&e.UseId==use.Id&&e.ActionId==a.ActionId)==1&&
        ProgramEventHistory<BoundaryYanYanPreparedEvent>().Count(e=>e.FrameId==p.ProducerFrameId&&e.Occurrence is{Ordinal:3}o&&
            o.OwnerSeat==p.Source.OwnerSeat&&o.SkillInstanceId==p.Source.SkillInstanceId&&o.GameplayHash==p.GameplayHash&&o.UseId==use.Id&&o.ActionId==a.ActionId&&
            (o.Polarity==SkillPolarity.Yin?o.ActorSeat:o.TargetSeat)==p.Source.OwnerSeat)==1;
    private bool IsBoundaryYanYanClaimedEntity(long useId,int card)=>LifecycleCardUse(useId)is{BoundaryYanYanClaims:{}claims}u&&claims.Any(p=>p.Settled&&BoundaryYanYanIssuedClaimMatches(u,p)&&
        p.Cards.Contains(card)&&p.After>p.Before&&_cardMovements.Count(m=>m.CardId==card&&m.Sequence>p.Before&&m.Sequence<=p.After&&
            (m.To==CardLocation.Hand(p.Source.OwnerSeat)||m.To==CardLocation.OutsideGame)&&m.Reason.Value==BoundaryYanYanClaimReason)==1);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments,54300)]
    private bool BoundaryYanYanClaimMovement(ProgramSkillFrame f,SkillProgramEffect? effect,ProgramMovementContinuation p)=>effect?.Op==SkillProgramEffectOp.ClaimBoundaryYanYanChengshi&&
        p==f.PendingMovementContinuation&&p.SubjectSeat==f.OwnerSeat&&p.BeforeCount==0&&p.CoverageResultBind is null&&f.BoundaryYanYanClaim is{}r&&
        f.WindowContext is{}w&&BoundaryYanYanClaimUse(new(f.OwnerSeat,f.SkillId,f.TriggerId!,f.SkillInstanceId,f.GameplayHash,0),w,true)is{BoundaryYanYanClaims:{}claims}&&
        claims.Any(c=>c.ProducerFrameId==r.ProducerFrameId&&c.Source.OwnerSeat==f.OwnerSeat&&c.Settled&&c.Before==r.Before&&c.After==r.After&&c.Batch==r.Batch&&c.Cards.SequenceEqual(r.Cards))&&
        ProgramEventHistory<BoundaryYanYanClaimPaidEvent>().Count(e=>e.FrameId==f.Id&&e.UseId==r.UseId&&e.Before==r.Before&&e.After==r.After&&e.Batch==r.Batch&&e.Count==r.Cards.Count)==1&&
        r.Cards.Count==r.From.Count&&r.Cards.Select((id,index)=>(id,index)).All(x=>_cardMovements.Count(m=>m.CardId==x.id&&m.From==r.From[x.index]&&m.Sequence>r.Before&&m.Sequence<=r.After&&
            m.Reason.Value==BoundaryYanYanClaimReason&&(m.To==CardLocation.Hand(f.OwnerSeat)||m.To==CardLocation.OutsideGame))==1);
    [ProgramContinuation(ProgramContinuationKind.Movement,54300)]
    private bool ReturnBoundaryYanYanClaim(ProgramSkillFrame f)
    {if(f.BoundaryYanYanClaim is null)return false;ReplaceRuntimeFrame(f.Id,f with{BoundaryYanYanClaim=null,PendingMovementContinuation=null});AdvanceRuntimeProgram(f.Id);return true;}
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss,54300)][ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath,54300)]
    private bool KeepPaidBoundaryYanYan(ProgramSkillFrame f)=>ValidBoundaryYanYanPrepared(f)||f.BoundaryYanYanClaim is not null&&f.WindowContext is{}w&&
        BoundaryYanYanClaimUse(new(f.OwnerSeat,f.SkillId,f.TriggerId!,f.SkillInstanceId,f.GameplayHash,0),w,true)is not null;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState,54300)]
    private void AssertBoundaryYanYan(ProgramSkillFrame f)
    {if(f.BoundaryYanYan is not null&&!ValidBoundaryYanYanPrepared(f))throw new InvalidOperationException("Juzhan lost its frozen state epoch or original Slash occurrence.");}
    private IReadOnlyList<ProgramPublicRuleStateSnapshot> BoundaryYanYanPublicStates(CharacterState owner,string skillId)=>skillId!=BoundaryYanYanComposition.Skill?[]:
        GetSkillBindingShard(owner).ProgramInstances.Where(i=>i.SkillId==skillId).SelectMany(i=>new[]{
            new ProgramPublicRuleStateSnapshot(i.SkillInstanceId,"juzhan-state-count",ProgramPublicRuleStateKind.GameActivationProgress,
                ProgramEventHistory<BoundaryYanYanOccurrenceRecordedEvent>().Count(e=>e.Occurrence.OwnerSeat==owner.Seat&&e.Occurrence.SkillInstanceId==i.SkillInstanceId&&e.Occurrence.Epoch==BoundaryYanYanEpoch(owner.Seat))),
            new ProgramPublicRuleStateSnapshot(i.SkillInstanceId,"juzhan-permanent-slash",ProgramPublicRuleStateKind.GameActivationProgress,BoundaryYanYanSlashBonus(owner.Seat))}).ToArray();
}
