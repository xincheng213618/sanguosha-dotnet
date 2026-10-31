namespace CardGame.Core;

// The original foreign-turn frame owns the frozen candidate roster and cursor.
// This runtime only retains match-local primitives and typed native returns.
[ProgramRuntimeModule]
internal sealed class ForeignActualTurnWindowRuntime(IProgramRuntimeFrames frames,    IProgramRuntimeEvents events,IProgramRuntimePrompts prompts,IProgramContentQueries content,    IProgramTurnQueries turn,ISelectionParticipantQueries participants,IProgramBindingFacts bindings,    IProgramBindingActions actions,IForeignActualTurnWindowFacts facts,IForeignActualTurnWindowReturns returns)
{
    private bool HasForeignTurnContests =>
        content.HasTriggerWindow(SkillProgramTriggerWindow.OtherActualTurnStarted) ||
        content.HasTriggerOperation(SkillProgramEffectOp.ResolveForeignTurnPindian) ||
        content.HasTriggerOperation(SkillProgramEffectOp.ResolveShenTaishiCiPowei) ||
        content.HasTriggerOperation(SkillProgramEffectOp.ResolveHappyTaishiCiChongwei) ||
        content.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryZhangLiaoCuixi) ||
        content.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryXiahouEnShiwu);

    internal bool TryBegin(int turnOwnerSeat)
    {
        if (!HasForeignTurnContests || facts.Winner != Winner.None || !participants.IsAlive(turnOwnerSeat)) return false;
        if (frames.Top is not null || prompts.Current is not null || turnOwnerSeat != turn.CurrentSeat)
            throw new InvalidOperationException("A foreign turn-start contest requires its clean actual turn boundary.");
        var entries = new List<(ProgramTriggerCandidate Candidate, SkillProgramTriggerFacts Facts)>();
        foreach (var ownerSeat in participants.Seats.Where(seat => participants.IsAlive(seat) && seat != turnOwnerSeat))
        {
            var frozen = facts.CaptureTriggerFacts(ownerSeat);
            entries.AddRange(facts.CollectCandidates(ownerSeat, frozen).Select(candidate => (candidate, frozen)));
        }
        if (entries.Count == 0) return false;
        var sorted = entries.OrderByDescending(entry => entry.Candidate.Priority)
            .ThenBy(entry => facts.RelativeTableOrder(0, entry.Candidate.OwnerSeat))
            .ThenBy(entry => entry.Candidate.SkillId, StringComparer.Ordinal)
            .ThenBy(entry => entry.Candidate.SkillInstanceId).ToArray();
        var id = frames.NextId();
        frames.Push(new ForeignActualTurnStartWindowFrame(id, turn.TurnNumber, turnOwnerSeat,
            sorted.Select(entry => entry.Candidate).ToArray(), sorted.Select(entry => new ProgramSkillWindowContext(
                SkillProgramTriggerWindow.OtherActualTurnStarted, id, entry.Candidate.OwnerSeat,
                SourceSeat: turnOwnerSeat, TargetSeat: turnOwnerSeat,
                OccurrenceIndex: entry.Candidate.OccurrenceIndex, Facts: entry.Facts)).ToArray()));
        frames.AdvanceTop<ForeignActualTurnStartWindowFrame>();
        return true;
    }

    [NativeFrameRoute(typeof(ForeignActualTurnStartWindowFrame), 220)]
    internal void ContinueWindow()
    {
        while (frames.Top is ForeignActualTurnStartWindowFrame parent)
        {
            if (parent.CandidateIndex >= parent.Candidates.Count || facts.Winner != Winner.None)
            {
                frames.Pop(parent.Id, ResolutionFrameKind.ForeignActualTurnStartWindow);
                ContinueOwnActualTurnStart(parent);
                return;
            }
            var candidate = parent.Candidates[parent.CandidateIndex];
            var context = parent.Contexts[parent.CandidateIndex];
            if (!bindings.CanRun(candidate, context))
            {
                AdvanceCandidate(parent, activated: false, completed: false);
                continue;
            }
            if (!bindings.Trigger(candidate).Optional) { actions.Begin(candidate, context); return; }
            frames.ReplaceTop(parent with { Step = ResolutionFrameStep.AwaitingResponse });
            actions.Expose(candidate, context);
            return;
        }
    }

    private void ContinueOwnActualTurnStart(ForeignActualTurnStartWindowFrame parent)
    {
        if (facts.Winner != Winner.None)
        {
            if (frames.Top is null && facts.Status != EngineStatus.Completed) returns.CompleteGame();
            return;
        }
        if (!participants.IsAlive(parent.OwnerSeat)) { returns.EndTurn(); return; }
        returns.AfterForeignActualContests(parent.OwnerSeat);
    }

    internal void AdvanceCandidate(ForeignActualTurnStartWindowFrame parent, bool activated, bool completed)
    {
        var candidate = parent.Candidates[parent.CandidateIndex];
        events.AdvanceFact(new ProgramBindingResolvedEvent(parent.Id, candidate.SkillId, candidate.BindingId,
            candidate.SkillInstanceId, candidate.OwnerSeat, SkillProgramTriggerWindow.OtherActualTurnStarted,
            activated, completed));
        frames.ReplaceTop(parent with { CandidateIndex = parent.CandidateIndex + 1, Step = ResolutionFrameStep.ResolvingEffect });
    }

    internal void CompleteBinding(ProgramSkillFrame child)
    {
        if (frames.Top is not ForeignActualTurnStartWindowFrame parent ||
            parent.Id != child.WindowContext?.ParentFrameId || parent.CandidateIndex >= parent.Candidates.Count ||
            !RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(child, parent.Candidates[parent.CandidateIndex]))
            throw new InvalidOperationException("A foreign turn-start binding lost its exact typed parent.");
        frames.ReplaceTop(parent with { CandidateIndex = parent.CandidateIndex + 1, Step = ResolutionFrameStep.ResolvingEffect });
        frames.AdvanceTop<ForeignActualTurnStartWindowFrame>();
    }
}
