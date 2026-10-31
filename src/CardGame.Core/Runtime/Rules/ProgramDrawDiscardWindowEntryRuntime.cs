namespace CardGame.Core;

internal interface IGuanduDrawWindowCandidates : IProgramSelectionQuery
{
    IReadOnlyList<ProgramTriggerCandidate> IncludeGuanduForeignDrawObservers(CharacterState subject,
        IReadOnlyList<ProgramTriggerCandidate> existing,
        out IReadOnlyDictionary<int, SkillProgramTriggerFacts>? participantFacts);
}

/// <summary>Prepares Draw and Discard lifecycle windows on their original scheduler frames.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramDrawDiscardWindowEntryRuntime(IProgramRuntimeFrames frames,
    IProgramRuntimePrompts prompts, IProgramContentQueries content, IMatchParticipantStateAccess participants, IProgramBindingFacts bindings,
    IProgramLifecycleFacts lifecycle, IProgramTurnQueries turn, IProgramPhaseQueries phase,
    IProgramWindowCandidateQueries candidatesQuery,
    IProgramDrawDiscardWindowEntryFacts facts, IProgramDrawDiscardWindowObservers observers,
    IActualPhaseEndOperations phaseEnds) : IGuanduDrawWindowCandidates
{
    internal static bool IsGuanduForeignDrawTrigger(SkillProgramTrigger trigger) =>
        RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.IsGuanduForeignDrawTrigger(trigger);

    public IReadOnlyList<ProgramTriggerCandidate> IncludeGuanduForeignDrawObservers(CharacterState subject,
        IReadOnlyList<ProgramTriggerCandidate> existing,
        out IReadOnlyDictionary<int, SkillProgramTriggerFacts>? participantFacts)
    {
        participantFacts = null;
        if (!facts.IsGuanduActivity || !content.HasTriggerOperation(SkillProgramEffectOp.ResolveGuanduJieliang)) return existing;
        var captured = participants.Players.Where(player => player.IsAlive)
            .ToDictionary(player => player.Seat, candidatesQuery.Capture);
        var extra = participants.Players.Where(player => player.IsAlive && player.Seat != subject.Seat).SelectMany(player =>
            candidatesQuery.Eligible(player, SkillProgramTriggerWindow.DrawPhaseStarting, captured[player.Seat])
                .Where(candidate => IsGuanduForeignDrawTrigger(bindings.Trigger(candidate)))).ToArray();
        if (extra.Length == 0) return existing;
        participantFacts = new System.Collections.ObjectModel.ReadOnlyDictionary<int, SkillProgramTriggerFacts>(captured);
        return Array.AsReadOnly(existing.Concat(extra).Distinct().OrderByDescending(candidate => candidate.Priority)
            .ThenBy(candidate => facts.RelativeTableOrder(subject.Seat, candidate.OwnerSeat))
            .ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal)
            .ThenBy(candidate => candidate.BindingId, StringComparer.Ordinal).ToArray());
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.ForeignDraw, 20)]
    private bool IsGuanduForeignDrawContext(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        facts.IsGuanduActivity && IsGuanduForeignDrawTrigger(bindings.Trigger(candidate)) &&
        context.Window == SkillProgramTriggerWindow.DrawPhaseStarting && context.SourceSeat == turn.CurrentSeat && context.TargetSeat == turn.CurrentSeat &&
        context.OwnerSeat == candidate.OwnerSeat && candidate.OwnerSeat != turn.CurrentSeat && phase.Phase == TurnPhase.Draw &&
        participants.At(candidate.OwnerSeat).IsAlive && participants.At(turn.CurrentSeat).IsAlive &&
        frames.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(frame => frame.Id == context.ParentFrameId) is
            { Window: SkillProgramTriggerWindow.DrawPhaseStarting, Continuation: ProgramLifecycleContinuation.CompleteDrawPhase,
                ActualPhaseSubstitution: null } parent && parent.OwnerSeat == turn.CurrentSeat &&
        parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == candidate;

    internal bool TryBeginDrawPhase(CharacterState owner, bool skipPlayPhaseAfterDraw)
    {
        if (!owner.IsAlive || owner.Seat != turn.CurrentSeat || phase.Phase != TurnPhase.Draw ||
            prompts.Current is not null || frames.Top is not null)
            return false;
        if (!facts.HasLivingBindings(SkillProgramTriggerWindow.DrawPhaseStarting) &&
            !facts.HasPendingBoundaryWurongDrawSkip(owner.Seat)) return false;
        var ownerSeat = owner.Seat;
        var captured = candidatesQuery.Capture(owner);
        if (lifecycle.HasAttributedEventOperations)
            captured = captured with { EventTargetMarkerCounts = new Dictionary<PlayerMarkerKind, int>(owner.Markers) };
        var candidates = candidatesQuery.Eligible(owner, SkillProgramTriggerWindow.DrawPhaseStarting, captured);
        candidates = observers.IncludeAttributedDrawObservers(owner, candidates);
        candidates = observers.IncludeYanPuDrawObservers(owner, candidates);
        candidates = observers.IncludeShiGyDrawObservers(owner, candidates);
        candidates = IncludeGuanduForeignDrawObservers(owner, candidates, out var participantFacts);
        if (candidates.Count == 0) return false;
        frames.Push(new ProgramLifecycleTriggerWindowFrame(
            frames.NextId(), ownerSeat, SkillProgramTriggerWindow.DrawPhaseStarting,
            candidates, ProgramLifecycleContinuation.CompleteDrawPhase, captured,
            SkipPlayPhaseAfterDraw: skipPlayPhaseAfterDraw) { ParticipantFacts = participantFacts });
        frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal bool TryBeginAfterNormalDraw(CharacterState owner)
    {
        if (!owner.IsAlive || owner.Seat != turn.CurrentSeat || phase.Phase != TurnPhase.Draw ||
            prompts.Current is not null || frames.Top is not null)
            return false;
        if (candidatesQuery.Shard(owner).GetInstanceTriggers(SkillProgramTriggerWindow.AfterNormalDraw).Count == 0)
            return false;
        var ownerSeat = owner.Seat;
        var captured = candidatesQuery.Capture(owner);
        var candidates = candidatesQuery.Eligible(owner, SkillProgramTriggerWindow.AfterNormalDraw, captured);
        if (candidates.Count == 0) return false;
        frames.Push(new ProgramLifecycleTriggerWindowFrame(
            frames.NextId(), ownerSeat, SkillProgramTriggerWindow.AfterNormalDraw,
            candidates, ProgramLifecycleContinuation.CompleteAfterNormalDraw, captured));
        frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal bool TryBeginDrawPhaseEnded(CharacterState owner, int delayedEffects)
    {
        if (!owner.IsAlive || owner.Seat != turn.CurrentSeat || phase.Phase != TurnPhase.Draw ||
            prompts.Current is not null || frames.Top is not null)
            return false;
        if (!facts.HasLivingBindings(SkillProgramTriggerWindow.DrawPhaseEnded) &&
            !facts.HasOutstandingHongjiPhase(owner, SkillProgramTriggerWindow.DrawPhaseEnded)) return false;
        var ownerSeat = owner.Seat;
        var captured = candidatesQuery.Capture(owner);
        var candidates = candidatesQuery.Eligible(owner, SkillProgramTriggerWindow.DrawPhaseEnded, captured);
        candidates = observers.IncludeRecipientCategoryGiftObservers(owner, candidates);
        candidates = observers.IncludeHongjiPhaseCandidates(owner, SkillProgramTriggerWindow.DrawPhaseEnded, candidates);
        candidates = observers.AppendAnniversaryHanSongDrawEndedCandidates(owner, candidates);
        if (candidates.Count == 0) return false;
        frames.Push(new ProgramLifecycleTriggerWindowFrame(
            frames.NextId(), ownerSeat, SkillProgramTriggerWindow.DrawPhaseEnded,
            candidates, ProgramLifecycleContinuation.CompleteDrawPhaseEnded, captured)
        { DrawPhaseEndedDelayedEffects = delayedEffects });
        frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal bool TryBeginDiscardPhase(CharacterState owner)
    {
        if (!owner.IsAlive || owner.Seat != turn.CurrentSeat || phase.Phase != TurnPhase.Discard ||
            prompts.Current is not null || frames.Top is not null)
            return false;
        if (!facts.HasLivingBindings(SkillProgramTriggerWindow.DiscardPhaseStarting)) return false;
        var ownerSeat = owner.Seat;
        var living = participants.Players.Where(player => player.IsAlive).ToArray();
        var participantFacts = living.ToDictionary(player => player.Seat, candidatesQuery.Capture);
        var captured = participantFacts[ownerSeat];
        var candidates = living.SelectMany(player =>
                candidatesQuery.Eligible(player, SkillProgramTriggerWindow.DiscardPhaseStarting,
                    participantFacts[player.Seat])
                    .Where(candidate => bindings.Trigger(candidate).TurnOwnerScope ==
                        (player.Seat == ownerSeat ? SkillProgramTurnOwnerScope.Own : SkillProgramTurnOwnerScope.OtherLiving)))
            .OrderBy(candidate => facts.RelativeTableOrder(ownerSeat, candidate.OwnerSeat))
            .ThenByDescending(candidate => candidate.Priority)
            .ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal)
            .ThenBy(candidate => candidate.BindingId, StringComparer.Ordinal).ToArray();
        if (candidates.Length == 0) return false;
        frames.Push(new ProgramLifecycleTriggerWindowFrame(
            frames.NextId(), ownerSeat, SkillProgramTriggerWindow.DiscardPhaseStarting,
            candidates, ProgramLifecycleContinuation.CompleteDiscardPhase, captured)
        { ParticipantFacts = participantFacts });
        frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal bool TryBeginDiscardPhaseEnded(CharacterState phaseOwner)
    {
        phaseEnds.ConfirmActualDiscardPhaseExecution();
        if (!phaseOwner.IsAlive || phaseOwner.Seat != turn.CurrentSeat || phase.Phase != TurnPhase.Discard ||
            prompts.Current is not null || frames.Top is not null)
            return false;
        if (!facts.HasLivingBindings(SkillProgramTriggerWindow.DiscardPhaseEnded)) return false;
        var phaseOwnerSeat = phaseOwner.Seat;
        var living = participants.Players.Where(player => player.IsAlive).ToArray();
        var participantFacts = living.ToDictionary(player => player.Seat, candidatesQuery.Capture);
        if (lifecycle.HasAttributedEventOperations)
            foreach (var seat in participantFacts.Keys.ToArray())
                participantFacts[seat] = participantFacts[seat] with
                {
                    // A foreign discard-phase end reports the discarder's markers as the event target.
                    EventTargetMarkerCounts = new Dictionary<PlayerMarkerKind, int>(phaseOwner.Markers)
                };
        var candidates = living.SelectMany(player =>
                candidatesQuery.Eligible(player, SkillProgramTriggerWindow.DiscardPhaseEnded,
                    participantFacts[player.Seat])
                    .Where(candidate => bindings.Trigger(candidate).TurnOwnerScope ==
                        (player.Seat == phaseOwnerSeat ? SkillProgramTurnOwnerScope.Own : SkillProgramTurnOwnerScope.OtherLiving)))
            .OrderBy(candidate => facts.RelativeTableOrder(phaseOwnerSeat, candidate.OwnerSeat))
            .ThenByDescending(candidate => candidate.Priority)
            .ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal)
            .ThenBy(candidate => candidate.BindingId, StringComparer.Ordinal).ToArray();
        if (candidates.Length == 0) return false;
        frames.Push(new ProgramLifecycleTriggerWindowFrame(
            frames.NextId(), phaseOwnerSeat, SkillProgramTriggerWindow.DiscardPhaseEnded,
            candidates, ProgramLifecycleContinuation.EndTurnAfterDiscardPhase,
            participantFacts[phaseOwnerSeat])
        { ParticipantFacts = participantFacts });
        frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }
}
