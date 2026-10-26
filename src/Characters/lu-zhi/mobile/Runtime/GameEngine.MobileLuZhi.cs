using System.Globalization;
namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string HpDifferencePileDiscard="skill-program.public-pile.hp-difference-payment";
    private bool? HpDifferencePileCardColor(int owner,Card card)=>SuitColor(EffectiveSuit(_players[owner],card));
    private int HpDifferencePileX(int owner,int target)=>_players[owner].HasHpValue&&_players[target].HasHpValue?Math.Max(1,Math.Abs(_players[owner].Hp-_players[target].Hp)):int.MaxValue;
    private Card[] HpDifferencePileCosts(int owner,string skill,string instance,bool red)=>GetHand(_players[owner]).Concat(GetEquipment(_players[owner])).Where(c=>
        HpDifferencePileCardColor(owner,c)==red&&!IsActiveProgramSourceEquipmentCard(owner,skill,instance,c)&&
        !IsSelfHandCategoryDiscardForbidden(owner,c,_cardZones.GetLocation(c.Id),OwnedCardMoveIntent.Discard)).OrderBy(c=>c.Id).ToArray();
    private IReadOnlyList<int> HpDifferencePileDamageTargets(int owner,string skill,string instance,SkillProgramEffect effect)=>
        GetProgramTargetSeats(owner,SkillProgramTargetKind.OtherLivingInAttackRange).Where(target=>SameGrantColorPiles(owner,effect.SkillIds.Single(),instance).Any(p=>
            HpDifferencePileCardColor(owner,PublicPileCards(p).Single()) is{} red&&HpDifferencePileCosts(owner,skill,instance,red).Length>=HpDifferencePileX(owner,target))).ToArray();
    private SkillProgramStepOutcome StartHpDifferencePileDamage(ProgramSkillFrame frame,SkillProgramEffect effect)
    {
        if(frame.HpDifferencePileDamage is not null||frame.TriggerId is not null||frame.InstructionIndex!=1||
            GetProgramConversionPolarity(frame.OwnerSeat,frame.SkillId)!=SkillPolarity.Yang||HpDifferencePileDamageTargets(frame.OwnerSeat,frame.SkillId,frame.SkillInstanceId,effect).Count==0)
            throw new InvalidOperationException("HP-difference pile damage requires its original payable Yang activation.");
        var ren=SameGrantColorPiles(frame.OwnerSeat,effect.SkillIds.Single(),frame.SkillInstanceId).First(p=>HpDifferencePileCardColor(frame.OwnerSeat,PublicPileCards(p).Single()) is{} red&&
            GetProgramTargetSeats(frame.OwnerSeat,SkillProgramTargetKind.OtherLivingInAttackRange).Any(target=>HpDifferencePileCosts(frame.OwnerSeat,frame.SkillId,frame.SkillInstanceId,red).Length>=HpDifferencePileX(frame.OwnerSeat,target)));
        var card=PublicPileCards(ren).Single();
        ReplaceRuntimeTop(frame=frame with{HpDifferencePileDamage=new(ren,card.Id,HpDifferencePileCardColor(frame.OwnerSeat,card)!.Value)});
        PublishHpDifferencePileDamage(frame);return SkillProgramStepOutcome.AwaitChoice;
    }
    private PromptChoice HpDifferencePileChoice(ProgramSkillFrame f,string branch,int value,string label)=>new(new($"pile-hp-difference.{f.Id}.{f.HpDifferencePileDamage!.CardIds.Count}.{branch}.{value}"),label,
        branch=="card"?[value]:[],branch=="target"?[value]:[],new Dictionary<string,string>{["program-action"]="pile-hp-difference",["branch"]=branch,["frame-id"]=f.Id.ToString(CultureInfo.InvariantCulture)});
    private IReadOnlyList<PromptChoice> HpDifferencePileChoices(ProgramSkillFrame f)
    {
        var r=f.HpDifferencePileDamage!;
        if(r.Stage==HpDifferencePileDamageStage.Cards)return HpDifferencePileCosts(f.OwnerSeat,f.SkillId,f.SkillInstanceId,r.IsRed).Where(c=>!r.CardIds.Contains(c.Id))
            .Select(c=>HpDifferencePileChoice(f,"card",c.Id,$"弃置【{c.DisplayName}】（{r.CardIds.Count+1}/{r.Required}）")).ToArray();
        return GetProgramTargetSeats(f.OwnerSeat,SkillProgramTargetKind.OtherLivingInAttackRange).Where(seat=>HpDifferencePileCosts(f.OwnerSeat,f.SkillId,f.SkillInstanceId,r.IsRed).Length>=HpDifferencePileX(f.OwnerSeat,seat))
            .Select(seat=>HpDifferencePileChoice(f,"target",seat,$"{_players[seat].Name}：弃置{HpDifferencePileX(f.OwnerSeat,seat)}张同色牌，造成1点伤害")).ToArray();
    }
    private void PublishHpDifferencePileDamage(ProgramSkillFrame f)
    {
        var choices=HpDifferencePileChoices(f);if(choices.Count==0)throw new InvalidOperationException("HP-difference discard lost its payable target or owned cost.");
        var skill=_contentRegistry.GetSkill(f.SkillId);
        _pendingDecision=new(DecisionKind.ProgramTrigger,f.OwnerSeat,skill.Description,choices.SelectMany(c=>c.Cards).ToArray(),choices.SelectMany(c=>c.Targets).ToArray(),f.OwnerSeat)
        {PromptId=CreatePromptId(),IsPrivate=true,Choices=choices,SkillPrompt=new(f.SkillId,skill.Name,skill.Name,skill.Description)};
        _status=_players[f.OwnerSeat].IsHuman?EngineStatus.AwaitingHumanResponse:EngineStatus.Running;
    }
    [ProgramAnswer("pile-hp-difference")]
    private void AnswerHpDifferencePileDamage(PromptChoice choice)
    {
        var f=(ProgramSkillFrame)_resolutionStack[^1];var r=f.HpDifferencePileDamage!;
        if(!HpDifferencePileChoices(f).Any(c=>c.Id==choice.Id)||choice.Parameters.GetValueOrDefault("frame-id")!=f.Id.ToString(CultureInfo.InvariantCulture))throw new InvalidOperationException("HP-difference damage accepts only its current exact choice.");
        ClearPendingDecision();
        if(r.Stage==HpDifferencePileDamageStage.Target)
        {
            var target=choice.Targets.Single();ReplaceRuntimeTop(f=f with{HpDifferencePileDamage=r with{Stage=HpDifferencePileDamageStage.Cards,TargetSeat=target,
                Required=HpDifferencePileX(f.OwnerSeat,target),OwnerHp=_players[f.OwnerSeat].Hp,TargetHp=_players[target].Hp}});PublishHpDifferencePileDamage(f);return;
        }
        var ids=r.CardIds.Append(choice.Cards.Single()).ToArray();ReplaceRuntimeTop(f=f with{HpDifferencePileDamage=r=r with{CardIds=ids}});
        if(ids.Length<r.Required){PublishHpDifferencePileDamage(f);return;}
        if(GetProgramConversionPolarity(f.OwnerSeat,f.SkillId)!=SkillPolarity.Yang||!_players[r.TargetSeat].IsAlive||ids.Any(id=>!HpDifferencePileCosts(f.OwnerSeat,f.SkillId,f.SkillInstanceId,r.IsRed).Any(c=>c.Id==id)))
            throw new InvalidOperationException("HP-difference damage lost its original exact same-color cost before payment.");
        CommitProgramConversionPolarity(f);f=GetActiveProgramFrame(f.Id);var before=_movementSequence;
        ReplaceRuntimeTop(f=f with{HpDifferencePileDamage=r with{Stage=HpDifferencePileDamageStage.Movement,Before=before,After=before},PendingMovementContinuation=new(f.OwnerSeat,0,null)});
        MoveProgramLogicalDiscard(f,f.OwnerSeat,ids,new(HpDifferencePileDiscard));
        f=GetActiveProgramFrame(f.Id);ReplaceRuntimeTop(f=f with{HpDifferencePileDamage=f.HpDifferencePileDamage! with{After=_movementSequence}});
        AdvanceEventRulesAndQueueFact(new HpDifferencePileDamagePaidEvent(f.Id,f.OwnerSeat,r.TargetSeat,r.Required,r.OwnerHp,r.TargetHp,before,_movementSequence));
        ContinueHpDifferencePileDamage(f.Id);
    }
    private void ContinueHpDifferencePileDamage(long id)
    {
        if(_pendingDecision is not null||_resolutionStack.LastOrDefault() is not ProgramSkillFrame {HpDifferencePileDamage:{Stage:HpDifferencePileDamageStage.Movement} r} f||f.Id!=id)return;
        if(TryBeginQueuedRecoveryReplacement(id,PostEventContinuation.AwaitedProgramMovement)||TryBeginCharacterStateProgramWindow(id,CharacterStateContinuation.Program)||
            TryBeginHpChangedProgramWindow(id,PostEventContinuation.AwaitedProgramMovement)||TryBeginCardsMovedProgramWindow(id)||TryBeginAdvancedSkillsChanged(id))return;
        ReplaceRuntimeTop(f=f with{PendingMovementContinuation=null,HpDifferencePileDamage=r with{Stage=HpDifferencePileDamageStage.Damage}});
        if(_winner==Winner.None&&_players[r.TargetSeat].IsAlive)BeginProgramSkillDamage(f,r.TargetSeat,1,new(ProgramParticipantRef.Owner));
        else AdvanceRuntimeProgram(id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume,81400)]
    private bool ResumeHpDifferencePileDamage(long id)
    {
        if(_resolutionStack.LastOrDefault() is not ProgramSkillFrame {HpDifferencePileDamage:{} r} f||f.Id!=id)return false;
        if(r.Stage!=HpDifferencePileDamageStage.Movement)return false;
        ContinueHpDifferencePileDamage(id);return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement,81400)]
    private bool ReturnHpDifferencePileDamage(ProgramSkillFrame f)=>f.HpDifferencePileDamage is{Stage:HpDifferencePileDamageStage.Movement}&&ResumeHpDifferencePileDamage(f.Id);

    private bool MobileLuZhiCompletion(ProgramTriggerCandidate candidate,ProgramSkillWindowContext context,bool committed=false)
    {
        if(candidate.SkillId!=MobileLuZhiComposition.Zhenliang||context.CardUse is not{} card||_currentSeat==candidate.OwnerSeat||_turnNumber<1||
            !committed&&GetProgramConversionPolarity(candidate.OwnerSeat,candidate.SkillId)!=SkillPolarity.Yin||
            _resolutionStack.SingleOrDefault(f=>f.Id==context.ParentFrameId) is not ProgramCardTriggerWindowFrame window||GetCardActionWindow(window)!=context.Window||window.Action.ActionId!=card.CardActionId)return false;
        var action=context.Window==SkillProgramTriggerWindow.CardResponseCompleted?ResponseCompletionActorAction(window):window.Action;
        if(action.ActorSeat!=candidate.OwnerSeat||!SameGrantColorPiles(candidate.OwnerSeat,MobileLuZhiComposition.Mingren,candidate.SkillInstanceId).Any(p=>
            GetProgramCardCategory(PublicPileCards(p).Single().Kind)==GetProgramCardCategory(action.EffectiveKind)))return false;
        return context.Window==SkillProgramTriggerWindow.CardUseCompleted?
            action.Type==CardActionType.Use||window.CompletedResponseReturn is{IsCommitted:false}&&IsCompletedResponseParentConsistent(window):
            context.Window==SkillProgramTriggerWindow.CardResponseCompleted&&window.ResponseCompletion is{CostsDrained:true}&&ValidResponseCompletionWindow(window)&&!IsMobileLuZhiTrueResponseUse(action);
    }
    private bool IsMobileLuZhiTrueResponseUse(CardActionContext action)=>IsCompletedResponseUse(action,ProgramCompletedResponseKind.Nullification)||
        IsCompletedResponseUse(action,ProgramCompletedResponseKind.Dodge)&&_resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f=>f.Action?.ActionId==action.ParentActionId)
            is{CardAttack:{EffectiveCardKind:{} kind} attack}&&IsSlashCard(kind)&&attack.TargetSeat==action.ActorSeat;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification,81400,SkillProgramEffectOp.CommitConversionPolarity)]
    private bool CanOfferMobileLuZhiCompletion(ProgramTriggerCandidate candidate,ProgramSkillWindowContext context)=>candidate.SkillId!=MobileLuZhiComposition.Zhenliang||MobileLuZhiCompletion(candidate,context);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent,81400)]
    private void CaptureMobileLuZhiYin(ProgramConversionPolarityCommittedEvent fact)
    {
        if(fact.SkillId!=MobileLuZhiComposition.Zhenliang||fact.PreviousState!=SkillPolarity.Yin)return;
        var f=GetActiveProgramFrame(fact.FrameId);var context=f.WindowContext??throw new InvalidOperationException("Mobile Zhenliang Yin lost its original completion.");
        if(!MobileLuZhiCompletion(new(f.OwnerSeat,f.SkillId,f.TriggerId!,f.SkillInstanceId,f.GameplayHash,0),context,true))throw new InvalidOperationException("Mobile Zhenliang Yin cannot commit without its actual matching-category completion.");
        var ren=SameGrantColorPiles(f.OwnerSeat,MobileLuZhiComposition.Mingren,f.SkillInstanceId).Single();var renCard=PublicPileCards(ren).Single();
        ReplaceRuntimeFrame(f.Id,f with{MobileLuZhiYin=new(context.CardUse!.CardActionId,context.ParentFrameId,context.Window,renCard.Id,GetProgramCardCategory(renCard.Kind),_turnNumber,_currentSeat)});
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss,81400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath,81400)]
    private bool KeepMobileLuZhiPaid(ProgramSkillFrame f)=>f.SkillId==MobileLuZhiComposition.Zhenliang&&
        (f.HpDifferencePileDamage is{Stage:HpDifferencePileDamageStage.Movement or HpDifferencePileDamageStage.Damage}&&f.ConversionPreviousPolarity==SkillPolarity.Yang||
         f.MobileLuZhiYin is{} r&&f.ConversionPreviousPolarity==SkillPolarity.Yin&&f.WindowContext?.ParentFrameId==r.ParentWindowId&&f.InstructionIndex>=3);

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState,81400)]
    private void AssertMobileLuZhiFrames(ProgramSkillFrame f)
    {
        if(f.HpDifferencePileDamage is{} r)
        {
            var effect=GetPausedPrivateOfferEffect(f);
            if(effect.Op!=SkillProgramEffectOp.PublicPileColorDamage||effect.PublicPileDamageCostPolicy!=PublicPileColorDamageCostPolicy.HpDifferenceAtLeastOne||f.InstructionIndex!=1||
                f.TriggerId is not null||f.WindowContext is not null||f.SelectedCardIds.Count!=0||f.SelectedTargetSeats.Count!=0||r.Ren.OwnerSeat!=f.OwnerSeat||
                !effect.SkillIds.SequenceEqual(new[]{r.Ren.SkillId})||!Enum.IsDefined(r.Stage)||r.CardIds.Distinct().Count()!=r.CardIds.Count||r.CardIds.Count>r.Required||
                r.Stage==HpDifferencePileDamageStage.Target&&r.CardIds.Count!=0||r.Stage!=HpDifferencePileDamageStage.Target&&
                    (r.TargetSeat==f.OwnerSeat||!IsValidPlayerSeat(r.TargetSeat)||r.Required!=Math.Max(1,Math.Abs(r.OwnerHp-r.TargetHp))))
                throw new InvalidOperationException("HP-difference pile damage lost its original source, frozen target HP or exact selection.");
            if(r.Stage is HpDifferencePileDamageStage.Movement or HpDifferencePileDamageStage.Damage)
                if(f.ConversionPreviousPolarity!=SkillPolarity.Yang||r.CardIds.Count!=r.Required||r.Before<0||r.After<r.Before||r.After>_movementSequence||
                    ProgramEventHistory<HpDifferencePileDamagePaidEvent>().Count(e=>e==new HpDifferencePileDamagePaidEvent(f.Id,f.OwnerSeat,r.TargetSeat,r.Required,r.OwnerHp,r.TargetHp,r.Before,r.After))!=1||
                    r.CardIds.Any(id=>_cardMovements.Count(m=>m.Sequence>r.Before&&m.Sequence<=r.After&&m.CardId==id&&m.From.OwnerSeat==f.OwnerSeat&&
                        m.From.Zone is(CardZoneKind.Hand or CardZoneKind.Equipment)&&m.Reason.Value==HpDifferencePileDiscard)!=1))
                    throw new InvalidOperationException("HP-difference pile damage lost its once-paid physical discard or original-frame continuation.");
        }
        if(f.MobileLuZhiYin is{} yin&&
            (f.SkillId!=MobileLuZhiComposition.Zhenliang||f.ConversionPreviousPolarity!=SkillPolarity.Yin||f.WindowContext is not{} context||
             context.ParentFrameId!=yin.ParentWindowId||context.Window!=yin.Window||context.CardUse?.CardActionId!=yin.ActionId||
             _resolutionStack.SingleOrDefault(w=>w.Id==yin.ParentWindowId) is not ProgramCardTriggerWindowFrame parent||
             GetProgramCardCategory(parent.Action.EffectiveKind)!=yin.Category||yin.TurnNumber!=_turnNumber||yin.TurnOwnerSeat!=_currentSeat||yin.TurnOwnerSeat==f.OwnerSeat))
            throw new InvalidOperationException("Mobile Zhenliang Yin lost its original outside-turn matching-category completion.");
    }
}
