namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]private sealed class BoundaryAnjianHost(GameEngine engine):IBoundaryAnjianHost
    {public void Issue(ProgramSkillFrame frame)=>engine.IssueBoundaryAnjianChoice(frame);}
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification,54000)]
    private bool CanOfferBoundaryPanZhangMaZhong(ProgramTriggerLegalityContext gate)
    {
        if(gate.SkillId==BoundaryPanZhangMaZhongComposition.Duodao)
            return gate.Context.SourceSeat is{}source&&IsValidPlayerSeat(source)&&GetEquipment(source).Any(c=>EquipmentCatalog.IsEquipment(c.Kind)&&EquipmentCatalog.Get(c.Kind).Slot==EquipmentSlot.Weapon);
        if(gate.SkillId!=BoundaryPanZhangMaZhongComposition.Anjian)return true;
        return gate.Context is{Window:SkillProgramTriggerWindow.CardUseTargetsFinalized,TargetSeat:{}target,CardUse:{}card}&&
            target!=gate.OwnerSeat&&IsValidPlayerSeat(target)&&_players[target].IsAlive&&card.ActorSeat==gate.OwnerSeat&&
            _resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u=>u.Id==card.ParentCardUseFrameId)?.Action is{Type:CardActionType.Use}action&&
            action.ActionId==card.CardActionId&&IsSlashCard(action.EffectiveKind)&&action.EffectiveDesignatedTargetSeats.Contains(target)&&!IsWithinAttackRange(target,gate.OwnerSeat);
    }
    private void IssueBoundaryAnjianChoice(ProgramSkillFrame frame)
    {
        var choice=frame.ChoiceBindings.SingleOrDefault(c=>c.Name=="anjian-choice");
        if(frame.SkillId!=BoundaryPanZhangMaZhongComposition.Anjian||frame.TriggerId!="outside-target-range-choice"||frame.InstructionIndex!=2||
            frame.WindowContext is not{Window:SkillProgramTriggerWindow.CardUseTargetsFinalized,TargetSeat:{}target,CardUse:{}context}||
            _resolutionStack.LastOrDefault()?.Id!=frame.Id||_resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u=>u.Id==context.ParentCardUseFrameId)is not{Action:{Type:CardActionType.Use}action}use||
            action.ActionId!=context.CardActionId||action.ActorSeat!=frame.OwnerSeat||!IsSlashCard(action.EffectiveKind)||!action.EffectiveDesignatedTargetSeats.Contains(target)||
            choice is null||choice.ChooserSeat!=frame.OwnerSeat||choice.OptionId is not("no-response"or"damage"))
            throw new InvalidOperationException("Anjian requires its original Slash actor, current designated target and exact mandatory choice.");
        var entries=use.BoundaryAnjianReceipts??[];
        if(entries.Any(r=>r.ProducerFrameId==frame.Id))throw new InvalidOperationException("Anjian cannot issue the same per-target choice twice.");
        var receipt=new BoundaryAnjianReceipt(use.Id,action.ActionId,frame.Id,new(frame.SkillId,GetProgramBindingId(frame),frame.OwnerSeat,frame.SkillInstanceId),frame.GameplayHash,target,choice.OptionId);
        ReplaceRuntimeFrame(use.Id,use with{BoundaryAnjianReceipts=entries.Append(receipt).ToArray()});
        AdvanceEventRulesAndQueueFact(new BoundaryAnjianChoiceIssuedEvent(receipt));
    }
    private bool IsBoundaryAnjianUnrespondable(long frameId)=>ActiveCardAttack is{}attack&&attack.ResolutionId==frameId&&
        IsSlashCard(attack.EffectiveCardKind??CardKind.Slash)&&BoundaryAnjianReceipts(frameId,attack.TargetSeat).Any(r=>r.Option=="no-response");
    private IEnumerable<BoundaryAnjianReceipt> BoundaryAnjianReceipts(long frameId,int target)=>
        _resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u=>u.Id==frameId)is{Action:{}action,BoundaryAnjianReceipts:{}entries}use?
            entries.Where(r=>r.CardUseFrameId==use.Id&&r.ActionId==action.ActionId&&r.Source.OwnerSeat==action.ActorSeat&&r.TargetSeat==target&&action.EffectiveDesignatedTargetSeats.Contains(target)):[];
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount,54000)]
    private int BoundaryAnjianDamage(IDamageAttempt attack)=>!attack.IsChainPropagation&&attack is CardAttackHandle&&IsSlashCard(attack.EffectiveCardKind??CardKind.Slash)?
        BoundaryAnjianReceipts(attack.ResolutionId,attack.TargetSeat).Count(r=>r.Option=="damage"):0;
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish,54001)]
    private void PresentBoundaryAnjianTarget()
    {
        if(_pendingDecision is not{Kind:DecisionKind.ProgramTrigger}decision||decision.SkillPrompt?.SkillId!=BoundaryPanZhangMaZhongComposition.Anjian||
            _resolutionStack.LastOrDefault()is not ProgramSkillFrame{SkillId:BoundaryPanZhangMaZhongComposition.Anjian,WindowContext:{TargetSeat:{}target}}frame||
            decision.PlayerSeat!=frame.OwnerSeat||!decision.Choices.All(c=>c.Parameters.GetValueOrDefault("result-bind")=="anjian-choice"))return;
        _pendingDecision=decision with{TargetSeat=target,Prompt=$"暗箭：对 {_players[target].Name} 选择无法响应，或此杀对其伤害+1。"};
    }
    private bool TrySelectBoundaryAnjianOption(PendingDecision decision,ProgramSkillFrame frame,out PromptChoice choice)
    {
        choice=null!;
        if(frame.SkillId!=BoundaryPanZhangMaZhongComposition.Anjian||frame.WindowContext?.TargetSeat is not{}target)return false;
        // Only public hand count is used; the actor never reads another player's actual Dodge entities.
        var preferred=CreateSnapshot(frame.OwnerSeat).Players.Single(p=>p.Seat==target).HandCount==0?"damage":"no-response";
        choice=decision.Choices.Single(c=>c.Parameters.GetValueOrDefault("option-id")==preferred);return true;
    }
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish,54000)]
    private void AssertBoundaryAnjianReceipts()
    {
        foreach(var use in _resolutionStack.OfType<CardUseFrame>().Where(u=>u.BoundaryAnjianReceipts is not null))
        {
            var entries=use.BoundaryAnjianReceipts!;
            if(use.Action is not{Type:CardActionType.Use}action||!IsSlashCard(action.EffectiveKind)||entries.DistinctBy(r=>r.ProducerFrameId).Count()!=entries.Count||
                entries.Any(r=>r.CardUseFrameId!=use.Id||r.ActionId!=action.ActionId||r.Source.OwnerSeat!=action.ActorSeat||r.Source.SkillId!=BoundaryPanZhangMaZhongComposition.Anjian||
                    r.Source.BindingId!="outside-target-range-choice"||string.IsNullOrWhiteSpace(r.Source.SkillInstanceId)||r.ProducerFrameId<=0||
                    r.Option is not("no-response"or"damage")||!IsValidPlayerSeat(r.TargetSeat)||r.TargetSeat==action.ActorSeat||!action.EffectiveDesignatedTargetSeats.Contains(r.TargetSeat)||
                    _contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash!=r.GameplayHash||
                    ProgramEventHistory<BoundaryAnjianChoiceIssuedEvent>().Count(e=>e.Receipt==r)!=1))
                throw new InvalidOperationException("Anjian lost its exact original Use, per-target choice or immutable source receipt.");
        }
    }
}
