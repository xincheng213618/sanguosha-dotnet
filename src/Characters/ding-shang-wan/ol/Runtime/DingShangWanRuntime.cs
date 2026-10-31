using System.Globalization;
namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed partial class DingShangWanRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramRuntimePrompts prompts, IDingShangWanRuntimeQueries proofs, IDingShangWanNativeFacts facts,
    IDingShangWanNativeOperations native, IMatchParticipantStateAccess participants, ICardZoneQueries zones,
    ICardMovementHistoryQueries movements, ICardMovementBatchQueue batches, IOwnedCardMoveQueries ownedMoves,
    IProgramTurnQueries turn, IProgramContentQueries content, INativeDrawProducerOperations draws,
    IProgramOperationDraws operationDraws, INativeLogicalDiscardOperations discards,
    IProgramHealthChildOperations healthOperations, IProgramEventHistoryQueries history,
    IProgramSkillCompletion completion, IProgramChildWindows children) : IDingShangWanHost
{
    private const string DingShangWanDrawReason="skill-program.ding-shang-wan.draw",FudaoGiftReason="skill-program.ding-shang-wan.fudao.give",        FudaoDiscardReason=global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.FudaoDiscardReason,FengyanGiftReason="skill-program.ding-shang-wan.fengyan.give",FengyanDiscardReason=global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.FengyanDiscardReason;
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame,SkillProgramEffectOp operation)=>BeginDingShangWan(frame,operation);
    private long PhysicalMovementSequence=>movements.Last()?.Sequence??0;
    private Card Card(int id)=>zones.CardsAt(zones.GetLocation(id)).Single(card=>card.Id==id);
    private void PublishChoice(ProgramSkillFrame frame,int chooser,IReadOnlyList<PromptChoice> choices,string text,int target)
    {
        var skill=content.Skill(frame.SkillId);
        prompts.Set(new(DecisionKind.ProgramTrigger,chooser,text,Array.AsReadOnly(choices.SelectMany(c=>c.Cards).Distinct().ToArray()),[],frame.OwnerSeat)
        {PromptId=prompts.NextId(),IsPrivate=true,TargetSeat=target,Choices=choices,
            RequiredCardCount=frame.PrivateHandTake is{Paid:false}take?take.RequiredCount-take.SelectedCardIds.Count:0,
            SkillPrompt=new(frame.SkillId,skill.Name,skill.Name,skill.Description)},participants.At(chooser).IsHuman);
    }
    private SkillProgramStepOutcome BeginDingShangWan(ProgramSkillFrame supplied,SkillProgramEffectOp op)
    {
        var f=frames.ActiveProgram(supplied.Id);if(f.DingShangWan is not null||f.InstructionIndex!=1||f.TriggerId is null||f.WindowContext is not{}c||
            !participants.At(f.OwnerSeat).IsAlive||facts.Winner!=Winner.None||!facts.HasSkillInstance(f.OwnerSeat,f.SkillId,f.SkillInstanceId)||
            !proofs.CanOfferDingShangWan(new(f.OwnerSeat,f.SkillId,f.TriggerId,f.SkillInstanceId,f.GameplayHash,facts.Trigger(f).Priority,c.OccurrenceIndex),facts.Trigger(f),c))
            throw new InvalidOperationException("Ding Shang Wan requires its exact current unpaid native candidate.");
        var counterpart=op==SkillProgramEffectOp.ResolveFengyanCounterpart?proofs.DingShangWanCounterpart(f.OwnerSeat,c):op==SkillProgramEffectOp.DrawFudaoRecordedPair?c.TargetSeat:null;
        var stage=op==SkillProgramEffectOp.InitializeFudaoHandRecord?DingShangWanStage.OpeningDraw:op==SkillProgramEffectOp.DrawFudaoRecordedPair?DingShangWanStage.PairOwnerDraw:DingShangWanStage.FengChoice;
        var r=new DingShangWanReceipt(proofs.DingShangWanSource(f),f.GameplayHash,1,c.ParentFrameId,c.Window,op,turn.TurnNumber,turn.TurnOwnerSeat,stage,counterpart,c.DamageFrameId,c.CardUse?.CardActionId);
        frames.ReplaceTop(f=f with{DingShangWan=r});
        events.AdvanceFact(new DingShangWanStartedEvent(f.Id,r.Source,r.GameplayHash,r.WindowId,r.Window,r.Operation,r.Turn,r.TurnOwner,r.Counterpart,r.DamageFrameId,r.ActionId));
        if(op==SkillProgramEffectOp.ResolveFengyanCounterpart)PublishDingShangWan(f);
        else BeginDingShangWanDraw(f,f.OwnerSeat,op==SkillProgramEffectOp.InitializeFudaoHandRecord?3:counterpart==f.OwnerSeat?2:1,stage);
        return SkillProgramStepOutcome.AwaitChild;
    }
    private bool DingShangWanCanPay(int seat,Card card,CardLocation from,bool gift)=>!card.IsGeneralWeapon&&from.OwnerSeat==seat&&from.Zone is(CardZoneKind.Hand or CardZoneKind.Equipment)&&
        !ownedMoves.IsForeignEquipmentDiscardPrevented(seat,card,from,gift?OwnedCardMoveIntent.Transfer:OwnedCardMoveIntent.Discard)&&
        (gift||!ownedMoves.IsSelfHandCategoryDiscardForbidden(seat,card,from,OwnedCardMoveIntent.Discard));
    private int DingShangWanChooser(ProgramSkillFrame f)=>f.DingShangWan!.Stage==DingShangWanStage.FengDiscardChoice?f.DingShangWan.Counterpart!.Value:f.OwnerSeat;
    private IReadOnlyList<PromptChoice> DingShangWanChoices(ProgramSkillFrame f)
    {
        var r=f.DingShangWan!;var result=new List<PromptChoice>();
        PromptChoice C(string branch,string label,int? card=null,int? target=null)=>new(new($"ding-shang-wan.{f.Id}.{r.Stage}.{branch}.{card}.{target}.{r.Selected.Count}"),label,card is{}id?[id]:[],target is{}seat?[seat]:[],new Dictionary<string,string>{{"program-action","ding-shang-wan"},{"frame-id",f.Id.ToString(CultureInfo.InvariantCulture)},{"branch",branch}});
        if(r.Stage==DingShangWanStage.FengChoice)
        {result.Add(C("owner-draw","摸一张牌并交给其一张牌"));result.Add(C("other-draw","其摸一张牌并弃置两张牌"));return Array.AsReadOnly(result.ToArray());}
        var payer=DingShangWanChooser(f);if(!participants.At(payer).IsAlive||facts.Winner!=Winner.None)return [];
        if(r.Stage==DingShangWanStage.OpeningGiftChoice)
            foreach(var p in participants.Players.Where(p=>p.IsAlive&&p.Seat!=f.OwnerSeat))result.Add(C("recipient",$"交给{p.Name}",target:p.Seat));
        if(r.Stage is not(DingShangWanStage.OpeningGiftChoice or DingShangWanStage.OpeningDiscardChoice or DingShangWanStage.FengGiftChoice or DingShangWanStage.FengDiscardChoice))return [];
        var gift=r.Stage is DingShangWanStage.OpeningGiftChoice or DingShangWanStage.FengGiftChoice;
        foreach(var id in r.Candidates.Where(id=>!r.Selected.Contains(id)&&r.Selected.Count<r.Maximum&&DingShangWanCanPay(payer,Card(id),zones.GetLocation(id),gift)&&
            (r.Stage is not(DingShangWanStage.OpeningGiftChoice or DingShangWanStage.OpeningDiscardChoice)||zones.GetLocation(id)==CardLocation.Hand(payer))))
            result.Add(C("pick",$"选择【{Card(id).DisplayName}】",id));
        foreach(var id in r.Selected)result.Add(C("unpick",$"撤回【{Card(id).DisplayName}】",id));
        if(r.Selected.Count>=r.Minimum&&(r.Stage!=DingShangWanStage.OpeningGiftChoice||r.Selected.Count==0||r.Recipient is{}recipient&&participants.At(recipient).IsAlive))
            result.Add(C("finish",r.Stage==DingShangWanStage.OpeningGiftChoice&&r.Selected.Count==0?"不交牌，继续弃牌":"完成选择"));
        return Array.AsReadOnly(result.ToArray());
    }
    private void PublishDingShangWan(ProgramSkillFrame f)
    {
        var choices=DingShangWanChoices(f);if(choices.Count==0){FinishDingShangWan(f);return;}
        PublishChoice(f,DingShangWanChooser(f),choices,"请选择实际结算。",f.DingShangWan!.Counterpart??f.DingShangWan.Recipient??f.OwnerSeat);
    }
    private void BeginDingShangWanSelection(ProgramSkillFrame f,DingShangWanStage stage)
    {
        var r=f.DingShangWan!;var payer=stage==DingShangWanStage.FengDiscardChoice?r.Counterpart!.Value:f.OwnerSeat;
        if(!participants.At(payer).IsAlive||facts.Winner!=Winner.None||stage==DingShangWanStage.FengGiftChoice&&(r.Counterpart is not{}target||!participants.At(target).IsAlive)){FinishDingShangWan(f);return;}
        var gift=stage is DingShangWanStage.OpeningGiftChoice or DingShangWanStage.FengGiftChoice;
        var handOnly=stage is DingShangWanStage.OpeningGiftChoice or DingShangWanStage.OpeningDiscardChoice;
        var cards=(handOnly?zones.CardsAt(CardLocation.Hand(payer)):zones.CardsAt(CardLocation.Hand(payer)).Concat(zones.CardsAt(CardLocation.Equipment(payer)))).Where(card=>DingShangWanCanPay(payer,card,zones.GetLocation(card.Id),gift)).Select(c=>c.Id).ToArray();
        var min=stage==DingShangWanStage.FengGiftChoice?Math.Min(1,cards.Length):stage==DingShangWanStage.FengDiscardChoice?Math.Min(2,cards.Length):0;
        var max=stage==DingShangWanStage.OpeningGiftChoice?Math.Min(3,cards.Length):stage==DingShangWanStage.FengGiftChoice?Math.Min(1,cards.Length):stage==DingShangWanStage.FengDiscardChoice?Math.Min(2,cards.Length):cards.Length;
        frames.ReplaceTop(f=f with{DingShangWan=r with{Stage=stage,Payer=payer,Minimum=min,Maximum=max,Candidates=cards,Selected=[]}});PublishDingShangWan(f);
    }
    [ProgramAnswer("ding-shang-wan")]
    private void AnswerDingShangWan(PromptChoice choice)
    {
        if(frames.Top is not ProgramSkillFrame{DingShangWan:{}r}f||prompts.Current?.PlayerSeat!=DingShangWanChooser(f)||
            !RuntimeSkills.RuntimeRules_Core_AssistedPhysicalCards.AssistedChoicesEqual([choice],DingShangWanChoices(f).Where(c=>c.Id==choice.Id).ToArray()))throw new InvalidOperationException("Ding Shang Wan lost its actual private chooser or selected physical entities.");
        proofs.AssertDingShangWan(f);prompts.Clear();var branch=choice.Parameters["branch"];
        if(r.Stage==DingShangWanStage.FengChoice)
        {
            events.AdvanceFact(new DingShangWanBranchEvent(f.Id,branch,r.Counterpart));frames.ReplaceTop(f=f with{DingShangWan=r with{Branch=branch}});
            BeginDingShangWanDraw(f,branch=="owner-draw"?f.OwnerSeat:r.Counterpart!.Value,1,branch=="owner-draw"?DingShangWanStage.FengOwnerDraw:DingShangWanStage.FengTargetDraw);return;
        }
        if(branch=="recipient"){frames.ReplaceTop(f=f with{DingShangWan=r with{Recipient=choice.Targets.Single()}});PublishDingShangWan(f);return;}
        if(branch!="finish")
        {var id=choice.Cards.Single();frames.ReplaceTop(f=f with{DingShangWan=r with{Selected=branch=="pick"?r.Selected.Append(id).ToArray():r.Selected.Where(x=>x!=id).ToArray()}});PublishDingShangWan(f);return;}
        var stage=r.Stage switch{DingShangWanStage.OpeningGiftChoice=>DingShangWanStage.OpeningGiftChildren,DingShangWanStage.OpeningDiscardChoice=>DingShangWanStage.OpeningDiscardChildren,DingShangWanStage.FengGiftChoice=>DingShangWanStage.FengGiftChildren,_=>DingShangWanStage.FengDiscardChildren};
        var gift=stage is DingShangWanStage.OpeningGiftChildren or DingShangWanStage.FengGiftChildren;var recipient=stage==DingShangWanStage.OpeningGiftChildren?r.Recipient:r.Counterpart;
        var grain=r.Selected.Any(id=>zones.GetLocation(id)==CardLocation.Equipment(r.Payer)&&Card(id).Kind==CardKind.WoodenOx)
            ?zones.CardsAt(CardLocation.WoodenOxGrain(r.Payer)).Select(c=>c.Id).ToArray():[];
        var before=facts.MovementSequence;frames.ReplaceTop(f=f with{DingShangWan=r with{Stage=stage,Before=before,After=before,Recipient=recipient,WoodenOxGrainIds=grain,Reshuffles=[]},PendingMovementContinuation=new(r.Payer,0,null)});
        if(r.Selected.Count>0)
        {
            var reason=new CardMoveReason(stage switch{DingShangWanStage.OpeningGiftChildren=>FudaoGiftReason,DingShangWanStage.OpeningDiscardChildren=>FudaoDiscardReason,DingShangWanStage.FengGiftChildren=>FengyanGiftReason,_=>FengyanDiscardReason});
            if(gift)native.Give(f.Id,r.Payer,recipient!.Value,r.Selected,reason);
            else discards.MoveProgram(f,r.Payer,r.Selected,reason);
        }
        RecordDingShangWanMovement(f.Id);ContinueDingShangWan(f.Id);
    }
    private void BeginDingShangWanDraw(ProgramSkillFrame f,int seat,int count,DingShangWanStage stage)
    {
        if(!participants.At(seat).IsAlive||facts.Winner!=Winner.None){FinishDingShangWan(f);return;}
        if(stage==DingShangWanStage.OpeningDraw){BeginDingShangWanOpeningDraw(f,seat,count);return;}
        var before=facts.MovementSequence;frames.ReplaceTop(f=f with{DingShangWan=f.DingShangWan! with{Stage=stage,Payer=seat,Before=before,After=before,Candidates=[],Selected=[],WoodenOxGrainIds=[],Reshuffles=[]},PendingMovementContinuation=new(seat,0,null)});
        draws.DrawProgramCards(f.Id,seat,count,null,SkillProgramCardSetVisibility.Private,new(DingShangWanDrawReason));RecordDingShangWanMovement(f.Id);ContinueDingShangWan(f.Id);
    }
    private void RecordDingShangWanMovement(long id)
    {
        var f=frames.ActiveProgram(id);var r=f.DingShangWan!;var after=facts.MovementSequence;
        var count=movements.Read().Count(m=>m.Sequence>r.Before&&m.Sequence<=after&&(!IsDingShangWanDrawStage(r.Stage)||m.From==CardLocation.DrawPile&&m.To==CardLocation.Hand(r.Payer)&&m.Reason.Value==DingShangWanDrawReason));
        var shuffles=IsDingShangWanDrawStage(r.Stage)?Enumerable.Range(0,batches.PendingBatchCount).Select(batches.PendingBatchAt).Where(b=>b.ParentFrameId==id&&b.ParentBatchId is null&&
            (b.AwaitingProgramFrameId==id||r.Stage==DingShangWanStage.OpeningDraw&&r.OpeningDrawOutcome is not null&&b.AwaitingProgramFrameId is null)&&b.TurnNumber==r.Turn&&
            b.OriginOwnerSeat==f.OwnerSeat&&b.OriginSkillId==f.SkillId&&b.OriginSkillInstanceId==f.SkillInstanceId&&b.Movements.Count>0&&
            b.Movements.All(m=>m.Sequence>r.Before&&m.Sequence<=after&&m.From==CardLocation.DiscardPile&&m.To==CardLocation.DrawPile&&m.Reason==CardMoveReasons.Reshuffle&&movements.Read().Contains(m))&&
            b.SourceCounts is [{Location:var source,CountBefore:var discarded,CountAfter:0}]&&source==CardLocation.DiscardPile&&discarded==b.Movements.Count&&
            b.DestinationCounts is [{Location:var destination,CountBefore:0,CountAfter:var replenished}]&&destination==CardLocation.DrawPile&&replenished==b.Movements.Count)
            .Select(b=>new DingShangWanReshuffle(b.Id,id,b.Movements[0].Sequence-1,b.Movements[^1].Sequence){Movements=b.Movements}).ToArray():[];
        frames.ReplaceTop(f with{DingShangWan=r with{After=after,ActualCount=count,Reshuffles=shuffles}});
        foreach(var shuffle in shuffles)events.AdvanceFact(new DingShangWanReshuffleCapturedEvent(id,shuffle.BatchId,shuffle.Before,shuffle.After,shuffle.Movements.Count));
        events.AdvanceFact(new DingShangWanMovementIssuedEvent(id,r.Stage,r.Payer,r.Recipient,r.Before,after,count));
    }
    private static bool IsDingShangWanDrawStage(DingShangWanStage stage) =>
        global::CardGame.Core.RuntimeSkills.RuntimeRules_ding_shang_wan_ol_DingShangWan.IsDingShangWanDrawStage(stage);
    private bool DrainDingShangWan(ProgramSkillFrame f)=>children.Begin(f.Id,PostEventContinuation.AwaitedProgramMovement);
    private void ContinueDingShangWan(long id)
    {
        if(frames.Top is not ProgramSkillFrame{DingShangWan:{}r}f||f.Id!=id)return;
        proofs.AssertDingShangWan(f);
        if(r.Stage is DingShangWanStage.OpeningGiftChoice or DingShangWanStage.OpeningDiscardChoice or DingShangWanStage.FengChoice or DingShangWanStage.FengGiftChoice or DingShangWanStage.FengDiscardChoice){if(prompts.Current is null)PublishDingShangWan(f);return;}
        if(DrainDingShangWan(f))return;frames.ReplaceTop(f=f with{PendingMovementContinuation=null});
        if(facts.Winner!=Winner.None||!participants.At(f.OwnerSeat).IsAlive){FinishDingShangWan(f);return;}
        switch(r.Stage)
        {
            case DingShangWanStage.OpeningDraw:BeginDingShangWanSelection(f,DingShangWanStage.OpeningGiftChoice);return;
            case DingShangWanStage.OpeningGiftChildren:BeginDingShangWanSelection(f,DingShangWanStage.OpeningDiscardChoice);return;
            case DingShangWanStage.OpeningDiscardChildren:
                events.AdvanceFact(new FudaoHandRecordedEvent(f.Id,r.Source,r.GameplayHash,zones.CardsAt(CardLocation.Hand(f.OwnerSeat)).Count));FinishDingShangWan(f);return;
            case DingShangWanStage.PairOwnerDraw:
                if(r.Counterpart!=f.OwnerSeat&&r.Counterpart is{}recipient&&participants.At(recipient).IsAlive){BeginDingShangWanDraw(f,recipient,1,DingShangWanStage.PairTargetDraw);return;}break;
            case DingShangWanStage.FengOwnerDraw:BeginDingShangWanSelection(f,DingShangWanStage.FengGiftChoice);return;
            case DingShangWanStage.FengTargetDraw:BeginDingShangWanSelection(f,DingShangWanStage.FengDiscardChoice);return;
        }
        FinishDingShangWan(f);
    }
    private void FinishDingShangWan(ProgramSkillFrame f)
    {events.AdvanceFact(new DingShangWanCompletedEvent(f.Id,true));frames.ReplaceTop(f=f with{DingShangWan=null,PendingMovementContinuation=null});completion.Finish(f,true);}
    [ProgramContinuation(ProgramContinuationKind.Resume,26700)]private bool ResumeDingShangWan(long id){if(frames.Top is not ProgramSkillFrame{DingShangWan:not null}f||f.Id!=id)return false;ContinueDingShangWan(id);return true;}
    [ProgramContinuation(ProgramContinuationKind.Movement,26700)]private bool ReturnDingShangWan(ProgramSkillFrame f){if(f.DingShangWan is null)return false;ContinueDingShangWan(f.Id);return true;}
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 160)]
    private bool IsDingShangWanAwaitedMovement(ProgramSkillFrame f,SkillProgramEffect? e,ProgramMovementContinuation p)=>f.DingShangWan is{}r&&p.SubjectSeat==r.Payer&&p.BeforeCount==0&&p.CoverageResultBind is null&&(e is null||DingShangWanComposition.IsOperation(e.Op));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss,26700)][ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath,26700)]
    private bool ContinuePaidDingShangWan(ProgramSkillFrame f){if(f.DingShangWan is null)return false;proofs.AssertDingShangWan(f);return true;}
    [ProgramAiChoice(SkillProgramEffectOp.InitializeFudaoHandRecord,SkillProgramEffectOp.DrawFudaoRecordedPair,SkillProgramEffectOp.ResolveFengyanCounterpart)]
    private PromptChoice AiDingShangWan(PendingDecision p,ProgramSkillFrame f)=>p.Choices.FirstOrDefault(c=>c.Parameters.GetValueOrDefault("branch")=="finish")??p.Choices.FirstOrDefault(c=>c.Parameters.GetValueOrDefault("branch")=="other-draw")??p.Choices.First(c=>c.Parameters.GetValueOrDefault("branch")!="unpick");
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth,26700)]private bool AdvanceDingShangWanSubtree()=>prompts.Current is null&&proofs.DingShangWanObserverRoot()is not null&&healthOperations.AdvanceObserverSubtree();
}
