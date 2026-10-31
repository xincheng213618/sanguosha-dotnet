#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAttributedMarkerEventsRuntime
{
    internal ProgramAiPublicContext CreateAttributedPaymentAiContext(CharacterState owner, SkillProgramTrigger trigger) => _coreProgramLifecycleOperations.CreateProgramAiPublicContext(owner)with
    {
        AttributedMarkerPaymentCounts = ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.ChangeParticipantMarker) && ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.SpendMarkerOrLoseHp) ? new Dictionary<PlayerMarkerKind, int>(owner.Markers) : null
    };
    internal bool HasAttributedEventOperations() => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ConsumeMarkerPreventDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AddMarkerSubjectNormalDraw) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ChangeParticipantMarker) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ClearLiushiEffect);
    internal bool IsMarkerSourceEnabled(int source, PlayerMarkerKind marker) => _coreMatchParticipantsPlayersAccess._players[source].IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[source]).GetInstanceTriggers(SkillProgramTriggerWindow.BeforeDamageApplied).Any(b => ProgramInstructionResolver.Default.Features(b.Trigger).ForOperation(SkillProgramEffectOp.ConsumeMarkerPreventDamage).Any(e => e.Marker == marker));
    internal int ActiveSubjectMarkerCount(int subject, PlayerMarkerKind marker) => !_coreMatchParticipantsPlayersAccess._players[subject].IsAlive ? 0 : _coreMatchParticipantsPlayersAccess._players[subject].MarkerSourceCounts.Where(p => p.Key.Marker == marker && IsMarkerSourceEnabled(p.Key.SkillOwnerSeat, marker)).Sum(p => p.Value);
    internal bool MarkerEventApplied(long parent, int subject, PlayerMarkerKind marker, SkillProgramEffectOp op) => _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).OfType<ProgramMarkerEventAppliedEvent>().Any(e => e.ParentFrameId == parent && e.SubjectSeat == subject && e.Marker == marker && e.Operation == op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 80)]
    internal bool CanOfferAttributedEvent(SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        foreach (var e in ProgramInstructionResolver.Default.Features(trigger).AttributedEventOperations)
            if (context.TargetSeat is not { } subject || ActiveSubjectMarkerCount(subject, e.Marker!.Value) < 1 || MarkerEventApplied(context.ParentFrameId, subject, e.Marker.Value, e.Op))
                return false;
        return true;
    }

    internal IReadOnlyList<ProgramTriggerCandidate> IncludeAttributedDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing)
    {
        var extra = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != subject.Seat).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.DrawPhaseStarting)).Where(c => ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger2(c)).HasOperation(SkillProgramEffectOp.AddMarkerSubjectNormalDraw));
        var observers = extra.ToArray();
        if (observers.Length == 0)
            return existing;
        return existing.Concat(observers).Select((candidate, index) => (candidate, index)).OrderByDescending(x => x.candidate.Priority).ThenBy(x => _coreTableOrderOperations.RelativeTableOrder(subject.Seat, x.candidate.OwnerSeat)).ThenBy(x => x.index).Select(x => x.candidate).ToArray();
    }

    internal void MutateParticipantMarker(ProgramSkillFrame frame, int seat, PlayerMarkerKind marker, int delta, bool activeOnly = false)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[seat];
        if (!target.IsAlive)
            return;
        var attributed = target.MarkerSourceCounts.Where(p => p.Key.Marker == marker).ToArray();
        if (attributed.Any(p => p.Value <= 0) || attributed.Sum(p => p.Value) != target.Markers.GetValueOrDefault(marker))
            throw new InvalidOperationException("Participant marker source totals are inconsistent.");
        if (delta > 0)
        {
            var key = (marker, frame.OwnerSeat);
            var sourceCount = checked(target.MarkerSourceCounts.GetValueOrDefault(key) + delta);
            var totalCount = checked(target.Markers.GetValueOrDefault(marker) + delta);
            target.MarkerSourceCounts[key] = sourceCount;
            target.Markers[marker] = totalCount;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frame.Id, seat, marker, delta, target.Markers[marker], frame.OwnerSeat, "program.participant-marker"));
            return;
        }

        var sources = target.MarkerSourceCounts.Where(p => p.Key.Marker == marker && (!activeOnly || IsMarkerSourceEnabled(p.Key.SkillOwnerSeat, marker))).OrderBy(p => p.Key.SkillOwnerSeat).ToArray();
        if (sources.Sum(p => p.Value) < -delta)
            throw new InvalidOperationException("The actual marker payment is no longer affordable.");
        var left = -delta;
        foreach (var source in sources)
        {
            var paid = Math.Min(left, source.Value);
            if (paid == 0)
                continue;
            if (paid == source.Value)
                target.MarkerSourceCounts.Remove(source.Key);
            else
                target.MarkerSourceCounts[source.Key] = source.Value - paid;
            var count = target.Markers[marker] - paid;
            if (count == 0)
                target.Markers.Remove(marker);
            else
                target.Markers[marker] = count;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frame.Id, seat, marker, -paid, count, source.Key.SkillOwnerSeat, "program.participant-marker"));
            left -= paid;
            if (left == 0)
                break;
        }
    }

    internal void ApplyAttributedEvent(ProgramSkillFrame frame, SkillProgramEffect effect, int targetSeat)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SkillId != frame.SkillId || active.SkillInstanceId != frame.SkillInstanceId || (!_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) && !_continuationPermissions.Value.CanContinueProgramAfterSkillLoss(active)))
            throw new InvalidOperationException("Attributed event lost its exact source.");
        if (active.WindowContext is null && effect.Op == SkillProgramEffectOp.ChangeParticipantMarker)
        {
            if (active.ActivationId is null || effect.Target != SkillProgramEffectTarget.Owner || effect.TargetReference is not null || targetSeat != active.OwnerSeat)
                throw new InvalidOperationException("An activation marker mutation requires its original owner.");
            var ownerMarker = effect.Marker!.Value;
            MutateParticipantMarker(active, targetSeat, ownerMarker,
                effect.ClearMarker ? -_coreMatchParticipantsPlayersAccess._players[targetSeat].Markers.GetValueOrDefault(ownerMarker) : effect.Amount);
            return;
        }
        var context = active.WindowContext ?? throw new InvalidOperationException("Attributed marker operation lost its lifecycle.");
        var marker = effect.Marker!.Value;
        if (effect.Op == SkillProgramEffectOp.ChangeParticipantMarker)
        {
            var lifecycleWindow = context.Window is SkillProgramTriggerWindow.AfterDamageApplied or SkillProgramTriggerWindow.DiscardPhaseEnded or SkillProgramTriggerWindow.CardUseCompleted or SkillProgramTriggerWindow.TurnEnding;
            if (context.Window is not (SkillProgramTriggerWindow.GameStarting or SkillProgramTriggerWindow.PlayPhaseStarting or SkillProgramTriggerWindow.DrawPhaseStarting or SkillProgramTriggerWindow.AfterDamageApplied or SkillProgramTriggerWindow.DiscardPhaseEnded or SkillProgramTriggerWindow.CardUseCompleted or SkillProgramTriggerWindow.TurnEnding) || effect.Target == SkillProgramEffectTarget.SelectedTarget && (context.Window != SkillProgramTriggerWindow.PlayPhaseStarting || active.SelectedTargetSeats is not [var selected] || selected != targetSeat || selected == active.OwnerSeat) || lifecycleWindow && effect.TargetReference is { } markerReference && markerReference.Kind is not (ProgramParticipantRef.EventTarget or ProgramParticipantRef.EventSource))
                throw new InvalidOperationException("Illegal marker participant.");
            if (_resolutionStackOperations._resolutionStack.Count < 2 || (context.Window == SkillProgramTriggerWindow.PlayPhaseStarting ? _resolutionStackOperations._resolutionStack[^2] is not PlayPhaseStartingBoundaryFrame playing || playing.Id != context.ParentFrameId || playing.OwnerSeat != active.OwnerSeat : context.Window == SkillProgramTriggerWindow.AfterDamageApplied ? _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger is not { } damage || damage.Id != context.ParentFrameId : context.Window == SkillProgramTriggerWindow.CardUseCompleted ? _resolutionStackOperations._resolutionStack[^2] is not ProgramCardTriggerWindowFrame cardUse || cardUse.Id != context.ParentFrameId : context.Window == SkillProgramTriggerWindow.TurnEnding ? _resolutionStackOperations._resolutionStack[^2] is not TurnEndingBoundaryFrame ending || ending.Id != context.ParentFrameId : _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame lifecycle || lifecycle.Id != context.ParentFrameId || lifecycle.Window != context.Window || context.Window == SkillProgramTriggerWindow.DrawPhaseStarting && lifecycle.OwnerSeat != active.OwnerSeat))
                throw new InvalidOperationException("Marker mutation lost its exact lifecycle parent.");
            MutateParticipantMarker(active, targetSeat, marker, effect.ClearMarker ? -_coreMatchParticipantsPlayersAccess._players[targetSeat].Markers.GetValueOrDefault(marker) : effect.Amount);
            return;
        }

        var subject = context.TargetSeat ?? throw new InvalidOperationException("Marker event lost its actual subject.");
        if (ActiveSubjectMarkerCount(subject, marker) < 1 || MarkerEventApplied(context.ParentFrameId, subject, marker, effect.Op))
            return;
        if (effect.Op == SkillProgramEffectOp.ConsumeMarkerPreventDamage)
        {
            if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame parent || parent.Id != context.ParentFrameId || parent.TargetSeat != subject || parent.Prevented)
                throw new InvalidOperationException("Marker prevention lost its actual damage parent.");
            MutateParticipantMarker(active, subject, marker, -1, true);
            _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(active);
        }
        else
        {
            if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.Window != SkillProgramTriggerWindow.DrawPhaseStarting || parent.OwnerSeat != subject || parent.NormalDrawReplaced)
                throw new InvalidOperationException("Marker draw lost its actual normal draw parent.");
            _coreProgramLifecycleOperations.AdjustProgramNormalDraw(active, 1);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramMarkerEventAppliedEvent(context.ParentFrameId, subject, marker, effect.Op, active.OwnerSeat, active.SkillId, active.SkillInstanceId));
    }

    void IAttributedEventProgramHost.ApplyAttributedEvent(ProgramSkillFrame f, SkillProgramEffect e, int target) => ApplyAttributedEvent(f, e, target);
}
