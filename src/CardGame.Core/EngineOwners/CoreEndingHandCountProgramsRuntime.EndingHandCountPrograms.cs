#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreEndingHandCountProgramsRuntime
{
    void IEndingHandCountProgramHost.RecordEndHandCountAndGrantMarker(ProgramSkillFrame frame, string stateId, PlayerMarkerKind marker) => RecordProgramEndingHandCount(frame, stateId, marker);
    internal IReadOnlyList<ProgramEndingHandCountHistory> GetProgramEndingHandCountHistoryDiagnostics() => Array.AsReadOnly(_coreEndingHandCountProgramsEndingHandCountHistoriesAccess._endingHandCountHistories.Values.OrderBy(s => s.OwnerSeat).ThenBy(s => s.SkillId, StringComparer.Ordinal).ThenBy(s => s.SkillInstanceId, StringComparer.Ordinal).ThenBy(s => s.StateId, StringComparer.Ordinal).Select(s => s with { Counts = Array.AsReadOnly(s.Counts.ToArray()) }).ToArray());
    internal void RecordProgramEndingHandCount(ProgramSkillFrame frame, string stateId, PlayerMarkerKind marker)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || context.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not TurnEndingBoundaryFrame parent || parent.Id != context.ParentFrameId || parent.OwnerSeat != frame.OwnerSeat || parent.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber <= 0 || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex].Candidate is not { } candidate || candidate.OwnerSeat != frame.OwnerSeat || candidate.SkillId != frame.SkillId || candidate.BindingId != frame.TriggerId || candidate.SkillInstanceId != frame.SkillInstanceId || candidate.GameplayHash != frame.GameplayHash)
            throw new InvalidOperationException("An ending hand-count history requires its actual owner ending frame.");
        var key = (frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, stateId);
        var count = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Count;
        _coreEndingHandCountProgramsEndingHandCountHistoriesAccess._endingHandCountHistories.TryGetValue(key, out var previous);
        if (previous is not null && previous.LastActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)
            return;
        if (previous is not null && previous.LastActualTurnNumber > _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)
            throw new InvalidOperationException("Ending hand-count history cannot move its actual-turn cursor backwards.");
        var isNew = previous?.Counts.Contains(count) != true;
        var counts = Array.AsReadOnly((previous?.Counts ?? []).Append(count).Distinct().Order().ToArray());
        _coreEndingHandCountProgramsEndingHandCountHistoriesAccess._endingHandCountHistories[key] = new(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, stateId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, count, counts);
        if (isNew)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
            var source = (marker, frame.OwnerSeat);
            owner.MarkerSourceCounts[source] = checked(owner.MarkerSourceCounts.GetValueOrDefault(source) + 1);
            var total = checked(owner.Markers.GetValueOrDefault(marker) + 1);
            owner.Markers[marker] = total;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frame.Id, owner.Seat, marker, 1, total, owner.Seat, $"skill-program.{(frame.SkillId)}.{(CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame))}.marker"));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramEndingHandCountRecordedEvent(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, stateId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, count, isNew));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }
}
