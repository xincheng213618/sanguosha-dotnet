namespace CardGame.Core;
public sealed record AlternatingChoiceBenefitResolvedEvent(int OwnerSeat,string SkillId,string SkillInstanceId,string StateId,string OptionId,int Amount,string? PendingOptionId) : IGameEvent
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public int? OwnerTurnOrdinal { get; init; }
}
public sealed record AlternatingChoiceDrawBenefitIssuedEvent(long ProgramFrameId, int InstructionIndex,
    CardUseEffectSource Source, string GameplayHash, string StateId, int TurnNumber, int OwnerTurnOrdinal, int Amount) : IGameEvent;
public sealed record AlternatingChoiceDrawPhaseAssignedEvent(long ProgramFrameId, int InstructionIndex,
    ActualDiscardRecoveryPhaseKey Phase) : IGameEvent;
public sealed record ConsecutiveTargetDeckGiftEvent(int OwnerSeat,string SkillId,string SkillInstanceId,string StateId,int TargetSeat,int OwnerTurnOrdinal,int? CardId,bool Consecutive) : IGameEvent;
public sealed partial class GameEngine
{
    private IEnumerable<IGameEvent> CompleteProgramEventHistory()=>_events.Select(e=>e.Payload).Concat(_pendingEvents);
    [ProgramCapability]
    private sealed class AlternatingBenefitsProgramCapability(GameEngine engine) : IAlternatingBenefitsProgramHost
    {
        public void ApplyAlternatingChoiceBenefit(ProgramSkillFrame f,SkillProgramEffect e)=>engine.ApplyAlternatingChoiceBenefit(f,e);
        public SkillProgramStepOutcome ObtainDeckCardWithConsecutiveTarget(ProgramSkillFrame f,int seat,SkillProgramEffect e)=>engine.ObtainDeckCardWithConsecutiveTarget(f,seat,e);
    }
    private int AlternatingOwnerTurnOrdinal(int owner) => ProgramEventHistory<TurnStartedEvent>().Count(e => e.ActorSeat == owner);
    private AlternatingChoiceBenefitResolvedEvent? GetAlternatingChoicePredecessor(int owner, string skill, string instance, SkillProgramEffect effect)
    {
        var last = ProgramEventHistory<AlternatingChoiceBenefitResolvedEvent>().LastOrDefault(e =>
            e.OwnerSeat == owner && e.SkillId == skill && e.SkillInstanceId == instance && e.StateId == effect.StateId);
        return effect.AlternatingHistoryScope == SkillProgramAlternatingHistoryScope.PreviousOwnerTurn &&
            last?.OwnerTurnOrdinal != AlternatingOwnerTurnOrdinal(owner) - 1 ? null : last;
    }
    private void ApplyAlternatingChoiceBenefit(ProgramSkillFrame frame,SkillProgramEffect effect)
    {
        ValidateProgramTurnEffectGrant(frame);
        var choice=frame.ChoiceBindings.Single(b=>b.Name==effect.SourceBind).OptionId;
        if(choice is not ("draw" or "targets")) throw new InvalidOperationException("Unknown alternating benefit.");
        var last=GetAlternatingChoicePredecessor(frame.OwnerSeat,frame.SkillId,frame.SkillInstanceId,effect);
        var switched=last?.PendingOptionId is { } pending && pending!=choice;
        var amount=switched?2:1;
        var ordinal = AlternatingOwnerTurnOrdinal(frame.OwnerSeat);
        AdvanceEventRulesAndQueueFact(new AlternatingChoiceBenefitResolvedEvent(frame.OwnerSeat,frame.SkillId,frame.SkillInstanceId,effect.StateId!,choice,amount,switched?null:choice)
            { OwnerTurnOrdinal = effect.AlternatingHistoryScope is null ? null : ordinal });
        if(choice=="draw")
        {
            if (effect.AlternatingDrawTiming == SkillProgramAlternatingDrawTiming.NextDrawPhase)
            {
                AdvanceEventRulesAndQueueFact(new AlternatingChoiceDrawBenefitIssuedEvent(frame.Id,frame.InstructionIndex-1,
                    CreateProgramTurnEffectSource(frame),frame.GameplayHash,effect.StateId!,_turnNumber,ordinal,amount));
                return;
            }
            var grant=_turnCardUseEffects.GrantRuleModifier(_turnNumber,_currentSeat,frame.Id,frame.InstructionIndex-1,CreateProgramTurnEffectSource(frame),SkillRuleQuery.DrawCount,SkillRuleOperation.Add,amount);
            AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(grant));
        }
        else GrantRedAdditionalTargets(frame,amount,effect.PersistAlternatingTargetsAfterSkillLoss);
    }
    private IReadOnlyList<ProgramAlternatingChoiceStateSnapshot>? GetAlternatingChoiceStateSnapshot(CharacterState owner)
    {
        var states=GetSkillBindingShard(owner).ProgramInstances.SelectMany(i=>i.Program.Triggers.SelectMany(t=>t.Effects).Where(e=>e.Op==SkillProgramEffectOp.ApplyAlternatingChoiceBenefit).Select(e=>new {Instance=i,Effect=e})).ToArray();
        if(states.Length==0) return null;
        return states.Select(s=>{
            var last=ProgramEventHistory<AlternatingChoiceBenefitResolvedEvent>().LastOrDefault(e=>e.OwnerSeat==owner.Seat && e.SkillId==s.Instance.SkillId && e.SkillInstanceId==s.Instance.SkillInstanceId && e.StateId==s.Effect.StateId);
            if(s.Effect.AlternatingHistoryScope == SkillProgramAlternatingHistoryScope.PreviousOwnerTurn)
            {
                var ordinal=AlternatingOwnerTurnOrdinal(owner.Seat);
                var choosing=_resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(f=>
                    f.Window==SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && f.OwnerSeat==owner.Seat);
                if(last?.OwnerTurnOrdinal!=ordinal && !(choosing && last?.OwnerTurnOrdinal==ordinal-1)) last=null;
            }
            return new ProgramAlternatingChoiceStateSnapshot(s.Instance.SkillId,s.Effect.StateId!,s.Instance.SkillInstanceId,last?.PendingOptionId,last?.PendingOptionId=="targets"?2:1,last?.PendingOptionId=="draw"?2:1);
        }).ToArray();
    }
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 81000)]
    private void AssignAlternatingNextDrawPhase(ActualDiscardRecoveryPhaseStartedEvent e)
    {
        if(e.Phase.Kind!=ActualDiscardRecoveryPhaseKind.Draw) return;
        var assigned=ProgramEventHistory<AlternatingChoiceDrawPhaseAssignedEvent>().Select(x=>(x.ProgramFrameId,x.InstructionIndex)).ToHashSet();
        foreach(var grant in ProgramEventHistory<AlternatingChoiceDrawBenefitIssuedEvent>().Where(x=>
            x.Source.OwnerSeat==e.Phase.ActorSeat && x.TurnNumber==e.Phase.TurnNumber &&
            x.Source.OwnerSeat==e.Phase.ActualTurnOwnerSeat && !assigned.Contains((x.ProgramFrameId,x.InstructionIndex))).ToArray())
        {
            if(_contentRegistry.GetSkill(grant.Source.SkillId).Program?.GameplayHash!=grant.GameplayHash)
                throw new InvalidOperationException("An issued alternating Draw benefit lost its immutable source program.");
            AdvanceEventRulesAndQueueFact(new AlternatingChoiceDrawPhaseAssignedEvent(grant.ProgramFrameId,grant.InstructionIndex,e.Phase));
        }
    }
    private int AlternatingNextDrawPhaseBonus(CharacterState player)
    {
        if(CurrentActualDiscardRecoveryPhase() is not {Kind:ActualDiscardRecoveryPhaseKind.Draw} phase || phase.ActorSeat!=player.Seat) return 0;
        var assigned=ProgramEventHistory<AlternatingChoiceDrawPhaseAssignedEvent>().Where(e=>e.Phase==phase)
            .Select(e=>(e.ProgramFrameId,e.InstructionIndex)).ToHashSet();
        return ProgramEventHistory<AlternatingChoiceDrawBenefitIssuedEvent>().Where(e=>assigned.Contains((e.ProgramFrameId,e.InstructionIndex))).Sum(e=>e.Amount);
    }
    private SkillProgramStepOutcome ObtainDeckCardWithConsecutiveTarget(ProgramSkillFrame frame,int targetSeat,SkillProgramEffect effect)
    {
        if(frame.OwnerSeat!=_currentSeat || targetSeat==frame.OwnerSeat || !_players[targetSeat].IsAlive || frame.WindowContext?.Window!=SkillProgramTriggerWindow.TurnEnding)
            throw new InvalidOperationException("A deck gift needs the owner's ending and a living other target.");
        var ordinal=ProgramEventHistory<TurnStartedEvent>().Count(e=>e.ActorSeat==frame.OwnerSeat);
        var last=ProgramEventHistory<ConsecutiveTargetDeckGiftEvent>().LastOrDefault(e=>e.OwnerSeat==frame.OwnerSeat && e.SkillId==frame.SkillId && e.SkillInstanceId==frame.SkillInstanceId && e.StateId==effect.StateId);
        var repeated=last is not null && last.TargetSeat==targetSeat && last.OwnerTurnOrdinal==ordinal-1;
        var card=_cardZones.CardsAt(CardLocation.DrawPile).Reverse().FirstOrDefault(c=>effect.Suits.Contains(c.Suit) && effect.CardCategories.Any(category => MatchesSkillProgramCardCategory(c.Kind, category)));
        var index=_resolutionStack.FindIndex(f=>f.Id==frame.Id);
        ReplaceRuntimeFrame(_resolutionStack[index].Id, frame with {ChoiceBindings=frame.ChoiceBindings.Append(new ProgramChoiceResultBinding(effect.ResultBind!,repeated?"repeat":"new",targetSeat)).ToArray()});
        AdvanceEventRulesAndQueueFact(new ConsecutiveTargetDeckGiftEvent(frame.OwnerSeat,frame.SkillId,frame.SkillInstanceId,effect.StateId!,targetSeat,ordinal,card?.Id,repeated));
        if(card is not null)
        {
            MoveCard(card,CardLocation.DrawPile,CardLocation.Hand(targetSeat),new CardMoveReason("skill-program.consecutive-target.deck-gift"));
            return AwaitProgramBoundCardMovements(frame.Id,frame.OwnerSeat);
        }
        return SkillProgramStepOutcome.Continue;
    }
}
