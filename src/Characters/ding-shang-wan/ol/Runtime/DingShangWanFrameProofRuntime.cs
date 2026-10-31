namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class DingShangWanFrameProofRuntime(IProgramRuntimeFrames frames, INativeFrameRoutingFacts nativeFrames,
    IDingShangWanNativeFacts facts, IProgramTurnQueries turn, IProgramPhaseQueries phase,
    IMatchParticipantStateAccess participants, IProgramEventHistoryQueries history, ICardMovementHistoryQueries movements) : IDingShangWanRuntimeQueries
{
    private const string DingShangWanDrawReason="skill-program.ding-shang-wan.draw",FudaoGiftReason="skill-program.ding-shang-wan.fudao.give",        FudaoDiscardReason=global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.FudaoDiscardReason,FengyanGiftReason="skill-program.ding-shang-wan.fengyan.give",FengyanDiscardReason=global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.FengyanDiscardReason;
    private long PhysicalMovementSequence=>movements.Last()?.Sequence??0;
    private IEnumerable<ResolutionFrame> AllFrames=>Enumerable.Range(0,nativeFrames.FrameCount).Select(nativeFrames.FrameAt);
    private static bool IsDingShangWanDrawStage(DingShangWanStage stage)=>RuntimeSkills.RuntimeRules_ding_shang_wan_ol_DingShangWan.IsDingShangWanDrawStage(stage);
    public CardConversionSource DingShangWanSource(ProgramSkillFrame f)=>new(f.SkillId,RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId);
    private FudaoHandRecordedEvent? DingShangWanRecord(int owner,string skill,string hash)=>history.Read<FudaoHandRecordedEvent>().SingleOrDefault(e=>e.Source.OwnerSeat==owner&&e.Source.SkillId==skill&&e.GameplayHash==hash);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification,26700)]
    private bool CanOffer(ProgramTriggerLegalityContext c)=>CanOfferDingShangWan(c.Candidate,c.Trigger,c.Context);
    public bool CanOfferDingShangWan(ProgramTriggerCandidate c,SkillProgramTrigger t,ProgramSkillWindowContext context)
    {
        if(t.Effects is not [var effect]||!DingShangWanComposition.IsOperation(effect.Op))return true;
        if(!DingShangWanParent(c,context))return false;
        if(effect.Op==SkillProgramEffectOp.InitializeFudaoHandRecord)return context.Window==SkillProgramTriggerWindow.GameStarting&&phase.Phase==TurnPhase.NotStarted&&
            !history.Read<DingShangWanStartedEvent>().Any(e=>e.Source.OwnerSeat==c.OwnerSeat&&e.Source.SkillId==c.SkillId&&e.Operation==effect.Op);
        if(effect.Op==SkillProgramEffectOp.DrawFudaoRecordedPair)return context.Window==SkillProgramTriggerWindow.AfterTurnEnded&&context.TargetSeat is{}ended&&participants.At(ended).IsAlive&&
            DingShangWanRecord(c.OwnerSeat,c.SkillId,c.GameplayHash)is{}record&&context.Facts?.EventTargetHandCount==record.HandCount;
        return DingShangWanCounterpart(c.OwnerSeat,context)is not null;
    }
    public int? DingShangWanCounterpart(int owner,ProgramSkillWindowContext c)=>c.Window switch
    {
        SkillProgramTriggerWindow.AfterDamageApplied when c.TargetSeat==owner&&c.Amount>0&&c.DamageFrameId is not null&&c.SourceSeat is{}s&&s!=owner&&facts.ValidSeat(s)&&facts.CurrentDamageHasSource=>s,
        SkillProgramTriggerWindow.CardResponseCompleted when c.CardUse is{ActorSeat:var actor,CardActionId:var action}&&actor==owner&&c.TargetSeat is{}s&&s!=owner&&facts.ValidSeat(s)&&
            frames.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w=>w.Id==c.ParentFrameId)is{ResponseCompletion:{CostsDrained:true}}w&&w.Action.ActionId==action&&w.Action.OpponentSeat==s&&facts.ValidResponseCompletionWindow(w)=>s,
        _=>null
    };
    private bool DingShangWanParent(ProgramTriggerCandidate candidate,ProgramSkillWindowContext c)
    {
        if(c.OwnerSeat!=candidate.OwnerSeat)return false;
        var parent=AllFrames.SingleOrDefault(f=>f.Id==c.ParentFrameId);
        return parent switch
        {
            ProgramLifecycleTriggerWindowFrame w=>c.Window==SkillProgramTriggerWindow.GameStarting&&w.Window==c.Window&&w.Continuation==ProgramLifecycleContinuation.CompleteGameStarting&&c.SourceSeat==w.OwnerSeat&&c.TargetSeat==w.OwnerSeat&&w.CandidateIndex>=0&&w.CandidateIndex<w.Candidates.Count&&w.Candidates[w.CandidateIndex]==candidate,
            DeferredTurnEndFrame w=>c.Window==SkillProgramTriggerWindow.AfterTurnEnded&&facts.IsActualAfterTurnEndedParent(w)&&facts.AfterTurnEndedCandidate(w)==candidate&&c.SourceSeat==w.OwnerSeat&&c.TargetSeat==w.OwnerSeat&&c.OccurrenceIndex==candidate.OccurrenceIndex&&c.Facts==w.AfterTurnEnded!.Items[w.AfterTurnEnded.ItemIndex].Facts,
            DamageTriggerWindowFrame w=>c.Window==SkillProgramTriggerWindow.AfterDamageApplied&&w.ParentFrameId==c.DamageFrameId&&w.CandidateIndex>=0&&w.CandidateIndex<w.Candidates.Count&&w.Candidates[w.CandidateIndex].ToProgramCandidate()==candidate,
            ProgramCardTriggerWindowFrame w=>c.Window==SkillProgramTriggerWindow.CardResponseCompleted&&w.ResponseCompletion is{CostsDrained:true}&&w.CandidateIndex>=0&&w.CandidateIndex<w.Candidates.Count&&RuntimeSkills.RuntimeRules_Core_CardActions.ToSharedCandidate(w.Candidates[w.CandidateIndex])==candidate&&facts.ValidResponseCompletionWindow(w),
            _=>false
        };
    }
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState,26700)]
    public void AssertDingShangWan(ProgramSkillFrame f)
    {
        if(f.DingShangWan is not{}r)return;
        if(f.TriggerId is null||f.InstructionIndex!=1||r.Instruction!=1||r.Source!=DingShangWanSource(f)||r.GameplayHash!=f.GameplayHash||
            facts.PausedEffect(f)?.Op!=r.Operation||!DingShangWanComposition.IsOperation(r.Operation)||f.WindowContext is not{}c||
            c.ParentFrameId!=r.WindowId||c.Window!=r.Window||c.DamageFrameId!=r.DamageFrameId||c.CardUse?.CardActionId!=r.ActionId||
            r.Turn!=turn.TurnNumber||r.TurnOwner!=turn.TurnOwnerSeat||!Enum.IsDefined(r.Stage)||r.Before<0||r.After<r.Before||r.After>facts.MovementSequence||
            r.Minimum<0||r.Maximum<r.Minimum||r.Selected.Count>r.Maximum||r.Selected.Distinct().Count()!=r.Selected.Count||r.Selected.Any(id=>!r.Candidates.Contains(id))||
            r.WoodenOxGrainIds.Distinct().Count()!=r.WoodenOxGrainIds.Count||r.WoodenOxGrainIds.Intersect(r.Selected).Any()||
            !DingShangWanParent(new(f.OwnerSeat,f.SkillId,f.TriggerId,f.SkillInstanceId,f.GameplayHash,facts.Trigger(f).Priority,c.OccurrenceIndex),c)||
            history.Read<DingShangWanStartedEvent>().Count(e=>e.FrameId==f.Id&&e.Source==r.Source&&e.GameplayHash==r.GameplayHash&&e.WindowId==r.WindowId&&
                e.Window==r.Window&&e.Operation==r.Operation&&e.Turn==r.Turn&&e.TurnOwner==r.TurnOwner&&e.Counterpart==r.Counterpart&&e.DamageFrameId==r.DamageFrameId&&e.ActionId==r.ActionId)!=1)
            throw new InvalidOperationException("Ding Shang Wan lost its exact native candidate, owning instruction, start fact or private selected entities.");
        if(r.Operation==SkillProgramEffectOp.InitializeFudaoHandRecord)
        {
            if(r.Window!=SkillProgramTriggerWindow.GameStarting||r.Counterpart is not null||r.Stage is not(DingShangWanStage.OpeningDraw or DingShangWanStage.OpeningGiftChoice or DingShangWanStage.OpeningGiftChildren or DingShangWanStage.OpeningDiscardChoice or DingShangWanStage.OpeningDiscardChildren)||
                DingShangWanRecord(f.OwnerSeat,f.SkillId,f.GameplayHash)is not null)throw new InvalidOperationException("Fudao cannot duplicate its game-opening record.");
        }
        else if(r.Operation==SkillProgramEffectOp.DrawFudaoRecordedPair)
        {
            if(r.Window!=SkillProgramTriggerWindow.AfterTurnEnded||r.Counterpart!=c.TargetSeat||r.Stage is not(DingShangWanStage.PairOwnerDraw or DingShangWanStage.PairTargetDraw)||
                DingShangWanRecord(f.OwnerSeat,f.SkillId,f.GameplayHash)is null)throw new InvalidOperationException("Fudao lost its original fixed count and accepted actual end.");
        }
        else if(r.Counterpart is not{}other||other==f.OwnerSeat||!facts.ValidSeat(other)||r.Stage is DingShangWanStage.OpeningDraw or DingShangWanStage.OpeningGiftChoice or DingShangWanStage.OpeningGiftChildren or DingShangWanStage.OpeningDiscardChoice or DingShangWanStage.OpeningDiscardChildren or DingShangWanStage.PairOwnerDraw or DingShangWanStage.PairTargetDraw||
            (r.Window==SkillProgramTriggerWindow.AfterDamageApplied?r.Counterpart!=c.SourceSeat||c.TargetSeat!=f.OwnerSeat||c.Amount<=0:r.Window!=SkillProgramTriggerWindow.CardResponseCompleted||r.Counterpart!=c.TargetSeat)||
            r.Stage!=DingShangWanStage.FengChoice&&(r.Branch is not("owner-draw" or "other-draw")||history.Read<DingShangWanBranchEvent>().Count(e=>e.FrameId==f.Id&&e.Branch==r.Branch&&e.Recipient==r.Counterpart)!=1))
            throw new InvalidOperationException("Fengyan lost its real foreign damage/response counterpart and committed option.");
        if(f.PendingMovementContinuation is not null)
        {
            if(history.Read<DingShangWanMovementIssuedEvent>().Count(e=>e.FrameId==f.Id&&e.Stage==r.Stage&&e.Payer==r.Payer&&e.Recipient==r.Recipient&&e.Before==r.Before&&e.After==r.After&&e.Count==r.ActualCount)!=1||
                movements.Read().Count(m=>m.Sequence>r.Before&&m.Sequence<=r.After&&(!IsDingShangWanDrawStage(r.Stage)||m.From==CardLocation.DrawPile&&m.To==CardLocation.Hand(r.Payer)&&m.Reason.Value==DingShangWanDrawReason))!=r.ActualCount||
                r.Reshuffles.Select(s=>s.BatchId).Distinct().Count()!=r.Reshuffles.Count||r.Reshuffles.Any(s=>s.ParentFrameId!=f.Id||s.Movements.Count==0||s.Before<r.Before||s.After>r.After||s.After<=s.Before||
                    s.Before!=s.Movements[0].Sequence-1||s.After!=s.Movements[^1].Sequence||s.Movements.Any(m=>m.Sequence<=s.Before||m.Sequence>s.After||m.From!=CardLocation.DiscardPile||m.To!=CardLocation.DrawPile||m.Reason!=CardMoveReasons.Reshuffle||!movements.Read().Contains(m))||
                    history.Read<DingShangWanReshuffleCapturedEvent>().Count(e=>e.FrameId==f.Id&&e.BatchId==s.BatchId&&e.Before==s.Before&&e.After==s.After&&e.Count==s.Movements.Count)!=1)||
                movements.Read().Where(m=>m.Sequence>r.Before&&m.Sequence<=r.After).Any(m=>!IsDingShangWanIssuedMovement(f,m)))
                throw new InvalidOperationException("Ding Shang Wan lost its once-issued real native movement interval.");
        }
    }
    private bool IsDingShangWanIssuedMovement(ProgramSkillFrame f,CardMovementRecord m)
    {
        var r=f.DingShangWan!;
        if(m.Sequence<=r.Before||m.Sequence>r.After||!movements.Read().Contains(m))return false;
        if(IsDingShangWanDrawStage(r.Stage)&&m.From==CardLocation.DiscardPile&&m.To==CardLocation.DrawPile&&m.Reason==CardMoveReasons.Reshuffle&&
            r.Reshuffles.Any(s=>s.ParentFrameId==f.Id&&s.Movements.Contains(m)&&history.Read<DingShangWanReshuffleCapturedEvent>().Any(e=>e.FrameId==f.Id&&e.BatchId==s.BatchId&&e.Before==s.Before&&e.After==s.After&&e.Count==s.Movements.Count)))return true;
        if(r.Stage is DingShangWanStage.FengGiftChildren or DingShangWanStage.FengDiscardChildren&&
            r.WoodenOxGrainIds.Contains(m.CardId)&&m.From==CardLocation.WoodenOxGrain(r.Payer)&&m.To==CardLocation.DiscardPile&&m.Reason==CardMoveReasons.WoodenOxGrainDiscard&&
            movements.Read().Any(ox=>ox.Sequence>r.Before&&ox.Sequence<=r.After&&ox.CardKind==CardKind.WoodenOx&&r.Selected.Contains(ox.CardId)&&
                ox.From==CardLocation.Equipment(r.Payer)&&ox.Reason.Value==(r.Stage==DingShangWanStage.FengGiftChildren?FengyanGiftReason:FengyanDiscardReason)))return true;
        return r.Stage switch
        {
            DingShangWanStage.OpeningDraw or DingShangWanStage.PairOwnerDraw or DingShangWanStage.PairTargetDraw or DingShangWanStage.FengOwnerDraw or DingShangWanStage.FengTargetDraw=>m.From==CardLocation.DrawPile&&m.To==CardLocation.Hand(r.Payer)&&m.Reason.Value==DingShangWanDrawReason,
            DingShangWanStage.OpeningGiftChildren=>r.Selected.Contains(m.CardId)&&m.From==CardLocation.Hand(f.OwnerSeat)&&m.To==CardLocation.Hand(r.Recipient!.Value)&&m.Reason.Value==FudaoGiftReason,
            DingShangWanStage.OpeningDiscardChildren=>r.Selected.Contains(m.CardId)&&m.From==CardLocation.Hand(f.OwnerSeat)&&m.To==CardLocation.DiscardPile&&m.Reason.Value==FudaoDiscardReason,
            DingShangWanStage.FengGiftChildren=>r.Selected.Contains(m.CardId)&&m.From.OwnerSeat==f.OwnerSeat&&m.From.Zone is(CardZoneKind.Hand or CardZoneKind.Equipment)&&m.To==CardLocation.Hand(r.Counterpart!.Value)&&m.Reason.Value==FengyanGiftReason,
            DingShangWanStage.FengDiscardChildren=>r.Selected.Contains(m.CardId)&&m.From.OwnerSeat==r.Counterpart&&m.From.Zone is(CardZoneKind.Hand or CardZoneKind.Equipment)&&m.To==CardLocation.DiscardPile&&m.Reason.Value==FengyanDiscardReason,
            _=>false
        };
    }
    private bool DingShangWanSilverLion(ProgramSkillFrame f)=>f.DingShangWan is{Stage:DingShangWanStage.FengGiftChildren or DingShangWanStage.FengDiscardChildren}r&&
        movements.Read().Any(m=>IsDingShangWanIssuedMovement(f,m)&&m.From==CardLocation.Equipment(r.Payer)&&m.CardKind==CardKind.SilverLion);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 690)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 630)]
    private bool IsDingShangWanRecoveryParent(ProgramSkillFrame f,RecoveryReplacementFrame recovery)=>f.DingShangWan is{}r&&DingShangWanSilverLion(f)&&
        facts.RecoveryReplacementFrameRidesOn(recovery,f)&&recovery.Return.Continuation==PostEventContinuation.AwaitedProgramMovement&&
        recovery.Attempt.SourceSeat==r.Payer&&recovery.Attempt.TargetSeat==r.Payer&&recovery.Attempt.Amount==1&&
        recovery.Attempt.Completion.Producer==RecoveryAttemptProducer.SilverLion&&
        recovery.Attempt.Completion.MoveReason?.Value==(r.Stage==DingShangWanStage.FengGiftChildren?FengyanGiftReason:FengyanDiscardReason);
    private bool DingShangWanFirstChild(ProgramSkillFrame f,ResolutionFrame child)
    {
        if(f.DingShangWan is not{}r)return false;
        AssertDingShangWan(f);
        if(r.Stage==DingShangWanStage.OpeningDraw&&f.NativeDraw is not null)return RuntimeSkills.RuntimeRules_Core_NativeDrawAttempts.NativeDrawAttemptRidesOn(child,f);
        if(f.PendingMovementContinuation is null)return false;
        if(child is CardsMovedTriggerWindowFrame moved)return RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramMovementReturnMatches(f,moved)&&
            moved.Batch.OriginOwnerSeat==f.OwnerSeat&&moved.Batch.OriginSkillId==f.SkillId&&moved.Batch.OriginSkillInstanceId==f.SkillInstanceId&&
            moved.Batch.Movements.Count>0&&moved.Batch.Movements.All(m=>IsDingShangWanIssuedMovement(f,m))&&
            (moved.Batch.Movements.All(m=>m.Reason!=CardMoveReasons.Reshuffle)||moved.Batch.ParentBatchId is null&&r.Reshuffles.Any(s=>s.BatchId==moved.Batch.Id&&s.ParentFrameId==moved.Batch.ParentFrameId&&s.Movements.SequenceEqual(moved.Batch.Movements)));
        if(child is RecoveryReplacementFrame recovery)return IsDingShangWanRecoveryParent(f,recovery);
        if(child is HpChangedTriggerWindowFrame hp)return hp.Change.ParentFrameId==f.Id&&hp.ResumeFrameId==f.Id&&hp.Continuation==PostEventContinuation.AwaitedProgramMovement&&
            hp.Change.Kind==HpChangeKind.Recovery&&hp.Change.Amount==1&&hp.Change.SourceSeat==r.Payer&&hp.Change.TargetSeat==r.Payer&&DingShangWanSilverLion(f);
        if(child is ProgramLifecycleTriggerWindowFrame{Window:SkillProgramTriggerWindow.SkillsChanged}skills)return skills.ResumeProgramFrameId==f.Id&&skills.Continuation==ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state&&state.Continuation==ProgramLifecycleContinuation.ResumeCharacterStateChange&&state.ResumeProgramFrameId==f.Id&&
            history.Read<CharacterStateChangedEvent>().Any(e=>e.Change.Id==state.Id&&e.Change.ParentFrameId==f.Id&&e.Change.TargetSeat==state.OwnerSeat&&e.Change.Window==state.Window);
    }
    public ProgramSkillFrame? DingShangWanObserverRoot()
    {
        for(var index=0;index+1<nativeFrames.FrameCount;index++)
            if(nativeFrames.FrameAt(index)is ProgramSkillFrame root&&DingShangWanFirstChild(root,nativeFrames.FrameAt(index+1))&&DingShangWanObserverSuffix(index))return root;
        return null;
    }
    private bool DingShangWanObserverSuffix(int rootIndex)
    {
        for(var child=rootIndex+2;child<nativeFrames.FrameCount;child++)
        {
            var current=nativeFrames.FrameAt(child);var parent=nativeFrames.FrameAt(child-1);
            if(!facts.MovementObserverEdge(child)&&!facts.DyingExitEdge(child)&&!facts.RecoveryReplacementFrameRidesOn(current,parent)&&
                !RuntimeSkills.RuntimeRules_Core_NativeDrawAttempts.NativeDrawAttemptRidesOn(current,parent))return false;
            if(current is DyingFrame dying&&child+1<nativeFrames.FrameCount&&facts.SuspendedOrPaidRescueSuffix(child,dying))return true;
        }
        return true;
    }
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 490)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 600)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 630)]
    private bool HasDingShangWanDamageObserver(long id)=>DingShangWanObserverRoot()is{}root&&AllFrames.Any(f=>f.Id==id&&f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame)&&nativeFrames.LastFrameIndex(id)>nativeFrames.LastFrameIndex(root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 460)]
    private bool IsDingShangWanDying()=>facts.ActiveDying is{}d&&DingShangWanObserverRoot()is{}root&&nativeFrames.LastFrameIndex(d.Id)>nativeFrames.LastFrameIndex(root.Id);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 26706)]
    public void AssertDingShangWanOpeningDraw(ProgramSkillFrame frame)
    {
        if (frame.DingShangWan is not { } receipt)
        {
            if (frame.NativeDraw?.Operation == SkillProgramEffectOp.InitializeFudaoHandRecord)
                throw new InvalidOperationException("Fudao's native request lost its owning opening receipt.");
            return;
        }
        if (receipt.Operation != SkillProgramEffectOp.InitializeFudaoHandRecord)
        {
            if (receipt.OpeningDrawOutcome is not null || frame.NativeDraw?.Operation == SkillProgramEffectOp.InitializeFudaoHandRecord)
                throw new InvalidOperationException("A later Fudao or Fengyan producer cannot borrow the opening draw outcome.");
            return;
        }
        var returned = history.Read<DingShangWanOpeningDrawReturnedEvent>().Where(fact => fact.FrameId == frame.Id).ToArray();
        if (frame.NativeDraw is { } paid)
        {
            if (receipt.Stage != DingShangWanStage.OpeningDraw || receipt.OpeningDrawOutcome is not null || returned.Length != 0 ||
                frame.PendingMovementContinuation is not null || paid.Operation != receipt.Operation ||
                paid.ReturnKind != NativeDrawAttemptReturnKind.ProgramOperation || paid.InstructionIndex != frame.InstructionIndex ||
                paid.RequestedCount != 3 || !paid.Recipients.SequenceEqual([frame.OwnerSeat]) || paid.RecipientIndex != 0 ||
                receipt.Payer != frame.OwnerSeat || receipt.ActualCount != 0 || receipt.Before != receipt.After ||
                receipt.Selected.Count != 0 || receipt.Reshuffles.Count != 0)
                throw new InvalidOperationException("Fudao lost its exact unpaid opening request and native child owner.");
            return;
        }
        if (receipt.OpeningDrawOutcome is not { } outcome || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != 3 ||
            outcome.SequenceBefore < 0 || outcome.SequenceAfter < outcome.SequenceBefore || outcome.SequenceAfter > PhysicalMovementSequence ||
            outcome.ActualDrawIds.Count > 3 || outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count ||
            outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 ||
            !movements.Read().Where(move => move.Sequence > outcome.SequenceBefore && move.Sequence <= outcome.SequenceAfter &&
                move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(frame.OwnerSeat) && move.Reason.Value == DingShangWanDrawReason)
                .OrderBy(move => move.Sequence).Select(move => move.CardId).SequenceEqual(outcome.ActualDrawIds) ||
            returned is not [var actual] || actual != new DingShangWanOpeningDrawReturnedEvent(frame.Id, frame.OwnerSeat, 3,
                outcome.AttemptFrameId, outcome.SequenceBefore, outcome.SequenceAfter, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count) ||
            receipt.Stage == DingShangWanStage.OpeningDraw && (frame.PendingMovementContinuation is null ||
                receipt.Before != outcome.SequenceBefore || receipt.After != outcome.SequenceAfter || receipt.ActualCount != outcome.ActualDrawIds.Count))
            throw new InvalidOperationException("Fudao lost its once-returned physical opening outcome.");
        if (outcome.AttemptFrameId is { } attempt)
        {
            if (history.Read<NativeDrawAttemptStartedEvent>().Where(fact => fact.Context.AttemptFrameId == attempt).ToArray() is not [var start] ||
                start.Context.ParentFrameId != frame.Id || start.Context.RecipientSeat != frame.OwnerSeat || start.Context.RequestedCount != 3 ||
                start.Context.Reason.Value != DingShangWanDrawReason || start.Context.ActualTurnNumber != receipt.Turn ||
                start.Context.ActualTurnOwnerSeat != receipt.TurnOwner || start.Context.SequenceBefore > outcome.SequenceBefore ||
                start.Return != new NativeDrawAttemptReturn(NativeDrawAttemptReturnKind.ProgramOperation, frame.InstructionIndex, 0) ||
                history.Read<NativeDrawAttemptDecidedEvent>().Count(fact => fact.AttemptFrameId == attempt && fact.ParentFrameId == frame.Id &&
                    fact.RecipientSeat == frame.OwnerSeat && fact.RequestedCount == 3 && fact.Replaced == (outcome.Replacement is not null)) != 1)
                throw new InvalidOperationException("Fudao retained an opening outcome from a foreign native request.");
        }
        else if (outcome.Replacement is not null || history.Read<NativeDrawAttemptStartedEvent>().Any(fact => fact.Context.ParentFrameId == frame.Id))
            throw new InvalidOperationException("Fudao cannot invent or omit a paid opening draw request.");
    }
}
