namespace CardGame.Core;

[ProgramRuntimeModule,ProgramCapability]
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
}
