namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundaryPingkouAction="boundary-zhu-huan-pingkou",BoundaryPingkouGain="skill.boundary-zhu-huan.pingkou.equipment";
    private bool TracksBoundaryZhuHuanPhaseSkips=>_contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBoundaryZhuHuanPingkou)==true;
    private static CardConversionSource BoundaryZhuHuanSource(ProgramSkillFrame frame)=>new(frame.SkillId,GetProgramBindingId(frame),frame.OwnerSeat,frame.SkillInstanceId);
    [ProgramCapability] private sealed class BoundaryZhuHuanHost(GameEngine engine):IBoundaryZhuHuanHost
    {
        public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame)=>engine.BeginBoundaryPingkou(frame);
        public void SkipDraw(ProgramSkillFrame frame)=>engine.SkipBoundaryZhuHuanDraw(frame);
    }
    private void SkipBoundaryZhuHuanDraw(ProgramSkillFrame frame)
    {
        if(frame.SkillId!=BoundaryZhuHuanComposition.Fenli||frame.TriggerId!="skip-draw-most-hand"||frame.InstructionIndex!=1||frame.OwnerSeat!=_currentSeat||
            frame.WindowContext is not{Window:SkillProgramTriggerWindow.DrawPhaseStarting}context||_resolutionStack.Count<2||
            _resolutionStack[^1].Id!=frame.Id||_resolutionStack[^2]is not ProgramLifecycleTriggerWindowFrame parent||parent.Id!=context.ParentFrameId||parent.OwnerSeat!=frame.OwnerSeat)
            throw new InvalidOperationException("Fenli Draw substitution requires its exact original owner starting window.");
        ReplaceRuntimeFrame(parent.Id,parent with{ActualPhaseSubstitution=new(parent.Id,frame.Id,frame.OwnerSeat,frame.SkillId,frame.SkillInstanceId,_turnNumber,SkillProgramTurnPhase.Draw)});
        AdvanceEventRulesAndQueueFact(new ProgramTurnPhaseSubstitutedEvent(frame.Id,frame.SkillId,GetProgramBindingId(frame),frame.SkillInstanceId,frame.OwnerSeat,_turnNumber,context.Window,SkillProgramTurnPhase.Draw));
    }

    private void RecordBoundaryZhuHuanSkippedPhase(int owner,SkillProgramTurnPhase phase,long extraDrawFrameId=0)
    {
        if(!TracksBoundaryZhuHuanPhaseSkips)return;
        if(owner!=_currentSeat||_turnNumber<=0||!IsValidPlayerSeat(owner)||extraDrawFrameId<0||extraDrawFrameId!=0&&phase!=SkillProgramTurnPhase.Draw)
            throw new InvalidOperationException("Pingkou phase history requires a genuine current actual phase.");
        if(ProgramEventHistory<BoundaryZhuHuanPhaseSkippedEvent>().Any(e=>e.OwnerSeat==owner&&e.ActualTurn==_turnNumber&&e.Phase==phase&&e.ExtraDrawFrameId==extraDrawFrameId))return;
        AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanPhaseSkippedEvent(owner,_turnNumber,phase,extraDrawFrameId));
    }
    // Observation precedes any hook that opens a child; re-entry deduplicates the same real occurrence.
    [ProgramPipelineHook(ProgramPipelinePhase.ActualPhaseSkipped,538)]
    private bool ObserveBoundaryZhuHuanSkippedPhase(ProgramActualPhaseSkipContext context)
    {
        if(TracksBoundaryZhuHuanPhaseSkips)
        {
            if(context.ActualTurn!=_turnNumber)throw new InvalidOperationException("Pingkou cannot count another actual turn's phase.");
            RecordBoundaryZhuHuanSkippedPhase(context.OwnerSeat,context.Phase);
        }
        return false;
    }
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent,53800)]
    private void ObserveBoundaryZhuHuanSkippedDraw(IGameEvent payload)
    {
        if(!TracksBoundaryZhuHuanPhaseSkips||payload is not DrawPhaseSkippedEvent e)return;
        if(e.ActualTurnNumber!=_turnNumber||e.OwnerSeat!=_currentSeat||
            _resolutionStack.OfType<DrawPhaseObligationFrame>().SingleOrDefault(f=>f.Id==e.FrameId)is not{Skipped:true}native||native.IsExtra!=e.IsExtra||native.OwnerSeat!=e.OwnerSeat)
            throw new InvalidOperationException("Pingkou requires the original skipped native Draw occurrence.");
        RecordBoundaryZhuHuanSkippedPhase(e.OwnerSeat,SkillProgramTurnPhase.Draw,e.IsExtra?e.FrameId:0);
    }
    private int BoundaryPingkouMaximum(int owner)=>ProgramEventHistory<BoundaryZhuHuanPhaseSkippedEvent>().Count(e=>e.OwnerSeat==owner&&e.ActualTurn==_turnNumber);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification,53800)]
    private bool CanOfferBoundaryZhuHuan(ProgramTriggerLegalityContext gate)
    {
        var owner=gate.Owner;
        if(gate.SkillId==BoundaryZhuHuanComposition.Fenli)return owner.Seat==_currentSeat&&(gate.Candidate.BindingId switch
        {
            "skip-draw-most-hand"=>_players.Where(p=>p.IsAlive).All(p=>GetHand(p).Count<=GetHand(owner).Count),
            "skip-play-most-hp"=>owner.HasHpValue&&_players.Where(p=>p.IsAlive&&p.HasHpValue).All(p=>p.Hp<=owner.Hp),
            "skip-discard-most-equipment"=>GetEquipment(owner).Count>0&&_players.Where(p=>p.IsAlive).All(p=>GetEquipment(p).Count<=GetEquipment(owner).Count),
            _=>false
        });
        return gate.SkillId!=BoundaryZhuHuanComposition.Pingkou||owner.Seat==_currentSeat&&BoundaryPingkouMaximum(owner.Seat)>0&&_players.Any(p=>p.IsAlive&&p.Seat!=owner.Seat);
    }
    private SkillProgramStepOutcome BeginBoundaryPingkou(ProgramSkillFrame supplied)
    {
        var frame=GetActiveProgramFrame(supplied.Id);
        if(frame.BoundaryZhuHuan is not null||frame.SkillId!=BoundaryZhuHuanComposition.Pingkou||frame.InstructionIndex!=1||
            frame.WindowContext is not{Window:SkillProgramTriggerWindow.AfterTurnEnded}context||frame.OwnerSeat!=_currentSeat||
            !_resolutionStack.OfType<DeferredTurnEndFrame>().Any(p=>p.Id==context.ParentFrameId&&p.OwnerSeat==frame.OwnerSeat&&p.TurnNumber==_turnNumber)||BoundaryPingkouMaximum(frame.OwnerSeat)<1)
            throw new InvalidOperationException("Pingkou requires its original actually ended turn and real skipped phases.");
        var receipt=new BoundaryZhuHuanReceipt(BoundaryZhuHuanSource(frame),frame.GameplayHash,_turnNumber,context.ParentFrameId,BoundaryPingkouMaximum(frame.OwnerSeat),BoundaryZhuHuanStage.Targets);
        ReplaceRuntimeTop(frame=frame with{BoundaryZhuHuan=receipt});
        AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanStartedEvent(frame.Id,receipt.Source,receipt.GameplayHash,receipt.ActualTurn,receipt.EndedFrame,receipt.Maximum));
        PublishBoundaryPingkou(frame);return SkillProgramStepOutcome.AwaitChoice;
    }
    private IReadOnlyList<PromptChoice> BoundaryPingkouChoices(ProgramSkillFrame frame)
    {
        var receipt=frame.BoundaryZhuHuan!;var choices=new List<PromptChoice>();
        if(receipt.Stage!=BoundaryZhuHuanStage.Targets)return [];
        if(receipt.Targets.Count<receipt.Maximum)
            choices.AddRange(_players.Where(p=>p.IsAlive&&p.Seat!=frame.OwnerSeat&&!receipt.Targets.Contains(p.Seat)).Select(p=>
                new PromptChoice(new($"boundary-pingkou.{frame.Id}.target.{p.Seat}"),"选择 "+p.Name,[],[p.Seat],
                    new Dictionary<string,string>{{"program-action",BoundaryPingkouAction},{"branch","target"}})));
        choices.Add(new(new($"boundary-pingkou.{frame.Id}.finish"),$"依次造成伤害（已选{receipt.Targets.Count}/{receipt.Maximum}）",[],[],
            new Dictionary<string,string>{{"program-action",BoundaryPingkouAction},{"branch","finish"}}));
        return Array.AsReadOnly(choices.ToArray());
    }
    private void PublishBoundaryPingkou(ProgramSkillFrame frame)
    {
        var choices=BoundaryPingkouChoices(frame);var skill=_contentRegistry.GetSkill(frame.SkillId);
        ReplaceRuntimeTop(frame with{Step=ResolutionFrameStep.AwaitingResponse});
        _pendingDecision=new(DecisionKind.ProgramTrigger,frame.OwnerSeat,$"平寇：本回合实际跳过{frame.BoundaryZhuHuan!.Maximum}个阶段，选择至多该数量的其他角色。",[],
            Array.AsReadOnly(choices.SelectMany(c=>c.Targets).Distinct().ToArray()),frame.OwnerSeat)
        {PromptId=CreatePromptId(),Choices=choices,IsPrivate=true,SkillPrompt=new(frame.SkillId,skill.Name,skill.Name,skill.Description)};
        _status=_players[frame.OwnerSeat].IsHuman?EngineStatus.AwaitingHumanResponse:EngineStatus.Running;AdvanceRulesAndPublishState();
    }
    [ProgramAnswer(BoundaryPingkouAction)]
    private void AnswerBoundaryPingkou(PromptChoice answer)
    {
        if(_resolutionStack.LastOrDefault()is not ProgramSkillFrame{BoundaryZhuHuan:{Stage:BoundaryZhuHuanStage.Targets}receipt}frame||
            !ValidBoundaryPingkou(frame)||_pendingDecision?.PlayerSeat!=frame.OwnerSeat||!AssistedChoicesEqual([answer],BoundaryPingkouChoices(frame).Where(c=>c.Id==answer.Id).ToArray()))
            throw new InvalidOperationException("Pingkou requires the same owner and fresh distinct living targets.");
        ClearPendingDecision();ReplaceRuntimeTop(frame=frame with{Step=ResolutionFrameStep.ResolvingEffect});
        if(answer.Parameters["branch"]=="target")
        {
            var target=answer.Targets.Single();ReplaceRuntimeTop(frame=frame with{BoundaryZhuHuan=receipt=receipt with{Targets=receipt.Targets.Append(target).ToArray()}});
            AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanTargetEvent(frame.Id,receipt.Targets.Count-1,target));PublishBoundaryPingkou(frame);return;
        }
        if(receipt.Targets.Count==0){CompleteBoundaryPingkou(frame);return;}
        ReplaceRuntimeTop(frame=frame with{BoundaryZhuHuan=receipt with{Targets=receipt.Targets.OrderBy(s=>RelativeTableOrder(frame.OwnerSeat,s)).ToArray(),Sealed=true,Stage=BoundaryZhuHuanStage.Damage}});
        AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanSealedEvent(frame.Id,receipt.Targets.Count));IssueBoundaryPingkouDamage(frame);
    }
    private void IssueBoundaryPingkouDamage(ProgramSkillFrame frame)
    {
        var receipt=frame.BoundaryZhuHuan!;
        while(receipt.Index<receipt.Targets.Count&&!_players[receipt.Targets[receipt.Index]].IsAlive)receipt=receipt with{Index=receipt.Index+1};
        ReplaceRuntimeTop(frame=frame with{BoundaryZhuHuan=receipt});
        if(receipt.Index>=receipt.Targets.Count){ObtainBoundaryPingkouEquipment(frame);return;}
        AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanDamageIssuedEvent(frame.Id,receipt.Index,receipt.Targets[receipt.Index]));
        BeginProgramSkillDamage(frame,receipt.Targets[receipt.Index],1);
    }
    private void ObtainBoundaryPingkouEquipment(ProgramSkillFrame frame)
    {
        var receipt=frame.BoundaryZhuHuan!;
        ReplaceRuntimeTop(frame=frame with{BoundaryZhuHuan=receipt with{Stage=BoundaryZhuHuanStage.Equipment}});
        if(!_players[frame.OwnerSeat].IsAlive||!EnsureDrawPile()){CompleteBoundaryPingkou(frame);return;}
        var candidates=_cardZones.CardsAt(CardLocation.DrawPile).Where(c=>EquipmentRules.IsEquipment(c.Kind)).OrderBy(c=>c.Id).ToArray();
        if(candidates.Length==0){CompleteBoundaryPingkou(frame);return;}
        var card=candidates[_random.Next(candidates.Length)];var before=_movementSequence;
        ReplaceRuntimeTop(frame=frame with{BoundaryZhuHuan=frame.BoundaryZhuHuan! with{Stage=BoundaryZhuHuanStage.Movement,EquipmentCard=card.Id,Before=before,After=before},
            PendingMovementContinuation=new(frame.OwnerSeat,0,null)});
        MoveProgramCardsFromMultipleSources([card.Id],CardLocation.Hand(frame.OwnerSeat),new(BoundaryPingkouGain),(batch,moves)=>
        {
            var active=GetActiveProgramFrame(frame.Id);ReplaceRuntimeTop(active with{BoundaryZhuHuan=active.BoundaryZhuHuan! with{After=_movementSequence,Batch=batch}});
            AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanEquipmentObtainedEvent(frame.Id,frame.OwnerSeat,card.Id,before,_movementSequence,batch));
        });
        ContinueBoundaryPingkou(frame.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume,53800)]
    private bool ContinueBoundaryPingkou(long id)
    {
        if(_resolutionStack.LastOrDefault()is not ProgramSkillFrame{BoundaryZhuHuan:{}receipt}frame||frame.Id!=id)return false;
        if(!ValidBoundaryPingkou(frame))throw new InvalidOperationException("Pingkou lost its exact actual-turn targets or issued payment.");
        if(frame.AttackAttempt is not null)return true;
        if(receipt.Stage==BoundaryZhuHuanStage.Targets){if(_pendingDecision is null)PublishBoundaryPingkou(frame);return true;}
        if(TryBeginQueuedRecoveryReplacement(id,PostEventContinuation.AwaitedProgramMovement)||TryBeginCharacterStateProgramWindow(id,CharacterStateContinuation.Program)||
            TryBeginHpChangedProgramWindow(id,PostEventContinuation.AwaitedProgramMovement)||TryBeginCardsMovedProgramWindow(id)||TryBeginAdvancedSkillsChanged(id))return true;
        frame=GetActiveProgramFrame(id);receipt=frame.BoundaryZhuHuan!;
        if(receipt.Stage==BoundaryZhuHuanStage.Movement){CompleteBoundaryPingkou(frame);return true;}
        if(receipt.Stage!=BoundaryZhuHuanStage.Damage||receipt.Index>=receipt.Targets.Count||
            ProgramEventHistory<BoundaryZhuHuanDamageIssuedEvent>().Count(e=>e.FrameId==id&&e.Index==receipt.Index&&e.Target==receipt.Targets[receipt.Index])!=1||
            ProgramEventHistory<BoundaryZhuHuanDamageReturnedEvent>().Any(e=>e.FrameId==id&&e.Index==receipt.Index))
            throw new InvalidOperationException("Pingkou damage may return only once to its exact issued target.");
        AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanDamageReturnedEvent(id,receipt.Index,receipt.Targets[receipt.Index]));
        ReplaceRuntimeTop(frame=frame with{BoundaryZhuHuan=receipt with{Index=receipt.Index+1}});IssueBoundaryPingkouDamage(frame);return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement,53800)]
    private bool ReturnBoundaryPingkouMovement(ProgramSkillFrame frame)=>frame.BoundaryZhuHuan is not null&&ContinueBoundaryPingkou(frame.Id);
    private void CompleteBoundaryPingkou(ProgramSkillFrame frame)
    {
        AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanCompletedEvent(frame.Id));
        ReplaceRuntimeTop(GetActiveProgramFrame(frame.Id)with{BoundaryZhuHuan=null,PendingMovementContinuation=null});AdvanceRuntimeProgram(frame.Id);
    }
    private bool ValidBoundaryPingkou(ProgramSkillFrame frame)=>frame.BoundaryZhuHuan is{}receipt&&frame.InstructionIndex==1&&frame.SkillId==BoundaryZhuHuanComposition.Pingkou&&
        receipt.Source==BoundaryZhuHuanSource(frame)&&receipt.GameplayHash==frame.GameplayHash&&receipt.ActualTurn==_turnNumber&&receipt.EndedFrame==frame.WindowContext?.ParentFrameId&&
        frame.WindowContext?.Window==SkillProgramTriggerWindow.AfterTurnEnded&&receipt.Maximum>0&&receipt.Maximum==BoundaryPingkouMaximum(frame.OwnerSeat)&&
        receipt.Targets.Count<=receipt.Maximum&&receipt.Targets.Distinct().Count()==receipt.Targets.Count&&receipt.Targets.All(s=>IsValidPlayerSeat(s)&&s!=frame.OwnerSeat)&&receipt.Index>=0&&receipt.Index<=receipt.Targets.Count&&
        ProgramEventHistory<BoundaryZhuHuanStartedEvent>().Count(e=>e.FrameId==frame.Id&&e.Source==receipt.Source&&e.GameplayHash==receipt.GameplayHash&&e.ActualTurn==receipt.ActualTurn&&e.EndedFrame==receipt.EndedFrame&&e.Maximum==receipt.Maximum)==1&&
        (!receipt.Sealed||ProgramEventHistory<BoundaryZhuHuanSealedEvent>().Count(e=>e.FrameId==frame.Id&&e.Count==receipt.Targets.Count)==1&&receipt.Targets.SequenceEqual(
            ProgramEventHistory<BoundaryZhuHuanTargetEvent>().Where(e=>e.FrameId==frame.Id).OrderBy(e=>RelativeTableOrder(frame.OwnerSeat,e.Target)).Select(e=>e.Target)));
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState,53800)]
    private void AssertBoundaryPingkou(ProgramSkillFrame frame)
    {
        if(frame.BoundaryZhuHuan is not{}r)return;
        if(!ValidBoundaryPingkou(frame)||frame.AttackAttempt is not null&&(r.Stage!=BoundaryZhuHuanStage.Damage||r.Index>=r.Targets.Count||
            ProgramEventHistory<BoundaryZhuHuanDamageIssuedEvent>().Count(e=>e.FrameId==frame.Id&&e.Index==r.Index&&e.Target==r.Targets[r.Index])!=1))
            throw new InvalidOperationException("Pingkou changed its original turn, frozen quota or native damage issuer.");
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments,53800)]
    private bool BoundaryPingkouPaidMovement(ProgramSkillFrame frame,SkillProgramEffect? effect,ProgramMovementContinuation pending)=>
        effect?.Op==SkillProgramEffectOp.ResolveBoundaryZhuHuanPingkou&&pending==frame.PendingMovementContinuation&&pending.SubjectSeat==frame.OwnerSeat&&pending.BeforeCount==0&&pending.CoverageResultBind is null&&
        ValidBoundaryPingkou(frame)&&frame.BoundaryZhuHuan is{Sealed:true,Stage:BoundaryZhuHuanStage.Movement}r&&r.Batch>0&&
        ProgramEventHistory<BoundaryZhuHuanEquipmentObtainedEvent>().Count(e=>e.FrameId==frame.Id&&e.OwnerSeat==frame.OwnerSeat&&e.CardId==r.EquipmentCard&&e.Before==r.Before&&e.After==r.After&&e.BatchId==r.Batch)==1&&
        _cardMovements.Where(m=>m.Sequence>r.Before&&m.Sequence<=r.After).ToArray() is[var move]&&move.CardId==r.EquipmentCard&&move.From==CardLocation.DrawPile&&move.To==CardLocation.Hand(frame.OwnerSeat)&&
        move.Reason.Value==BoundaryPingkouGain&&EquipmentRules.IsEquipment(move.CardKind);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath,53800)] [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss,53800)]
    private bool KeepIssuedBoundaryPingkou(ProgramSkillFrame frame)=>frame.BoundaryZhuHuan is{Sealed:true}&&ValidBoundaryPingkou(frame);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryZhuHuanPingkou)]
    private PromptChoice BoundaryPingkouAi(PendingDecision pending,ProgramSkillFrame frame)=>pending.Choices.FirstOrDefault(c=>c.Targets is[var target]&&
        !AreProgramDistributionAllies(_players[frame.OwnerSeat],_players[target]))??pending.Choices.Single(c=>c.Parameters["branch"]=="finish");
}
