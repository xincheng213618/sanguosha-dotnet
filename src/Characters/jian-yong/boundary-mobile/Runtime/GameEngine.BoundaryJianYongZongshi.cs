namespace CardGame.Core;
public sealed partial class GameEngine
{
    private const string BoundaryZongshiReason="skill-program.boundary:zongshih-mobile.obtain";
    private bool TryBeginBoundaryJianYongZongshi(PindianFrame p)=>p.Result is{}r&&TryBeginBoundaryZongshiContest(p,r,-1);
    private bool TryBeginBoundaryJianYongSimultaneousZongshi(ProgramSkillFrame f)=>f.SimultaneousPindian is{ComparisonIssued:true}r&&TryBeginBoundaryZongshiContest(f,SimultaneousResult(f),r.ComparisonIndex);
    private bool BoundaryZongshiNativeResult(ResolutionFrame p,out PindianResult result,out int comparison)
    {
        comparison=-1;result=null!;
        if(p is PindianFrame{Result:{}pair}){result=pair;return true;}
        if(p is ProgramSkillFrame{SimultaneousPindian:{ComparisonIssued:true}r}f&&r.ComparisonIndex<r.Opponents.Count){comparison=r.ComparisonIndex;result=SimultaneousResult(f);return true;}
        return false;
    }
    private bool TryBeginBoundaryZongshiContest(ResolutionFrame supplied,PindianResult result,int comparison)
    {
        var p=_resolutionStack.Single(f=>f.Id==supplied.Id);var r=p.BoundaryJianYongZongshiContest;
        if(r is null||r.Result!=result||r.ComparisonIndex!=comparison)
        {
            if(r is not null&&r.Cursor<r.Candidates.Count)throw new InvalidOperationException("Zongshi cannot replace an unfinished native comparison.");
            var candidates=new[]{result.SourceSeat,result.OpponentSeat}.SelectMany(seat=>CollectProgramTriggerCandidates(_players[seat],SkillProgramTriggerWindow.PindianResultDetermined)).Where(c=>c.SkillId==BoundaryJianYongComposition.Zongshi).ToArray();
            if(candidates.Length==0)return false;
            r=new(p.Id,comparison,result){Candidates=candidates};ReplaceRuntimeTop(p=p with{BoundaryJianYongZongshiContest=r});
        }
        while(r.Cursor<r.Candidates.Count)
        {
            var c=r.Candidates[r.Cursor];var context=BoundaryZongshiContext(p,c);
            if(CanRunProgramTrigger(c,context)){BeginProgramBinding(c,context);return true;}
            ReplaceRuntimeTop(p=p with{BoundaryJianYongZongshiContest=r=r with{Cursor=r.Cursor+1}});
        }
        return false;
    }
    private ProgramSkillWindowContext BoundaryZongshiContext(ResolutionFrame p,ProgramTriggerCandidate c)
    {var r=p.BoundaryJianYongZongshiContest!.Result;return new(SkillProgramTriggerWindow.PindianResultDetermined,p.Id,c.OwnerSeat,SourceSeat:c.OwnerSeat,TargetSeat:r.SourceSeat==c.OwnerSeat?r.OpponentSeat:r.SourceSeat,Facts:null);}
    private bool BoundaryZongshiParent(ProgramSkillFrame f,ResolutionFrame p)=>p.BoundaryJianYongZongshiContest is{}r&&r.ParentFrameId==p.Id&&BoundaryZongshiNativeResult(p,out var result,out var comparison)&&r.Result==result&&r.ComparisonIndex==comparison&&r.Cursor<r.Candidates.Count&&MountObserverCandidateMatches(f,r.Candidates[r.Cursor])&&f.WindowContext==BoundaryZongshiContext(p,r.Candidates[r.Cursor]);
    private bool IsBoundaryJianYongZongshiCandidate(ProgramTriggerCandidate c,ProgramSkillWindowContext context)=>context.Window==SkillProgramTriggerWindow.PindianResultDetermined&&c.SkillId==BoundaryJianYongComposition.Zongshi&&
        _resolutionStack.SingleOrDefault(p=>p.Id==context.ParentFrameId)is{BoundaryJianYongZongshiContest:{}r}parent&&r.ParentFrameId==parent.Id&&BoundaryZongshiNativeResult(parent,out var result,out var comparison)&&r.Result==result&&r.ComparisonIndex==comparison&&
        r.Cursor<r.Candidates.Count&&r.Candidates[r.Cursor]==c&&context==BoundaryZongshiContext(parent,c);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification,56500,SkillProgramEffectOp.ResolveBoundaryJianYongZongshi)]
    private bool BoundaryZongshiQualifies(ProgramTriggerLegalityContext g)=>g.WindowContext is{}context&&IsBoundaryJianYongZongshiCandidate(g.Candidate,context);
    [ProgramPipelineHook(ProgramPipelinePhase.ReturnCompletedProgramBinding,56500)]
    private bool ReturnBoundaryZongshiBinding(ProgramSkillFrame f,bool completed)
    {
        if(_resolutionStack.LastOrDefault()is not{}parent||!BoundaryZongshiParent(f,parent))return false;
        var r=parent.BoundaryJianYongZongshiContest!;ReplaceRuntimeTop(parent=parent with{BoundaryJianYongZongshiContest=r with{Cursor=r.Cursor+1}});
        if(parent is PindianFrame pair){if(!BeginPindianClaims(pair))CompletePindian((PindianFrame)_resolutionStack[^1]);}
        else if(parent is ProgramSkillFrame group)AdvanceRuntimeProgram(group.Id);
        else throw new InvalidOperationException("Zongshi lost its exact original native comparison return.");
        return true;
    }
    private SkillProgramStepOutcome BeginBoundaryJianYongZongshiChoice(ProgramSkillFrame f)
    {
        var n=_resolutionStack.FindIndex(p=>p.Id==f.Id);
        if(n<1||!BoundaryZongshiParent(f,_resolutionStack[n-1])||f.BoundaryJianYongZongshi is not null||f.InstructionIndex!=1)throw new InvalidOperationException("Mobile Zongshi requires the original completed own Pindian and unused choice.");
        var top=EnsureDrawPile()?_cardZones.CardsAt(CardLocation.DrawPile).Last().Id:(int?)null;
        ReplaceRuntimeTop(f=f with{BoundaryJianYongZongshi=new(f.WindowContext!.ParentFrameId,top,ComparisonIndex:_resolutionStack[n-1].BoundaryJianYongZongshiContest!.ComparisonIndex)});PublishBoundaryZongshiChoice(f);return SkillProgramStepOutcome.AwaitChoice;
    }
    private int[] BoundaryZongshiLower(ResolutionFrame p)=>p.BoundaryJianYongZongshiContest is{Result:{}r}?(r.SourceRank==r.OpponentRank?new[]{r.SourceCardId,r.OpponentCardId}:new[]{r.SourceRank<r.OpponentRank?r.SourceCardId:r.OpponentCardId}).Where(id=>_cardZones.GetLocation(id)==CardLocation.Processing).ToArray():[];
    private IReadOnlyList<PromptChoice> BoundaryZongshiChoices(ProgramSkillFrame f)
    {
        var r=f.BoundaryJianYongZongshi!;var p=_resolutionStack.Single(p=>p.Id==r.PindianId);var result=new List<PromptChoice>();
        void Add(string branch,int? id,string label)=>result.Add(new(new($"mobile-zongshi.{f.Id}.{branch}.{id}"),label,id is{}card?[card]:[],[],new Dictionary<string,string>{{"program-action","boundary-jian-yong-zongshi"},{"branch",branch}}));
        if(r.TopCardId is{}top){var card=GetAdvancedCard(top);var suit=card.Suit switch{Suit.Spade=>"♠",Suit.Heart=>"♥",Suit.Club=>"♣",Suit.Diamond=>"♦",_=>""};Add("top",top,$"获得所观看的牌堆顶牌【{card.DisplayName}】（{suit}{card.RankText}）");}
        foreach(var id in BoundaryZongshiLower(p))Add("contest",id,$"获得拼点牌【{GetAdvancedCard(id).DisplayName}】（{(p.BoundaryJianYongZongshiContest!.Result.SourceCardId==id?p.BoundaryJianYongZongshiContest.Result.SourceRank:p.BoundaryJianYongZongshiContest.Result.OpponentRank)}）");
        Add("skip",null,"不获得牌");return Array.AsReadOnly(result.ToArray());
    }
    private void PublishBoundaryZongshiChoice(ProgramSkillFrame f)
    {
        var choices=BoundaryZongshiChoices(f);var skill=_contentRegistry.GetSkill(f.SkillId);
        _pendingDecision=new(DecisionKind.ProgramTrigger,f.OwnerSeat,"纵适：已观看牌堆顶牌，选择获得顶牌或点数较小的拼点牌。",choices.SelectMany(c=>c.Cards).ToArray(),[],f.OwnerSeat){PromptId=CreatePromptId(),IsPrivate=true,Choices=choices,SkillPrompt=new(f.SkillId,skill.Name,skill.Name,skill.Description)};
        _status=_players[f.OwnerSeat].IsHuman?EngineStatus.AwaitingHumanResponse:EngineStatus.Running;
    }
    [ProgramAnswer("boundary-jian-yong-zongshi")]
    private void AnswerBoundaryZongshi(PromptChoice choice)
    {
        var f=(ProgramSkillFrame)_resolutionStack[^1];RequireBoundaryZongshi(f);var r=f.BoundaryJianYongZongshi!;
        if(r.Choice!=0||_pendingDecision?.PlayerSeat!=f.OwnerSeat||!AssistedChoicesEqual([choice],BoundaryZongshiChoices(f).Where(c=>c.Id==choice.Id).ToArray()))throw new InvalidOperationException("Zongshi lost its exact original private peek choice.");
        ClearPendingDecision();if(choice.Cards.Count==0){ReplaceRuntimeTop(f with{BoundaryJianYongZongshi=null});AdvanceRuntimeProgram(f.Id);return;}
        var id=choice.Cards.Single();var from=choice.Parameters["branch"]=="top"?CardLocation.DrawPile:CardLocation.Processing;var before=_movementSequence;
        if(_cardZones.GetLocation(id)!=from||from==CardLocation.DrawPile&&_cardZones.CardsAt(from).Last().Id!=id)throw new InvalidOperationException("Zongshi cannot substitute its viewed top or original contest entity.");
        ReplaceRuntimeTop(f=f with{BoundaryJianYongZongshi=r with{Choice=1,PaidCardId=id,From=from,Before=before,After=before},PendingMovementContinuation=new(f.OwnerSeat,0,null)});
        MoveCard(GetAdvancedCard(id),from,CardLocation.Hand(f.OwnerSeat),new(BoundaryZongshiReason));
        f=GetActiveProgramFrame(f.Id);ReplaceRuntimeTop(f=f with{BoundaryJianYongZongshi=f.BoundaryJianYongZongshi! with{After=_movementSequence}});
        AdvanceEventRulesAndQueueFact(new BoundaryJianYongZongshiPaidEvent(f.Id,r.PindianId,f.OwnerSeat,from.Zone,before,_movementSequence,r.ComparisonIndex));
        if(AwaitProgramBoundCardMovements(f.Id,f.OwnerSeat)==SkillProgramStepOutcome.Continue)FinishBoundaryZongshi(f.Id);
    }
    private void FinishBoundaryZongshi(long id){var f=GetActiveProgramFrame(id);ReplaceRuntimeTop(f with{BoundaryJianYongZongshi=null,PendingMovementContinuation=null});AdvanceRuntimeProgram(id);}
    [ProgramContinuation(ProgramContinuationKind.Resume,56500)]
    private bool ResumeBoundaryZongshi(long id)
    {if(_resolutionStack.LastOrDefault()is not ProgramSkillFrame{BoundaryJianYongZongshi:{}r}f||f.Id!=id)return false;RequireBoundaryZongshi(f);if(r.Choice==0){PublishBoundaryZongshiChoice(f);return true;}if(TryBeginHpChangedProgramWindow(id,PostEventContinuation.Program)||TryBeginCardsMovedProgramWindow(id)||TryBeginAdvancedSkillsChanged(id))return true;FinishBoundaryZongshi(id);return true;}
    [ProgramContinuation(ProgramContinuationKind.Movement,56500)]private bool ReturnBoundaryZongshiMovement(ProgramSkillFrame f)=>f.BoundaryJianYongZongshi is not null&&ResumeBoundaryZongshi(f.Id);
    private void RequireBoundaryZongshi(ProgramSkillFrame f)
    {
        var n=_resolutionStack.FindIndex(p=>p.Id==f.Id);var r=f.BoundaryJianYongZongshi;
        if(r is null||f.SkillId!=BoundaryJianYongComposition.Zongshi||f.InstructionIndex!=1||n<1||!BoundaryZongshiParent(f,_resolutionStack[n-1])||r.PindianId!=f.WindowContext!.ParentFrameId||r.Choice is <0 or >1)throw new InvalidOperationException("Zongshi lost its original Pindian, exact participant source or stage.");
        if(r.Choice==0&&r.TopCardId is{}viewed&&(_cardZones.GetLocation(viewed)!=CardLocation.DrawPile||_cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault()?.Id!=viewed))throw new InvalidOperationException("Zongshi lost its original privately viewed deck top before choosing.");
        var parent=_resolutionStack[n-1];var pair=parent.BoundaryJianYongZongshiContest!.Result;
        if(r.ComparisonIndex!=parent.BoundaryJianYongZongshiContest.ComparisonIndex)throw new InvalidOperationException("Zongshi lost its exact original simultaneous comparison index.");
        var comparisons=ProgramEventHistory<PindianResultDeterminedEvent>().Where(e=>e.FrameId==parent.Id).ToArray();
        var comparison=r.ComparisonIndex<0?0:r.ComparisonIndex;
        if(comparison>=comparisons.Length||comparisons[comparison].Result!=pair)throw new InvalidOperationException("Zongshi lost the actual native public result for its original comparison.");
        var lower=pair.SourceRank==pair.OpponentRank?new[]{pair.SourceCardId,pair.OpponentCardId}:new[]{pair.SourceRank<pair.OpponentRank?pair.SourceCardId:pair.OpponentCardId};
        if(r.Choice==1&&(r.From==CardLocation.DrawPile?r.PaidCardId!=r.TopCardId:r.From!=CardLocation.Processing||r.PaidCardId is not{}claimed||!lower.Contains(claimed)))throw new InvalidOperationException("Zongshi payment must be its viewed top or exact lower/equal final-rank contest entity.");
        if(r.Choice==1&&(r.PaidCardId is not{}id||r.From is not{}from||r.After<=r.Before||r.After>_movementSequence||_cardMovements.Count(m=>m.Sequence>r.Before&&m.Sequence<=r.After&&m.CardId==id&&m.From==from&&m.To==CardLocation.Hand(f.OwnerSeat)&&m.Reason.Value==BoundaryZongshiReason)!=1||
            ProgramEventHistory<BoundaryJianYongZongshiPaidEvent>().Count(e=>e==new BoundaryJianYongZongshiPaidEvent(f.Id,r.PindianId,f.OwnerSeat,from.Zone,r.Before,r.After,r.ComparisonIndex))!=1))throw new InvalidOperationException("Zongshi lost its exact once-paid native Obtain.");
    }
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost,56500)] [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments,56500)] [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment,56500)]
    private bool BoundaryZongshiMovementProof(ProgramSkillFrame f,SkillProgramEffect? e,ProgramMovementContinuation p)=>f.BoundaryJianYongZongshi is{Choice:1}&&e?.Op==SkillProgramEffectOp.ResolveBoundaryJianYongZongshi&&f.PendingMovementContinuation==p&&p.SubjectSeat==f.OwnerSeat&&p.BeforeCount==0&&p.CoverageResultBind is null;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState,56500)]private void AssertBoundaryZongshi(ProgramSkillFrame f){if(f.BoundaryJianYongZongshi is not null)RequireBoundaryZongshi(f);}
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss,56500)] [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath,56500)]private bool KeepBoundaryZongshiPaid(ProgramSkillFrame f){if(f.BoundaryJianYongZongshi is not{Choice:1})return false;RequireBoundaryZongshi(f);return true;}
    private bool IsBoundaryJianYongZongshiSubtree(PindianFrame p)
    {
        var n=_resolutionStack.FindIndex(f=>f.Id==p.Id);if(n<0||n+1>=_resolutionStack.Count||_resolutionStack[n+1]is not ProgramSkillFrame f||!BoundaryZongshiParent(f,p))return false;
        if(f.BoundaryJianYongZongshi is not null)RequireBoundaryZongshi(f);
        return n+2==_resolutionStack.Count||BoundaryZongshiFirstChild(f,_resolutionStack[n+2])&&BoundaryZongshiSuffix(n+1);
    }
    private bool BoundaryZongshiFirstChild(ProgramSkillFrame f,ResolutionFrame child)
    {
        if(f.BoundaryJianYongZongshi is not{Choice:1}r||child is not CardsMovedTriggerWindowFrame moved||!ProgramMovementReturnMatches(f,moved)||moved.Batch.ParentFrameId!=f.Id)return false;
        RequireBoundaryZongshi(f);return moved.Batch.OriginOwnerSeat==f.OwnerSeat&&moved.Batch.OriginSkillId==f.SkillId&&moved.Batch.OriginSkillInstanceId==f.SkillInstanceId&&moved.Batch.Movements.Count>0&&moved.Batch.Movements.All(m=>_cardMovements.Contains(m)&&m.Sequence>r.Before&&m.Sequence<=r.After);
    }
    private bool BoundaryZongshiSuffix(int n)=>HasObserverSuffix(n,static(e,i)=>e.IsMovementObserverEdge(i)||e.IsSameNameHandObserverEdge(i)||e.IsYanPuDyingExitEdge(i)||e.TypedDamagePostEventEdge(e._resolutionStack[i-1],e._resolutionStack[i])||e.RecoveryReplacementFrameRidesOn(e._resolutionStack[i],e._resolutionStack[i-1]),static(e,i,d)=>e.IsSuspendedOrPaidRescueObserverSuffix(i,d));
    private ProgramSkillFrame? BoundaryZongshiObserverRoot()=>FindProgramObserverRoot(static(e,f,c)=>e.BoundaryZongshiFirstChild(f,c),static(e,n)=>e.BoundaryZongshiSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage,56500)] [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage,56500)]
    private bool BoundaryZongshiDamageChild(long id)=>BoundaryZongshiObserverRoot()is{}f&&_resolutionStack.FindIndex(x=>x.Id==id&&x is DamageTriggerWindowFrame)>_resolutionStack.FindIndex(x=>x.Id==f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage,56500)]
    private bool BoundaryZongshiBeforeDamageChild(long id)=>BoundaryZongshiObserverRoot()is{}f&&_resolutionStack.FindIndex(x=>x.Id==id&&x is BeforeDamageProgramWindowFrame)>_resolutionStack.FindIndex(x=>x.Id==f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant,56500)] [ProgramDyingProof(ProgramDyingProofPhase.Runtime,56500)]
    private bool BoundaryZongshiDyingChild()=>ActiveDying is{}d&&BoundaryZongshiObserverRoot()is{}f&&_resolutionStack.FindIndex(x=>x.Id==d.Id)>_resolutionStack.FindIndex(x=>x.Id==f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth,56500)]
    private bool AdvanceBoundaryZongshiChild()=>_pendingDecision is null&&BoundaryZongshiObserverRoot()is not null&&TryAdvanceObserverSubtree(true);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryJianYongZongshi)]private PromptChoice SelectAiBoundaryZongshi(PendingDecision p)=>p.Choices.FirstOrDefault(c=>c.Parameters.GetValueOrDefault("branch")=="top")??p.Choices.First();
}
