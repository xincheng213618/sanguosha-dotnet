namespace CardGame.Core;
[ProgramRuntimeModule,ProgramCapability]
internal sealed partial class QuHuangRuntime(IProgramRuntimeFrames frames,IProgramRuntimeEvents events,
    IProgramRuntimePrompts prompts,IProgramEventHistoryQueries history,ICardMovementHistoryQueries movements,
    ICardZoneQueries zones,IProgramContentQueries content,IMatchParticipantStateAccess participants,
    IPhysicalCardDrawOperations draws,IProgramBoundCardMovementOperations movement,IProgramBoundCardFacts bound,
    INativeLogicalDiscardOperations logicalDiscards,INativeLogicalDiscardMovement nativeMovement,
    IActualDiscardRecoveryPhaseQueries phases,IActualPhaseEndQueries phaseEnds,IProgramBindingFacts bindings,
    ICardsMovedWindowFacts triggers,IProgramCardSnapshotQueries snapshots,IQuHuangFacts facts,
    IQuHuangNativeQueries native,IQuHuangNativeOperations operations,IProgramHealthChildOperations children,
    IQuHuangPhaseEndQueries phaseLedger,ISkillGrantRevisionObservations observations,
    IProgramSkillCompletion completion,IProgramChildWindows childWindows):IQuHuangHost
{
    private const string QiejianDrawReason="skill-program.qu-huang.qiejian.draw",QiejianDiscardReason="skill-program.qu-huang.qiejian.discard",NishouHoldReason="skill-program.qu-huang.nishou.hold",NishouGiveReason="skill-program.qu-huang.nishou.give",NishouCleanupReason="skill-program.qu-huang.nishou.cleanup";
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame supplied,SkillProgramEffectOp op)
    {
        var f=frames.ActiveProgram(supplied.Id);var c=f.WindowContext??throw new InvalidOperationException("Qu Huang requires a genuine native trigger.");var candidate=new ProgramTriggerCandidate(f.OwnerSeat,f.SkillId,GetProgramBindingId(f),f.SkillInstanceId,f.GameplayHash,GetProgramTrigger(f).Priority,c.OccurrenceIndex);
        if(f.QuHuang is not null||f.InstructionIndex!=1||f.SelectedCardIds.Count!=0||f.SelectedTargetSeats.Count!=0||!_players[f.OwnerSeat].IsAlive||(op==SkillProgramEffectOp.ResolveNishouIssuedCurrentPhaseSwap?!IsIssuedNishouCandidate(candidate,c):!QuHuangMovementCandidateCurrent(candidate,c)))throw new InvalidOperationException("Qu Huang lost his original native source, movement or current phase entitlement.");
        var target=op==SkillProgramEffectOp.ResolveQiejianLastHandLoss?c.SourceSeat!.Value:f.OwnerSeat;
        var stage=op==SkillProgramEffectOp.ResolveQiejianLastHandLoss?QuHuangStage.DrawOwner:op==SkillProgramEffectOp.ResolveNishouEquipmentLossChoice?QuHuangStage.NishouChoice:QuHuangStage.SwapTarget;
        var r=new QuHuangReceipt(QuHuangSource(f),f.GameplayHash,op,c.ParentFrameId,_turnNumber,facts.TurnOwnerSeat,stage,target);
        if(op==SkillProgramEffectOp.ResolveNishouEquipmentLossChoice)r=r with{Equipment=ProgramEventHistory<NishouEquipmentLostEvent>().Single(e=>e.BatchId==c.MovementBatch!.Id&&e.OwnerSeat==f.OwnerSeat)};
        if(op==SkillProgramEffectOp.ResolveNishouIssuedCurrentPhaseSwap)r=r with{Paid=true,DebtId=c.ActualPhaseEnd!.EntitlementId};
        frames.ReplaceTop(f=f with{QuHuang=r});events.AdvanceFact(new QuHuangStartedEvent(f.Id,r.Source,r.GameplayHash,op,r.ParentId,r.ActualTurn,target));
        if(r.DebtId is{}debt)events.AdvanceFact(new NishouSwapDebtConsumedEvent(debt,f.Id,c.ActualPhaseEnd!.BoundaryFrameId));
        if(stage==QuHuangStage.DrawOwner)PayQuHuangDraw(f,f.OwnerSeat,QuHuangStage.DrawOwner);else ContinueQuHuang(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
    private PromptChoice QuHuangChoice(ProgramSkillFrame f,string branch,string label,int? card=null,int? seat=null)=>new(new($"qu-huang.{f.Id}.{branch}.{card??seat??-1}"),label,card is{}id?[id]:[],seat is{}s?[s]:[],new Dictionary<string,string>{{"program-action","qu-huang"},{"branch",branch}});
    private IReadOnlyList<PromptChoice> QuHuangChoices(ProgramSkillFrame f)
    {
        var r=f.QuHuang!;var choices=new List<PromptChoice>();
        if(r.Stage==QuHuangStage.QiejianChoice)
        {choices.AddRange(QiejianFieldCards(f).Select(c=>QuHuangChoice(f,"discard",$"弃置场上【{GetAdvancedCard(c.CardId).DisplayName}】",c.CardId,c.From.OwnerSeat)));choices.Add(QuHuangChoice(f,"block","本轮不再对其发动切谏"));}
        if(r.Stage==QuHuangStage.NishouChoice)
        {if(CanIssueQuHuangNishouLightning(f))choices.Add(QuHuangChoice(f,"lightning","将原装备牌当闪电使用",r.Equipment!.CardId));if(NishouSwapAvailable(f.OwnerSeat,r.Equipment?.Phase))choices.Add(QuHuangChoice(f,"defer","本阶段结束时交换手牌"));}
        if(r.Stage==QuHuangStage.SwapTarget)choices.AddRange(NishouMinimumTargets(f.OwnerSeat).Select(s=>QuHuangChoice(f,"swap","与其交换本次原手牌",seat:s)));
        return Array.AsReadOnly(choices.ToArray());
    }
    private void PublishQuHuang(ProgramSkillFrame f)
    {
        var choices=QuHuangChoices(f);if(choices.Count==0){FinishQuHuang(f);return;}var skill=content.Skill(f.SkillId);
        frames.ReplaceTop(f with{Step=ResolutionFrameStep.AwaitingResponse});prompts.Set(new(DecisionKind.ProgramTrigger,f.OwnerSeat,skill.Name+"：选择本次结算。",choices.SelectMany(c=>c.Cards).Distinct().ToArray(),choices.SelectMany(c=>c.Targets).Distinct().ToArray(),f.OwnerSeat){PromptId=prompts.NextId(),IsPrivate=true,Choices=choices,SkillPrompt=new(f.SkillId,skill.Name,skill.Name,skill.Description)},_players[f.OwnerSeat].IsHuman);
        operations.PublishState();
    }
    [ProgramAnswer("qu-huang")]private void AnswerQuHuang(PromptChoice supplied)
    {
        if(_resolutionStack.LastOrDefault()is not ProgramSkillFrame{QuHuang:{}r}f||prompts.Current?.PlayerSeat!=f.OwnerSeat||!QuHuangChoices(f).Any(c=>AssistedChoicesEqual([c],[supplied])))throw new InvalidOperationException("Qu Huang requires his exact private current entity/branch answer.");
        AssertQuHuang(f);prompts.Clear();frames.ReplaceTop(f=f with{Step=ResolutionFrameStep.ResolvingEffect});
        if(!_players[f.OwnerSeat].IsAlive||_winner!=Winner.None){ContinueQuHuang(f.Id);return;}
        var branch=supplied.Parameters["branch"];
        if(branch=="block"){events.AdvanceFact(new QiejianRoundBlockedEvent(f.Id,f.OwnerSeat,r.TargetSeat,_roundNumber));FinishQuHuang(f);return;}
        if(branch=="discard")
        {var card=QiejianFieldCards(f).Single(c=>c.CardId==supplied.Cards.Single());var logical=logicalDiscards.Capture(f.OwnerSeat,f.OwnerSeat,[card.CardId],new(QiejianDiscardReason),f.Id);PayQuHuangCards(f,QuHuangStage.FieldChildren,[card],CardLocation.DiscardPile,new(QiejianDiscardReason));logicalDiscards.Complete(logical);CompleteQuHuangMovement(f.Id);return;}
        if(branch=="defer")
        {var phase=r.Equipment!.Phase!;frames.ReplaceTop(f=f with{QuHuang=r with{Paid=true}});events.AdvanceFact(new NishouSwapDebtIssuedEvent(f.Id,r.Source,r.GameplayHash,phase));FinishQuHuang(f);return;}
        if(branch=="lightning"){IssueQuHuangLightning(f);return;}
        if(branch=="swap")
        {
            var target=supplied.Targets.Single();var own=GetHand(_players[f.OwnerSeat]).Select(c=>new QuHuangEntity(c.Id,c.Kind,CardLocation.Hand(f.OwnerSeat))).ToArray();var other=GetHand(_players[target]).Select(c=>new QuHuangEntity(c.Id,c.Kind,CardLocation.Hand(target))).ToArray();
            frames.ReplaceTop(f=f with{QuHuang=r with{Paid=true,TargetSeat=target,OwnerCards=own,OtherCards=other}});foreach(var c in own.Concat(other))events.AdvanceFact(new NishouOriginalHandFrozenEvent(f.Id,c.From.OwnerSeat!.Value,c.CardId,c.Kind));
            PayQuHuangCards(f,QuHuangStage.HoldOwner,own,CardLocation.Processing,new(NishouHoldReason));CompleteQuHuangMovement(f.Id);return;
        }
        throw new InvalidOperationException("Unknown Qu Huang branch.");
    }
    private void PrepareQuHuangPayment(ref ProgramSkillFrame f,QuHuangStage stage,IReadOnlyList<QuHuangEntity> entities)
    {
        observations.Capture(participants.Players);var r=f.QuHuang!;
        var grain=entities.Where(c=>c.Kind==CardKind.WoodenOx&&c.From.Zone==CardZoneKind.Equipment).SelectMany(c=>_cardZones.CardsAt(CardLocation.WoodenOxGrain(c.From.OwnerSeat!.Value))).Select(c=>c.Id).ToArray();
        frames.ReplaceTop(f=f with{QuHuang=r with{Paid=true,Stage=stage,Entities=entities,Grain=grain,Step=r.Step+1,Before=_movementSequence,After=_movementSequence,ActualCount=0},PendingMovementContinuation=new(r.TargetSeat,0,null)});
    }
    private void PayQuHuangCards(ProgramSkillFrame f,QuHuangStage stage,IReadOnlyList<QuHuangEntity> entities,CardLocation destination,CardMoveReason reason)
    {
        var actual=entities.Where(c=>_cardZones.GetLocation(c.CardId)==c.From).ToArray();PrepareQuHuangPayment(ref f,stage,actual);
        if(actual.Length>0)movement.MoveMultipleSources(actual.Select(c=>c.CardId).ToArray(),destination,reason,CardMovementDisclosure.Unspecified,(batch,records)=>{foreach(var m in records)events.AdvanceFact(new QuHuangEntityMovedEvent(f.Id,f.QuHuang!.Step,stage,batch,m.CardId,m.CardKind,m.From,m.To,m.Sequence));});
    }
    private void CompleteQuHuangMovement(long id,int? count=null)
    {var f=frames.ActiveProgram(id);var r=f.QuHuang!;var actual=count??ProgramEventHistory<QuHuangEntityMovedEvent>().Count(e=>e.FrameId==id&&e.Step==r.Step);frames.ReplaceTop(f=f with{QuHuang=r with{After=_movementSequence,ActualCount=actual}});events.AdvanceFact(new QuHuangMovementPaidEvent(f.Id,r.Step,r.Stage,r.Before,_movementSequence,actual));ContinueQuHuang(id);}
    private void PayQuHuangDraw(ProgramSkillFrame f,int seat,QuHuangStage stage)
    {PrepareQuHuangPayment(ref f,stage,[]);var actual=_players[seat].IsAlive&&_winner==Winner.None?draws.DrawCards(_players[seat],1,true,new(QiejianDrawReason)).Count:0;CompleteQuHuangMovement(f.Id,actual);}
    private bool DrainQuHuang(ProgramSkillFrame f)=>childWindows.Begin(f.Id, PostEventContinuation.AwaitedProgramMovement);
    private void ContinueQuHuang(long id)
    {
        while(_resolutionStack.LastOrDefault()is ProgramSkillFrame{QuHuang:{}r}f&&f.Id==id)
        {
            AssertQuHuang(f);
            if(r.Stage==QuHuangStage.Lightning)return;
            if(r.Stage is QuHuangStage.QiejianChoice or QuHuangStage.NishouChoice or QuHuangStage.SwapTarget)
            {if(!_players[f.OwnerSeat].IsAlive||_winner!=Winner.None){FinishQuHuang(f);return;}if(prompts.Current is null)PublishQuHuang(f);return;}
            if(DrainQuHuang(f))return;f=frames.ActiveProgram(id);r=f.QuHuang!;frames.ReplaceTop(f=f with{PendingMovementContinuation=null});
            if(r.Op==SkillProgramEffectOp.ResolveNishouIssuedCurrentPhaseSwap&&(!_players[f.OwnerSeat].IsAlive||!_players[r.TargetSeat].IsAlive||_winner!=Winner.None)&&r.Stage!=QuHuangStage.Cleanup)
            {var held=r.OwnerCards.Concat(r.OtherCards).Where(c=>IsNishouHeldProcessingCard(c.CardId)).Select(c=>c with{From=CardLocation.Processing}).ToArray();PayQuHuangCards(f,QuHuangStage.Cleanup,held,CardLocation.DiscardPile,new(NishouCleanupReason));CompleteQuHuangMovement(id);return;}
            switch(r.Stage)
            {
                case QuHuangStage.DrawOwner:PayQuHuangDraw(f,r.TargetSeat,QuHuangStage.DrawOther);return;
                case QuHuangStage.DrawOther:frames.ReplaceTop(f with{QuHuang=r with{Stage=QuHuangStage.QiejianChoice}});continue;
                case QuHuangStage.HoldOwner:PayQuHuangCards(f,QuHuangStage.HoldOther,r.OtherCards,CardLocation.Processing,new(NishouHoldReason));CompleteQuHuangMovement(id);return;
                case QuHuangStage.HoldOther:PayQuHuangCards(f,QuHuangStage.GiveOwner,r.OtherCards.Where(c=>IsNishouHeldProcessingCard(c.CardId)).Select(c=>c with{From=CardLocation.Processing}).ToArray(),CardLocation.Hand(f.OwnerSeat),new(NishouGiveReason));CompleteQuHuangMovement(id);return;
                case QuHuangStage.GiveOwner:PayQuHuangCards(f,QuHuangStage.GiveOther,r.OwnerCards.Where(c=>IsNishouHeldProcessingCard(c.CardId)).Select(c=>c with{From=CardLocation.Processing}).ToArray(),CardLocation.Hand(r.TargetSeat),new(NishouGiveReason));CompleteQuHuangMovement(id);return;
                default:FinishQuHuang(f);return;
            }
        }
    }
    private void FinishQuHuang(ProgramSkillFrame f){events.AdvanceFact(new QuHuangCompletedEvent(f.Id));frames.ReplaceTop(f=f with{QuHuang=null,PendingMovementContinuation=null});completion.Finish(f,true);}
    [ProgramContinuation(ProgramContinuationKind.Resume,27420)]private bool ResumeQuHuang(long id){if(_resolutionStack.LastOrDefault()is not ProgramSkillFrame{QuHuang:not null}f||f.Id!=id)return false;ContinueQuHuang(id);return true;}
    [ProgramContinuation(ProgramContinuationKind.Movement,27420)]private bool ReturnQuHuangMovement(ProgramSkillFrame f){if(f.QuHuang is null)return false;ContinueQuHuang(f.Id);return true;}
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss,27420)][ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath,27420)]private bool ContinuePaidQuHuang(ProgramSkillFrame f)
    {if(f.QuHuang is{Paid:true}){AssertQuHuang(f);return true;}return f.QuHuang is null&&f.InstructionIndex is 0 or 1&&f.TriggerId=="issued-current-phase-swap"&&f.WindowContext is{}c&&ProgramEventHistory<ProgramBindingStartedEvent>().Any(e=>e.FrameId==f.Id&&e.BindingId==f.TriggerId&&e.SkillInstanceId==f.SkillInstanceId)&&IsIssuedNishouCandidate(new(f.OwnerSeat,f.SkillId,f.TriggerId,f.SkillInstanceId,f.GameplayHash,GetProgramTrigger(f).Priority,c.OccurrenceIndex),c);}
    [ProgramAiChoice(SkillProgramEffectOp.ResolveQiejianLastHandLoss,SkillProgramEffectOp.ResolveNishouEquipmentLossChoice,SkillProgramEffectOp.ResolveNishouIssuedCurrentPhaseSwap)]private PromptChoice ChooseQuHuang(PendingDecision p,ProgramSkillFrame f)
    {var r=f.QuHuang!;if(r.Stage==QuHuangStage.QiejianChoice)return p.Choices.FirstOrDefault(c=>c.Parameters["branch"]=="discard"&&c.Targets.Single()!=f.OwnerSeat)??p.Choices.First(c=>c.Parameters["branch"]=="block");if(r.Stage==QuHuangStage.NishouChoice)return p.Choices.FirstOrDefault(c=>c.Parameters["branch"]=="defer")??p.Choices[0];return p.Choices.OrderByDescending(c=>GetHand(_players[f.OwnerSeat]).Count-GetHand(_players[c.Targets.Single()]).Count).ThenBy(c=>c.Targets.Single()).First();}
}
