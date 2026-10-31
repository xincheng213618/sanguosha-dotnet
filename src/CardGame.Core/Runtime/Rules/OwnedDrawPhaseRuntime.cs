namespace CardGame.Core;

// Owns actual Draw-phase boundaries, observer issuance and the exact requested
// draw return. All pending progress remains on the original scheduler frames.
[ProgramRuntimeModule]
internal sealed class OwnedDrawPhaseRuntime(IProgramRuntimeFrames frames, INativeFrameRoutingFacts frameFacts,
    IProgramRuntimePrompts prompts, IProgramRuntimeEvents events,
    IMatchParticipantStateAccess participants, IProgramContentQueries content,
    IProgramTurnQueries turn, IProgramPhaseQueries phase, ICardMovementBatchQueue movementBatches,
    IProgramWindowCandidateQueries candidatesQuery, IProgramDrawDiscardWindowObservers observers,
    IProgramLifecycleFacts lifecycle, IGuanduDrawWindowCandidates guandu,
    IPhysicalCardDrawOperations draws, INativeDrawAttemptRuntime attempts,
    IProgramMovementWindows movementWindows, IOwnedDrawPhaseFacts facts, IOwnedDrawPhaseNativeOperations native,
    IActualDiscardRecoveryPhaseQueries actualPhases, IActualDiscardRecoveryPhaseOperations phaseOperations,
    IActualPhaseEndQueries phaseEnds, IActualPhaseEndOperations phaseEndOperations)
{
    internal bool HasDrawPhaseObligationBoundaryFrame()
    {
        if (frameFacts.FrameCount == 0 || frameFacts.FrameAt(0) is not DrawPhaseObligationFrame) return false;
        for (var index = 0; index < frameFacts.FrameCount; index++)
        {
            if (frameFacts.FrameAt(index) is not DrawPhaseObligationFrame frame) continue;
            AssertDrawPhaseAttemptProducer(frame);
            if (frame.OwnerSeat != turn.CurrentSeat || frame.ActualTurnNumber != turn.TurnNumber || phase.Phase != TurnPhase.Draw ||
                frame.DrawCount < 0 || frame.InheritedMovementBatchIds is { } inherited &&
                    (inherited.Distinct().Count() != inherited.Count || inherited is not System.Collections.IList { IsReadOnly: true }))
                throw new InvalidOperationException("An actual draw boundary lost its turn or frozen movement provenance.");
            if (index == 0 ? frame.ParentFrameId is not null || frame.IsExtra :
                !frame.IsExtra || frameFacts.FrameAt(index - 1) is not DrawPhaseObligationFrame parent ||
                parent.Id != frame.ParentFrameId || parent.Stage != DrawPhaseObligationStage.Finished)
                throw new InvalidOperationException("An extra draw boundary lost its exact finished parent phase.");
            if (frame.ActiveWindowFrameId is not { } childId) continue;
            if (index + 1 >= frameFacts.FrameCount || frameFacts.FrameAt(index + 1) is not ProgramLifecycleTriggerWindowFrame child ||
                child.Id != childId || child.ResumeDrawPhaseObligationFrameId != frame.Id || child.OwnerSeat != frame.OwnerSeat ||
                !(child.Window == SkillProgramTriggerWindow.DrawPhaseStarting && frame.Stage == DrawPhaseObligationStage.Drawing && child.Continuation == ProgramLifecycleContinuation.CompleteDrawPhase ||
                  child.Window == SkillProgramTriggerWindow.DrawPhaseEnded && frame.Stage == DrawPhaseObligationStage.ConsumeMarker && child.Continuation == ProgramLifecycleContinuation.CompleteDrawPhaseEnded ||
                  child.Window == SkillProgramTriggerWindow.DrawPhaseSkipped && frame.Stage == DrawPhaseObligationStage.Finished && frame.Skipped && child.Continuation == ProgramLifecycleContinuation.ResumeDrawPhaseObligation))
                throw new InvalidOperationException("An actual draw boundary lost its exact active lifecycle return.");
        }
        return true;
    }

    internal bool IsActualSkippedDrawContext(ProgramSkillWindowContext context) =>
        phase.Phase == TurnPhase.Draw && context.SourceSeat == turn.CurrentSeat && context.TargetSeat == turn.CurrentSeat &&
        frames.OfType<ProgramLifecycleTriggerWindowFrame>().LastOrDefault() is
            { Window: SkillProgramTriggerWindow.DrawPhaseSkipped, Continuation: ProgramLifecycleContinuation.ResumeDrawPhaseObligation,
              ResumeDrawPhaseObligationFrameId: { } ownerId } child && child.Id == context.ParentFrameId &&
        frames.OfType<DrawPhaseObligationFrame>().SingleOrDefault(f => f.Id == ownerId) is
            { Stage: DrawPhaseObligationStage.Finished, Skipped: true } parent && parent.ActiveWindowFrameId == child.Id &&
        parent.OwnerSeat == turn.CurrentSeat && parent.ActualTurnNumber == turn.TurnNumber;

    internal bool TryBeginActualDrawCompletion(CharacterState owner, int delayedEffects)
    {
        var skipped = (delayedEffects & facts.SkipDrawPhaseMask) != 0;
        if (frames.Top is not null || prompts.Current is not null || !owner.IsAlive || facts.Winner != Winner.None ||
            !(skipped && (content.HasTriggerWindow(SkillProgramTriggerWindow.DrawPhaseSkipped) || facts.TracksShanheS5PhaseSkips || facts.TracksBoundaryZhuHuanPhaseSkips) ||
              !skipped && (phaseEnds.Tracks || facts.TracksNativeDrawThresholds || facts.HasIssuedExtraDrawMarker(owner.Seat) ||
                  content.HasTriggerOperation(SkillProgramEffectOp.ResolveGangshuAttributeIncrement)))) return false;
        var inherited = Array.AsReadOnly(PendingBatches().Where(batch => batch.ParentFrameId is null &&
            batch.TurnNumber == turn.TurnNumber && batch.Movements.Count > 0 && batch.Movements.All(m =>
                m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(owner.Seat) && m.Reason == CardMoveReasons.Draw))
            .Select(batch => batch.Id).ToArray());
        var frame = new DrawPhaseObligationFrame(frames.NextId(), owner.Seat, turn.TurnNumber, false,
            delayedEffects, skipped ? DrawPhaseObligationStage.SkipObservers : DrawPhaseObligationStage.EndObservers,
            Skipped: skipped, DrawCount: PendingBatches().Where(batch => inherited.Contains(batch.Id)).Sum(batch => batch.Movements.Count))
        { InheritedMovementBatchIds = inherited, DiscardRecoveryPhase = actualPhases.Current() };
        frames.Push(frame); frames.Advance(frame.Id); return true;
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 170, NativeFrameAutomaticPending.Ignore, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(DrawPhaseObligationFrame), 350)]
    internal void ContinueDrawPhaseObligation(long frameId)
    {
        while (frames.Top is DrawPhaseObligationFrame frame && frame.Id == frameId)
        {
            if (frame.OwnerSeat != turn.CurrentSeat || frame.ActualTurnNumber != turn.TurnNumber || phase.Phase != TurnPhase.Draw ||
                frame.ActiveWindowFrameId is not null)
                throw new InvalidOperationException("An actual draw phase lost its owner, turn or exact active child.");
            if (frame.Stage == DrawPhaseObligationStage.RequestingDraw)
            {
                if (ContinueOwnedDrawAttempt(frame)) return;
                continue;
            }
            if (native.BeginRecoveryReplacement(frame.Id) ||
                native.BeginHpChangedWindow(frame.Id) ||
                movementWindows.Begin(frame.Id)) return;
            if (facts.Winner != Winner.None || !participants.Players[frame.OwnerSeat].IsAlive)
            { frames.ReplaceTop(frame with { Stage = DrawPhaseObligationStage.Finished }); frame = (DrawPhaseObligationFrame)frames.Top!; }
            switch (frame.Stage)
            {
                case DrawPhaseObligationStage.Starting:
                    frames.ReplaceTop(frame with { Stage = DrawPhaseObligationStage.Drawing });
                    if (TryBeginOwnedDrawPhaseWindow(frame.Id, SkillProgramTriggerWindow.DrawPhaseStarting)) return;
                    break;
                case DrawPhaseObligationStage.Drawing:
                    if (frame.Skipped)
                    { frames.ReplaceTop(frame with { Stage = DrawPhaseObligationStage.SkipObservers }); break; }
                    native.ConsumeDuFuRenDrawPhase(frame.OwnerSeat);
                    native.ConsumeChengGongYingDraw(frame.OwnerSeat);
                    var baseDrawCount = frame.FrozenBaseDrawCount ?? native.FreezeNativeDrawBase(participants.Players[frame.OwnerSeat], frame.NormalDrawReplaced);
                    var count = frame.NormalDrawReplaced ? 0 : Math.Max(0, checked(baseDrawCount + frame.DrawAdjustment));
                    if (facts.HasAttemptCapability && !frame.NormalDrawReplaced)
                    { BeginOwnedDrawAttempt(frame, count); break; }
                    frames.ReplaceTop(frame with { Stage = DrawPhaseObligationStage.DrawChildren, DrawCount = 0 });
                    if (!frame.NormalDrawReplaced)
                    {
                        var actual = draws.DrawCards(participants.Players[frame.OwnerSeat], count, true, new("program.extra-draw-phase.draw"));
                        var paidDraw = (DrawPhaseObligationFrame)frames.Top!;
                        frames.ReplaceTop(paidDraw with { DrawCount = actual.Count });
                    }
                    break;
                case DrawPhaseObligationStage.DrawChildren:
                    // The next dispatcher iteration has drained the actual gain/HP children.
                    frames.ReplaceTop(frame with { Stage = DrawPhaseObligationStage.EndObservers });
                    break;
                case DrawPhaseObligationStage.EndObservers:
                    frames.ReplaceTop(frame with { Stage = DrawPhaseObligationStage.ConsumeMarker });
                    if (TryBeginOwnedDrawPhaseWindow(frame.Id, SkillProgramTriggerWindow.DrawPhaseEnded)) return;
                    break;
                case DrawPhaseObligationStage.SkipObservers:
                    frames.ReplaceTop(frame with { Stage = DrawPhaseObligationStage.Finished });
                    if (content.HasTriggerWindow(SkillProgramTriggerWindow.DrawPhaseSkipped) || facts.TracksShanheS5PhaseSkips || facts.TracksBoundaryZhuHuanPhaseSkips)
                    {
                        events.AdvanceFact(new DrawPhaseSkippedEvent(frame.Id, frame.OwnerSeat, frame.ActualTurnNumber, frame.IsExtra));
                        if (TryBeginOwnedDrawPhaseWindow(frame.Id, SkillProgramTriggerWindow.DrawPhaseSkipped)) return;
                    }
                    break;
                case DrawPhaseObligationStage.ConsumeMarker:
                    if (phaseEndOperations.TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind.Draw,
                            ActualPhaseEndContinuation.OwnedDraw, parentFrameId: frame.Id)) return;
                    var policy = native.ConsumeIssuedExtraDrawMarker(frame);
                    if (policy is null)
                    { frames.ReplaceTop(frame with { Stage = DrawPhaseObligationStage.Finished }); break; }
                    phaseOperations.Start(ActualDiscardRecoveryPhaseKind.Draw, frame.OwnerSeat);
                    var child = new DrawPhaseObligationFrame(frames.NextId(), frame.OwnerSeat, frame.ActualTurnNumber,
                        true, 0, DrawPhaseObligationStage.Starting, ParentFrameId: frame.Id)
                    { DiscardRecoveryPhase = actualPhases.Current() };
                    // This phase's ended boundary has consumed its obligation. A
                    // marker issued inside the child cannot borrow this old boundary.
                    frames.ReplaceTop(frame with { Stage = DrawPhaseObligationStage.Finished });
                    frames.Push(child);
                    events.AdvanceFact(new ExtraDrawPhaseStartedEvent(child.Id, frame.Id, frame.OwnerSeat,
                        frame.ActualTurnNumber, policy.Marker, policy.SourceSeat, policy.SkillId, policy.SkillInstanceId));
                    frames.Advance(child.Id); return;
                case DrawPhaseObligationStage.Finished:
                    CompleteOwnedDrawPhase(frame); return;
                default: throw new InvalidOperationException("Unknown actual draw phase stage.");
            }
        }
    }

    private bool TryBeginOwnedDrawPhaseWindow(long frameId, SkillProgramTriggerWindow window)
    {
        if (frames.Top is not DrawPhaseObligationFrame frame || frame.Id != frameId ||
            frame.ActiveWindowFrameId is not null || prompts.Current is not null)
            throw new InvalidOperationException("A draw phase cannot replace a still-active observer window.");
        var owner = participants.Players[frame.OwnerSeat];
        IReadOnlyDictionary<int, SkillProgramTriggerFacts>? windowParticipants = null;
        var capturedFacts = candidatesQuery.Capture(owner);
        IReadOnlyList<ProgramTriggerCandidate> candidates;
        if (window == SkillProgramTriggerWindow.DrawPhaseSkipped)
        {
            var participantFacts = participants.Players.Where(p => p.IsAlive).ToDictionary(p => p.Seat, candidatesQuery.Capture);
            windowParticipants = new System.Collections.ObjectModel.ReadOnlyDictionary<int, SkillProgramTriggerFacts>(participantFacts);
            candidates = Array.AsReadOnly(participants.Players.Where(p => p.IsAlive).SelectMany(p =>
                candidatesQuery.Eligible(p, window, participantFacts[p.Seat]))
                .OrderBy(c => facts.RelativeTableOrder(frame.OwnerSeat, c.OwnerSeat)).ThenByDescending(c => c.Priority)
                .ThenBy(c => c.SkillId, StringComparer.Ordinal).ThenBy(c => c.BindingId, StringComparer.Ordinal).ToArray());
        }
        else
        {
            if (window == SkillProgramTriggerWindow.DrawPhaseStarting && lifecycle.HasAttributedEventOperations)
                capturedFacts = capturedFacts with { EventTargetMarkerCounts = new Dictionary<PlayerMarkerKind, int>(owner.Markers) };
            candidates = candidatesQuery.Eligible(owner, window, capturedFacts);
            if (window == SkillProgramTriggerWindow.DrawPhaseStarting)
            {
                candidates = observers.IncludeAttributedDrawObservers(owner, candidates);
                candidates = observers.IncludeYanPuDrawObservers(owner, candidates);
                candidates = observers.IncludeShiGyDrawObservers(owner, candidates);
                candidates = guandu.IncludeGuanduForeignDrawObservers(owner, candidates, out windowParticipants);
            }
            if (window == SkillProgramTriggerWindow.DrawPhaseEnded)
            {
                candidates = observers.IncludeRecipientCategoryGiftObservers(owner, candidates);
                candidates = observers.IncludeHongjiPhaseCandidates(owner, window, candidates);
            }
        }
        if (candidates.Count == 0) return false;
        var continuation = window switch
        {
            SkillProgramTriggerWindow.DrawPhaseStarting => ProgramLifecycleContinuation.CompleteDrawPhase,
            SkillProgramTriggerWindow.DrawPhaseEnded => ProgramLifecycleContinuation.CompleteDrawPhaseEnded,
            SkillProgramTriggerWindow.DrawPhaseSkipped => ProgramLifecycleContinuation.ResumeDrawPhaseObligation,
            _ => throw new InvalidOperationException("The owned draw phase requested an unrelated window.")
        };
        var child = new ProgramLifecycleTriggerWindowFrame(frames.NextId(), owner.Seat, window,
            candidates, continuation, capturedFacts)
        { ResumeDrawPhaseObligationFrameId = frame.Id, ParticipantFacts = windowParticipants,
          DrawPhaseEndedDelayedEffects = window == SkillProgramTriggerWindow.DrawPhaseEnded ? frame.DelayedEffects : null };
        frames.ReplaceTop(frame with { ActiveWindowFrameId = child.Id });
        frames.Push(child); frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>(); return true;
    }

    internal void ReturnDrawPhaseObligationWindow(ProgramLifecycleTriggerWindowFrame completed)
    {
        if (frames.Top is not DrawPhaseObligationFrame frame ||
            frame.Id != completed.ResumeDrawPhaseObligationFrameId || frame.ActiveWindowFrameId != completed.Id ||
            frame.OwnerSeat != completed.OwnerSeat || frame.ActualTurnNumber != turn.TurnNumber)
            throw new InvalidOperationException("The actual draw window lost its typed producer return.");
        var stage = completed.Window switch
        {
            SkillProgramTriggerWindow.DrawPhaseStarting => DrawPhaseObligationStage.Drawing,
            SkillProgramTriggerWindow.DrawPhaseEnded => DrawPhaseObligationStage.ConsumeMarker,
            SkillProgramTriggerWindow.DrawPhaseSkipped => DrawPhaseObligationStage.Finished,
            _ => throw new InvalidOperationException("A draw producer received an unrelated window return.")
        };
        if (frame.Stage != stage) throw new InvalidOperationException("The actual draw cursor was advanced before its child returned.");
        var skipped = completed.Window == SkillProgramTriggerWindow.DrawPhaseStarting && completed.ActualPhaseSubstitution is
            { SkippedPhase: SkillProgramTurnPhase.Draw } substitution && substitution.ParentFrameId == completed.Id &&
            substitution.OwnerSeat == frame.OwnerSeat && substitution.ActualTurnNumber == frame.ActualTurnNumber;
        frames.ReplaceTop(frame with { ActiveWindowFrameId = null, Stage = stage,
            NormalDrawReplaced = completed.Window == SkillProgramTriggerWindow.DrawPhaseStarting ? completed.NormalDrawReplaced : frame.NormalDrawReplaced,
            DrawAdjustment = completed.Window == SkillProgramTriggerWindow.DrawPhaseStarting ? completed.NormalDrawAdjustment : frame.DrawAdjustment,
            FrozenBaseDrawCount = completed.Window == SkillProgramTriggerWindow.DrawPhaseStarting ? completed.FrozenBaseDrawCount : frame.FrozenBaseDrawCount,
            Skipped = frame.Skipped || skipped });
        frames.Advance(frame.Id);
    }

    private void CompleteOwnedDrawPhase(DrawPhaseObligationFrame frame)
    {
        if (frame.ActiveWindowFrameId is not null) throw new InvalidOperationException("A draw phase cannot finish before its observer returns.");
        events.AdvanceFact(new ActualDrawPhaseCompletedEvent(frame.Id, frame.OwnerSeat, frame.ActualTurnNumber,
            frame.IsExtra, frame.Skipped, frame.DrawCount));
        frames.Pop(frame.Id, ResolutionFrameKind.DrawPhaseObligation);
        if (frame.ParentFrameId is { } parentId)
        {
            if (frames.Top is not DrawPhaseObligationFrame parent || parent.Id != parentId ||
                parent.OwnerSeat != frame.OwnerSeat || parent.ActualTurnNumber != frame.ActualTurnNumber ||
                parent.Stage != DrawPhaseObligationStage.Finished)
                throw new InvalidOperationException("An inserted draw phase lost its exact parent phase.");
            phaseOperations.Restore(parent);
            frames.Advance(parent.Id); return;
        }
        if (facts.Winner != Winner.None) { native.AdvanceRulesAndPublishState(); return; }
        if (!participants.Players[frame.OwnerSeat].IsAlive) { native.EndTurn(); return; }
        if (native.CompleteShanheSourceChunlaoJudgmentDraw(frame)) return;
        native.CompleteTurnStartAfterDraw(participants.Players[frame.OwnerSeat], frame.DelayedEffects);
    }

    internal bool TryBeginNormalDrawAttemptPhase(CharacterState owner, int delayed,
        bool afterStartingWindow = false, bool replaced = false, int adjustment = 0, int? frozenBase = null)
    {
        if (!facts.HasAttemptCapability || frames.Top is not null || prompts.Current is not null ||
            !owner.IsAlive || owner.Seat != turn.CurrentSeat || phase.Phase != TurnPhase.Draw ||
            (delayed & facts.SkipDrawPhaseMask) != 0 || facts.Winner != Winner.None)
            return false;
        var frame = new DrawPhaseObligationFrame(frames.NextId(), owner.Seat, turn.TurnNumber, false,
            delayed, afterStartingWindow ? DrawPhaseObligationStage.Drawing : DrawPhaseObligationStage.Starting,
            NormalDrawReplaced: replaced, DrawAdjustment: adjustment, FrozenBaseDrawCount: frozenBase)
            { DiscardRecoveryPhase = actualPhases.Current() };
        frames.Push(frame); frames.Advance(frame.Id); return true;
    }

    private void BeginOwnedDrawAttempt(DrawPhaseObligationFrame frame, int requested)
    {
        if (!facts.HasAttemptCapability || frame.Stage != DrawPhaseObligationStage.Drawing ||
            frame.NativeDraw is not null || requested < 0 || frame.NormalDrawReplaced)
            throw new InvalidOperationException("An actual draw phase cannot issue an unrelated draw request.");
        var reason = frame.IsExtra ? new CardMoveReason("program.extra-draw-phase.draw") : CardMoveReasons.Draw;
        frames.ReplaceTop(frame with { Stage = DrawPhaseObligationStage.RequestingDraw,
            NativeDraw = new NativeDrawProducerReceipt(0, requested, null, SkillProgramCardSetVisibility.Private, reason)
            { Recipients = [frame.OwnerSeat], ReturnKind = NativeDrawAttemptReturnKind.DrawPhase } });
    }

    // Like program producers, the real phase waits for its exact pre-attempt
    // return. Stolen replacement entities are never its actual DrawCount.
    private bool ContinueOwnedDrawAttempt(DrawPhaseObligationFrame frame)
    {
        var paid = frame.NativeDraw ?? throw new InvalidOperationException("An actual draw phase lost its request.");
        if (paid.ActiveAttemptId is not null && !paid.AttemptReturned)
            throw new InvalidOperationException("An actual draw phase resumed before its exact request child returned.");
        if (!paid.AttemptReturned && attempts.TryBegin(frame)) return true;
        var drawn = paid.Replacement is null && participants.Players[frame.OwnerSeat].IsAlive && facts.Winner == Winner.None
            ? draws.DrawCards(participants.Players[frame.OwnerSeat], paid.RequestedCount, true, paid.Reason)
            : Array.AsReadOnly(Array.Empty<int>());
        var active = (DrawPhaseObligationFrame)frames.Top!;
        frames.ReplaceTop(active with { NativeDraw = null, DrawCount = drawn.Count,
            Stage = DrawPhaseObligationStage.DrawChildren });
        return false;
    }

    private void AssertDrawPhaseAttemptProducer(DrawPhaseObligationFrame frame)
    {
        if (frame.NativeDraw is not { } paid)
        {
            if (frame.Stage == DrawPhaseObligationStage.RequestingDraw)
                throw new InvalidOperationException("A requesting Draw phase has no owning producer receipt.");
            return;
        }
        if (!facts.HasAttemptCapability || frame.Stage != DrawPhaseObligationStage.RequestingDraw ||
            paid.ReturnKind != NativeDrawAttemptReturnKind.DrawPhase || paid.InstructionIndex != 0 ||
            paid.Operation is not null || paid.Recipients is not System.Collections.IList { IsReadOnly: true } ||
            paid.Recipients is not [var recipient] || recipient != frame.OwnerSeat || paid.RecipientIndex != 0 ||
            paid.RequestedCount < 0 || paid.ResultBind is not null || paid.RecipientApplied ||
            paid.Reason != (frame.IsExtra ? new CardMoveReason("program.extra-draw-phase.draw") : CardMoveReasons.Draw) ||
            paid.ActiveAttemptId is null && (paid.AttemptReturned || paid.Replacement is not null) ||
            paid.Replacement is not null && !paid.AttemptReturned)
            throw new InvalidOperationException("An actual Draw phase lost its exact requested count or return contract.");
    }

    private IEnumerable<CardMovementBatchContext> PendingBatches()
    {
        for (var index = 0; index < movementBatches.PendingBatchCount; index++)
            yield return movementBatches.PendingBatchAt(index);
    }
}
