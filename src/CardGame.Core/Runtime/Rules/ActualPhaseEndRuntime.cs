namespace CardGame.Core;

// The actual phase producer remains the sole owner of pending progress. This
// runtime owns issuance, exact typed returns and final continuation selection.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ActualPhaseEndRuntime(IProgramRuntimeFrames frames, INativeFrameRoutingFacts nativeFrames,
    IProgramRuntimePrompts prompts, IProgramRuntimeEvents events, IProgramEventJournalQueries history,
    IMatchParticipantStateAccess participants, IProgramContentQueries content, IProgramTurnQueries turn,
    ICardMovementBatchQueue movementBatches, IProgramMovementWindows movementWindows,
    IProgramWindowCandidateQueries candidates, IProgramWindowCursors cursors,
    IActualDiscardRecoveryPhaseQueries actualPhases, IActualDiscardRecoveryPhaseOperations phaseOperations,
    IProgramPhaseScheduleState schedule,
    IActualPhaseEndFacts facts, IActualPhaseEndIssuedItems issuedItems, IActualPhaseEndNativeReturns native,
    IQuHuangPhaseEndQueries quHuang)
    : IActualPhaseEndQueries, IActualPhaseEndOperations
{
    public bool Tracks => facts.HasLishiCapability || facts.HasMouSimaShi || facts.HasMouHuCheEr ||
        facts.ModeTracksActualPhaseEnds || facts.HasOlLuShi ||
        content.HasTriggerOperation(SkillProgramEffectOp.ResolveQingyaNextActualPhaseUse) ||
        content.HasTriggerOperation(SkillProgramEffectOp.ResolveNishouIssuedCurrentPhaseSwap) ||
        content.HasActivationOperation(SkillProgramEffectOp.ResolveAnniversaryWuFuJuekai) ||
        content.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiHuangZuPhaseUse) ||
        content.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiRunweiPhaseEnd);

    public void ConfirmActualDiscardPhaseExecution()
    {
        if (Tracks && actualPhases.Current() is { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase &&
            !history.Complete().OfType<ActualDiscardPhaseExecutionConfirmedEvent>().Any(e => e.Phase == phase))
            events.AdvanceFact(new ActualDiscardPhaseExecutionConfirmedEvent(phase));
    }

    public bool TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind kind, ActualPhaseEndContinuation continuation,
        int delayedEffects = 0, bool afterNormalDrawProgramsCompleted = false,
        bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false,
        long? parentFrameId = null)
    {
        if (!Tracks || actualPhases.Current() is not { } phase || phase.Kind != kind ||
            phase.TurnNumber != turn.TurnNumber || phase.ActualTurnOwnerSeat != turn.TurnOwnerSeat ||
            history.Complete().OfType<ActualPhaseEndedEvent>().Any(e => e.Phase == phase) ||
            kind == ActualDiscardRecoveryPhaseKind.Discard &&
                !history.Complete().OfType<ActualDiscardPhaseExecutionConfirmedEvent>().Any(e => e.Phase == phase)) return false;
        if (prompts.Current is not null || (parentFrameId is null ? nativeFrames.FrameCount != 0 :
                frames.Top is not DrawPhaseObligationFrame draw || draw.Id != parentFrameId ||
                draw.Stage != DrawPhaseObligationStage.ConsumeMarker || draw.Skipped || draw.DiscardRecoveryPhase != phase))
            throw new InvalidOperationException("An actual phase end lost its clean native producer boundary.");
        var inherited = Array.AsReadOnly(PendingBatches().Where(b => b.TurnNumber == turn.TurnNumber &&
            b.AwaitingProgramFrameId is null && (b.ParentFrameId is null || b.ParentFrameId == parentFrameId ||
                frames.Find(b.ParentFrameId.Value) is null))
            .Select(b => b.Id).ToArray());
        var frame = new ActualPhaseEndFrame(frames.NextId(), phase, continuation,
            Array.AsReadOnly(Array.Empty<ActualPhaseEndItem>()), inherited,
            DelayedEffects: delayedEffects, AfterNormalDrawProgramsCompleted: afterNormalDrawProgramsCompleted,
            DrawPhaseEndedProgramsCompleted: drawPhaseEndedProgramsCompleted,
            ActualDrawCompletionCompleted: actualDrawCompletionCompleted, ParentFrameId: parentFrameId);
        frames.Push(frame); frames.Advance(frame.Id); return true;
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 160, NativeFrameAutomaticPending.RequireNoneAfterFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(ActualPhaseEndFrame), 370)]
    internal void ContinueActualPhaseEnd(long frameId)
    {
        if (frames.Top is not ActualPhaseEndFrame frame || frame.Id != frameId ||
            frame.ActiveWindowFrameId is not null || prompts.Current is not null)
            throw new InvalidOperationException("An actual phase end cannot advance before its exact child returns.");
        if (!frame.ItemsCaptured)
        {
            if (movementWindows.Begin(frame.Id)) return;
            events.AdvanceFact(new ActualPhaseEndingEvent(frame.Id, frame.Phase));
            var items = Array.AsReadOnly(issuedItems.Qingya(frame.Phase).Concat(quHuang.CollectNishouPhaseEndCandidates(frame.Phase))
                .Concat(issuedItems.MouSimaShi(frame.Phase)).Concat(issuedItems.ShiHuangZu(frame.Phase))
                .Concat(issuedItems.ShiLuYuSheng(frame.Phase)).ToArray());
            frames.ReplaceTop(frame with { Items = items, ItemsCaptured = true });
            frame = (ActualPhaseEndFrame)frames.Top!;
        }
        var withLateNishou = AppendNishouLatePhaseItems(frame);
        if (!ReferenceEquals(withLateNishou, frame)) frames.ReplaceTop(frame = withLateNishou);
        if (frame.ItemIndex >= frame.Items.Count)
        {
            events.AdvanceFact(new ActualPhaseEndedEvent(frame.Id, frame.Phase));
            frames.Pop(frame.Id, ResolutionFrameKind.ActualPhaseEnd);
            ReturnActualPhaseEnd(frame); return;
        }
        var item = frame.Items[frame.ItemIndex];
        var context = new ActualPhaseEndContext(frame.Phase, item.EntitlementId, frame.Id);
        var window = new ProgramLifecycleTriggerWindowFrame(frames.NextId(), frame.Phase.ActorSeat,
            SkillProgramTriggerWindow.ActualPhaseEnded, Array.AsReadOnly(new[] { item.Candidate }),
            ProgramLifecycleContinuation.ResumeActualPhaseEnd, candidates.Capture(participants.Players[item.Candidate.OwnerSeat]))
        { ActualPhaseEnd = context };
        frames.ReplaceTop(frame with { ActiveWindowFrameId = window.Id });
        frames.Push(window); frames.Advance(window.Id);
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 10, SkillProgramTriggerWindow.ActualPhaseEnded)]
    public bool IsExactActualPhaseEndCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.ActualPhaseEnded || context.ActualPhaseEnd is not { } issued ||
            frames.OfType<ProgramLifecycleTriggerWindowFrame>().LastOrDefault() is not { } window ||
            window.Id != context.ParentFrameId || window.Window != context.Window || window.ActualPhaseEnd != issued ||
            window.Continuation != ProgramLifecycleContinuation.ResumeActualPhaseEnd || window.Candidates.Count != 1 ||
            window.CandidateIndex != 0 || window.Candidates[0] != candidate || context.OwnerSeat != candidate.OwnerSeat ||
            frames.OfType<ActualPhaseEndFrame>().LastOrDefault() is not { } parent ||
            parent.Id != issued.BoundaryFrameId || parent.ActiveWindowFrameId != window.Id || parent.Phase != issued.Phase ||
            !parent.ItemsCaptured || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count ||
            parent.Items[parent.ItemIndex] != new ActualPhaseEndItem(candidate, issued.EntitlementId) ||
            parent.Phase.TurnNumber != turn.TurnNumber || parent.Phase.ActualTurnOwnerSeat != turn.TurnOwnerSeat) return false;
        var wi = FindFrameIndex(window.Id);
        return wi > 0 && nativeFrames.FrameAt(wi - 1).Id == parent.Id;
    }

    public void ReturnActualPhaseEndWindow(ProgramLifecycleTriggerWindowFrame window)
    {
        if (frames.Top is not ActualPhaseEndFrame parent ||
            parent.ActiveWindowFrameId != window.Id || window.ActualPhaseEnd is not { } context ||
            context.BoundaryFrameId != parent.Id || context.Phase != parent.Phase ||
            window.CandidateIndex != 1 || window.Candidates.Count != 1 ||
            parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex] != new ActualPhaseEndItem(window.Candidates[0], context.EntitlementId))
            throw new InvalidOperationException("An actual phase item lost its exact mother-frame return.");
        events.AdvanceFact(new ActualPhaseEndItemResolvedEvent(parent.Id, parent.Phase,
            context.EntitlementId, window.Candidates[0].OwnerSeat));
        frames.ReplaceTop(parent with { ItemIndex = parent.ItemIndex + 1, ActiveWindowFrameId = null });
        frames.Advance(parent.Id);
    }

    [ProgramWindowReturn(SkillProgramTriggerWindow.ActualPhaseEnded)]
    internal void ReturnActualPhaseEndedBinding(ProgramSkillFrame frame, bool completed)
    {
        var context = frame.WindowContext ?? throw new InvalidOperationException("Actual phase binding lost its context.");
        if (frames.Top is not ProgramLifecycleTriggerWindowFrame parent ||
            parent.Id != context.ParentFrameId || parent.CandidateIndex != 0 || parent.Candidates.Count != 1 ||
            !RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(frame, parent.Candidates[0]) ||
            !IsExactActualPhaseEndCandidate(parent.Candidates[0], context))
            throw new InvalidOperationException("An actual phase binding lost its current issued item.");
        cursors.AdvanceProgramLifecycleCursor(parent); frames.Advance(parent.Id);
    }

    private void ReturnActualPhaseEnd(ActualPhaseEndFrame frame)
    {
        if (frame.Continuation != ActualPhaseEndContinuation.OwnedDraw &&
            (facts.Winner != Winner.None || !participants.Players[frame.Phase.ActorSeat].IsAlive))
        {
            if (frame.Continuation == ActualPhaseEndContinuation.Play)
            { native.CompletePlayPhaseAfterProgramWindow(); return; }
            if (schedule.Current is not null && native.ResumeScheduledProgramPhase()) return;
            if (facts.Winner != Winner.None) { native.AdvanceRulesAndPublishState(); return; }
            // Death during the issued phase-ending use finishes the actual turn;
            // it does not manufacture a Draw, Play or Ending phase for its corpse.
            phaseOperations.Clear();
            native.FinalizeEndTurn(frame.Phase.ActualTurnOwnerSeat); return;
        }
        switch (frame.Continuation)
        {
            case ActualPhaseEndContinuation.Preparation:
                native.BeginTurnStartAfterProgramLifecycle(frame.Phase.ActorSeat); break;
            case ActualPhaseEndContinuation.Judgment:
                native.CompleteTurnStart(frame.Phase.ActorSeat, frame.DelayedEffects); break;
            case ActualPhaseEndContinuation.Draw:
                native.CompleteTurnStartAfterDraw(frame.Phase.ActorSeat, frame.DelayedEffects,
                    frame.AfterNormalDrawProgramsCompleted, frame.DrawPhaseEndedProgramsCompleted, frame.ActualDrawCompletionCompleted); break;
            case ActualPhaseEndContinuation.Play:
                native.CompletePlayPhaseAfterProgramWindow(); break;
            case ActualPhaseEndContinuation.Discard:
                native.EndTurn(); break;
            case ActualPhaseEndContinuation.Ending:
                native.FinalizeEndTurn(frame.Phase.ActorSeat); break;
            case ActualPhaseEndContinuation.OwnedDraw:
                if (frames.Top is not DrawPhaseObligationFrame draw || draw.Id != frame.ParentFrameId ||
                    draw.DiscardRecoveryPhase != frame.Phase || draw.Stage != DrawPhaseObligationStage.ConsumeMarker)
                    throw new InvalidOperationException("Actual Draw end lost its owning phase producer.");
                frames.Advance(draw.Id); break;
            default: throw new InvalidOperationException("Unknown actual phase continuation.");
        }
    }

    internal bool HasActualPhaseEndBoundaryFrame()
    {
        if (nativeFrames.FrameCount == 0 || nativeFrames.FrameAt(0) is not (ActualPhaseEndFrame or DrawPhaseObligationFrame)) return false;
        var found = false;
        foreach (var frame in frames.OfType<ActualPhaseEndFrame>())
        {
            found = true;
            var index = FindFrameIndex(frame.Id);
            if (!Tracks || !actualPhases.IsRecorded(frame.Phase) ||
                frame.Phase.TurnNumber != turn.TurnNumber || frame.Phase.ActualTurnOwnerSeat != turn.TurnOwnerSeat ||
                frame.Items is not System.Collections.IList { IsReadOnly: true } ||
                frame.InheritedMovementBatchIds is not System.Collections.IList { IsReadOnly: true } ||
                frame.Items.Select(i => i.EntitlementId).Distinct().Count() != frame.Items.Count ||
                frame.ItemIndex < 0 || frame.ItemIndex > frame.Items.Count ||
                !frame.ItemsCaptured && (frame.Items.Count != 0 || frame.ItemIndex != 0 || frame.ActiveWindowFrameId is not null) ||
                frame.ItemsCaptured && history.Complete().OfType<ActualPhaseEndingEvent>().Count(e => e.BoundaryFrameId == frame.Id && e.Phase == frame.Phase) != 1 ||
                (frame.Continuation == ActualPhaseEndContinuation.OwnedDraw) != (frame.ParentFrameId is not null) ||
                (frame.ParentFrameId is null ? index != 0 : index < 1 || nativeFrames.FrameAt(index - 1) is not DrawPhaseObligationFrame draw ||
                    draw.Id != frame.ParentFrameId || draw.Stage != DrawPhaseObligationStage.ConsumeMarker || draw.DiscardRecoveryPhase != frame.Phase))
                throw new InvalidOperationException("An actual phase end lost its exact phase, frozen items or native parent.");
            if (frame.ActiveWindowFrameId is { } id && (frame.ItemIndex >= frame.Items.Count || index + 1 >= nativeFrames.FrameCount ||
                nativeFrames.FrameAt(index + 1) is not ProgramLifecycleTriggerWindowFrame child || child.Id != id ||
                child.ActualPhaseEnd != new ActualPhaseEndContext(frame.Phase, frame.Items[frame.ItemIndex].EntitlementId, frame.Id)))
                throw new InvalidOperationException("An actual phase end lost its exact current lifecycle child.");
            if (frame.ActiveWindowFrameId is null && index + 1 < nativeFrames.FrameCount &&
                (frame.ItemsCaptured || nativeFrames.FrameAt(index + 1) is not CardsMovedTriggerWindowFrame movement ||
                 movement.ResumeActualPhaseEndFrameId != frame.Id || !frame.InheritedMovementBatchIds.Contains(movement.Batch.Id) ||
                 movement.Batch.AwaitingProgramFrameId is not null))
                throw new InvalidOperationException("An actual phase end retained an unissued native child.");
        }
        return found;
    }

    private ActualPhaseEndFrame AppendNishouLatePhaseItems(ActualPhaseEndFrame frame)
    {
        if (!quHuang.HasCapability || !frame.ItemsCaptured) return frame;
        var extra = quHuang.CollectNishouPhaseEndCandidates(frame.Phase).Where(i => !frame.Items.Any(old => old.EntitlementId == i.EntitlementId)).ToArray();
        return extra.Length == 0 ? frame : frame with { Items = Array.AsReadOnly(frame.Items.Concat(extra).ToArray()) };
    }

    private int FindFrameIndex(long id)
    {
        for (var index = 0; index < nativeFrames.FrameCount; index++)
            if (nativeFrames.FrameAt(index).Id == id) return index;
        return -1;
    }

    private IEnumerable<CardMovementBatchContext> PendingBatches()
    {
        for (var index = 0; index < movementBatches.PendingBatchCount; index++)
            yield return movementBatches.PendingBatchAt(index);
    }
}
