namespace CardGame.Core;

internal sealed partial class ProgramEventHistoryIndex
{
    // These are derived versions of journal facts, never a second resolution state.
    private sealed class CardUseFactStream(int turn, int owner)
    {
        internal readonly int Turn = turn, Owner = owner;
        internal long? RetiredAt;
        internal readonly List<(long Cursor, CardUseHistoryFact Fact)> Versions = [];
    }
    private readonly Dictionary<long, CardUseFactStream> _factActions = [];
    private readonly Dictionary<long, CardUseFactStream> _factNativeUses = [];
    private readonly Dictionary<(int Turn, int Owner), List<(CardUseFactStream Stream, long FromCursor)>> _factUsesByTurn = [];
    private readonly Dictionary<(int Turn, int Owner), List<(long Cursor, CardUseHistoryFact Fact)>> _factObservations = [];
    private readonly Dictionary<(int Turn, int Owner), List<(long Cursor, CardUseHistoryFact Fact)>> _factConfirmations = [];
    private readonly Dictionary<long, List<(long Cursor, CardUseHistoryFact Fact)>> _factAcceptedActions = [];
    private readonly Dictionary<(int Turn, int Owner), List<long>> _factAcceptedByTurn = [];
    private readonly Dictionary<(int Turn, int Owner), List<(long Cursor, CardUseHistoryFact Fact)>> _factFinalizations = [];

    public long CardUseHistoryCursor => events.Count + (long)pendingEvents.Count;

    public IReadOnlyList<CardUseHistoryFact> ReadCardUseFacts(CardUseHistoryPoint point, CardUseHistoryView view)
    {
        SynchronizeNativeHistory();
        var key = (point.ActualTurn, point.TurnOwnerSeat);
        // Accepted membership/order is fixed at acceptance. Actor/Targets are observed as of the
        // cutoff; PrimaryTargets is the latest finalized/completed stage result by that cutoff.
        // Finalizations instead exposes immutable first-finalization snapshots in fact order.
        if (view == CardUseHistoryView.Accepted)
            return _factAcceptedByTurn.TryGetValue(key, out var accepted)
                ? Array.AsReadOnly(accepted.Select(id => FactAt(_factAcceptedActions[id], point.Cursor)).OfType<CardUseHistoryFact>().ToArray()) : [];
        if (view == CardUseHistoryView.Finalizations)
            return _factFinalizations.TryGetValue(key, out var finalized)
                ? Array.AsReadOnly(finalized.TakeWhile(item => item.Cursor <= point.Cursor).Select(item => item.Fact).ToArray()) : [];
        if (view is CardUseHistoryView.Observations or CardUseHistoryView.Confirmations)
            return (view == CardUseHistoryView.Confirmations ? _factConfirmations : _factObservations).TryGetValue(key, out var observations)
                ? Array.AsReadOnly(observations.TakeWhile(item => item.Cursor <= point.Cursor).Select(item => item.Fact).ToArray()) : [];
        if (!_factUsesByTurn.TryGetValue(key, out var streams)) return [];
        var result = new List<CardUseHistoryFact>();
        foreach (var membership in streams.Where(item => item.FromCursor <= point.Cursor &&
                     (item.Stream.RetiredAt is null || item.Stream.RetiredAt > point.Cursor)))
        {
            if (FactAt(membership.Stream.Versions, point.Cursor) is { } fact) result.Add(fact);
        }
        return Array.AsReadOnly(result.OrderBy(fact => fact.DeclarationCursor).ToArray());
    }

    private static CardUseHistoryFact? FactAt(IReadOnlyList<(long Cursor, CardUseHistoryFact Fact)> versions, long cursor)
    {
        var low = 0; var high = versions.Count;
        while (low < high)
        {
            var middle = low + (high - low) / 2;
            if (versions[middle].Cursor <= cursor) low = middle + 1;
            else high = middle;
        }
        return low == 0 ? null : versions[low - 1].Fact;
    }

    private void ClearCardUseFacts()
    {
        _factActions.Clear(); _factNativeUses.Clear(); _factUsesByTurn.Clear(); _factObservations.Clear(); _factConfirmations.Clear();
        _factAcceptedActions.Clear(); _factAcceptedByTurn.Clear(); _factFinalizations.Clear();
    }

    private void IndexCardUseFacts(IGameEvent payload, long cursor)
    {
        if (payload is CardResponseCompletedEvent response)
        {
            if (_factAcceptedActions.TryGetValue(response.ActionId, out var versions))
            {
                var completed = versions[^1].Fact with { Finalized = true, Completed = true, PrimaryTargets = Array.Empty<int>() };
                versions.Add((cursor, completed));
                AppendLatestStageStatus(response.ActionId, completed, cursor);
            }
            return;
        }
        if (payload is CardUseFinishedEvent finished)
        {
            if (_completedNativeActions.TryGetValue(finished.ResolutionId, out var completedActionId) &&
                _factAcceptedActions.TryGetValue(completedActionId, out var versions))
            {
                var completed = versions[^1].Fact with { Completed = true };
                versions.Add((cursor, completed));
                AppendLatestStageStatus(completedActionId, completed, cursor);
            }
            return;
        }
        int turn, owner; long? nativeId, actionId; CardActionContext? action;
        IReadOnlyList<int> targets; bool established, selfResponse = false;
        switch (payload)
        {
            case ActualCardActionRecordedEvent accepted:
                turn = accepted.ActualTurn; owner = accepted.TurnOwner;
                nativeId = null; action = accepted.Action; actionId = action.ActionId;
                targets = action.EffectiveDesignatedTargetSeats; established = false;
                selfResponse = accepted.IsSelfNativeResponseUse;
                break;
            case NativeCardUseHistoryRecordedEvent native:
                turn = native.ActualTurn ?? native.Snapshot.ActualTurn;
                owner = native.TurnOwnerSeat ?? native.Snapshot.TurnOwnerSeat;
                nativeId = native.Snapshot.NativeUseId; actionId = native.Snapshot.ActionId; action = null;
                targets = native.Snapshot.DesignatedTargets; established = false;
                break;
            case ActualCardUseTargetsFinalizedEvent finalized:
                turn = finalized.ActualTurn; owner = finalized.TurnOwner;
                nativeId = finalized.NativeUseId; action = finalized.Action; actionId = action.ActionId;
                targets = action.EffectiveDesignatedTargetSeats; established = true;
                break;
            case ActualCardUseTargetsCompletedEvent completed:
                turn = completed.ActualTurn; owner = completed.TurnOwner;
                nativeId = completed.NativeUseId; action = completed.Action; actionId = action.ActionId;
                targets = action.EffectiveDesignatedTargetSeats; established = true;
                break;
            case ActualCardUseActionUpdatedEvent updated:
                turn = updated.ActualTurn; owner = updated.TurnOwner;
                nativeId = updated.NativeUseId; action = updated.Action; actionId = action.ActionId;
                targets = action.EffectiveDesignatedTargetSeats; established = false;
                break;
            default: return;
        }
        _factActions.TryGetValue(actionId ?? -1, out var acceptedStream);
        _factNativeUses.TryGetValue(nativeId ?? -1, out var nativeStream);
        var stream = nativeStream ?? acceptedStream;
        if (stream is null)
        {
            stream = new(turn, owner);
            if (!_factUsesByTurn.TryGetValue((turn, owner), out var uses)) _factUsesByTurn[(turn, owner)] = uses = [];
            uses.Add((stream, cursor));
        }
        if (nativeStream is not null && acceptedStream is not null && nativeStream != acceptedStream)
        {
            // The alias becomes known here. Do not rewrite either identity's
            // earlier versions or make earlier as-of reads depend on the future.
            acceptedStream.RetiredAt = cursor;
            if (stream.Turn != acceptedStream.Turn || stream.Owner != acceptedStream.Owner)
                _factUsesByTurn[(acceptedStream.Turn, acceptedStream.Owner)].Add((stream, cursor));
        }
        if (actionId is { } id) _factActions[id] = stream;
        if (nativeId is { } useId) _factNativeUses[useId] = stream;
        var previous = stream.Versions.LastOrDefault().Fact;
        var acceptedFact = acceptedStream?.Versions.FirstOrDefault().Fact;
        var actor = action?.ActorSeat ?? ((NativeCardUseHistoryRecordedEvent)payload).Snapshot.ActorSeat;
        var nativeFact = payload as NativeCardUseHistoryRecordedEvent;
        var confirmation = nativeFact?.Confirmation;
        var effectiveKind = action?.EffectiveKind ?? nativeFact!.Snapshot.EffectiveKind;
        var fact = new CardUseHistoryFact(nativeId ?? previous?.NativeUseId, actionId ?? previous?.ActionId,
            actor, acceptedFact?.DeclarerSeat ?? previous?.DeclarerSeat ?? actor, action?.Type ?? acceptedFact?.Type ?? previous?.Type ?? CardActionType.Use,
            selfResponse || acceptedFact?.IsSelfNativeResponseUse == true || previous?.IsSelfNativeResponseUse == true, targets,
            established || previous?.Established == true, stream.Versions.Count == 0 ? cursor : stream.Versions[0].Fact.DeclarationCursor)
        {
            DeclaredKind = nativeFact?.Snapshot.DeclaredKind ?? previous?.DeclaredKind,
            EffectiveKind = effectiveKind,
            Phase = confirmation?.Phase ?? previous?.Phase,
            ConfirmedTargets = confirmation?.Targets ?? previous?.ConfirmedTargets,
            PrimaryTargets = confirmation is not null
                ? ActualCardUseTargets.PrincipalTargets(effectiveKind, actor, confirmation.Targets, confirmation.Targets)
                : previous?.PrimaryTargets,
            ConfirmationCursor = confirmation is null ? previous?.ConfirmationCursor : cursor
        };
        fact = IndexAcceptedStage(payload, cursor, fact);
        stream.Versions.Add((cursor, fact));
        if (!_factObservations.TryGetValue((turn, owner), out var observations)) _factObservations[(turn, owner)] = observations = [];
        observations.Add((cursor, fact));
        if (confirmation is not null)
        {
            if (!_factConfirmations.TryGetValue((turn, owner), out var confirmations)) _factConfirmations[(turn, owner)] = confirmations = [];
            // A confirmation observes its original native target payload. It
            // does not replace the latest stream's designated-target relation.
            confirmations.Add((cursor, fact with { Targets = confirmation.Targets }));
        }
    }

    private CardUseHistoryFact IndexAcceptedStage(IGameEvent payload, long cursor, CardUseHistoryFact fact)
    {
        if (fact.ActionId is not { } actionId) return fact;
        if (payload is ActualCardActionRecordedEvent accepted && !_factAcceptedActions.ContainsKey(actionId))
        {
            var issued = fact with
            {
                DeclarerSeat = accepted.Action.ActorSeat,
                AcceptedActualTurn = accepted.ActualTurn, AcceptedTurnOwnerSeat = accepted.TurnOwner,
                AcceptedPhase = accepted.Phase, AcceptedPlayPhaseInstanceId = accepted.PlayPhaseInstanceId,
                AcceptedCursor = cursor, PrimaryTargets = null
            };
            _factAcceptedActions.Add(actionId, [(cursor, issued)]);
            if (!_factAcceptedByTurn.TryGetValue((accepted.ActualTurn, accepted.TurnOwner), out var actions))
                _factAcceptedByTurn[(accepted.ActualTurn, accepted.TurnOwner)] = actions = [];
            actions.Add(actionId);
            return WithAcceptedStage(fact, issued);
        }
        if (!_factAcceptedActions.TryGetValue(actionId, out var versions)) return fact;
        var previous = versions[^1].Fact;
        var stage = WithAcceptedStage(fact, previous) with { PrimaryTargets = previous.PrimaryTargets };
        if (payload is ActualCardUseTargetsFinalizedEvent finalized && !_finalizedActions.ContainsKey(finalized.NativeUseId))
        {
            var finalizedFact = stage with { Finalized = true, PrimaryTargets = finalized.PrimaryTargets };
            var key = (stage.AcceptedActualTurn!.Value, stage.AcceptedTurnOwnerSeat!.Value);
            if (!_factFinalizations.TryGetValue(key, out var finalizations)) _factFinalizations[key] = finalizations = [];
            finalizations.Add((cursor, finalizedFact));
            stage = _completedActualActions.ContainsKey(actionId) ? stage with { Finalized = true } : finalizedFact;
        }
        else if (payload is ActualCardUseTargetsCompletedEvent completed && !_completedActualActions.ContainsKey(actionId))
            stage = stage with { Finalized = true, Completed = _completedNativeUses.Contains(completed.NativeUseId),
                PrimaryTargets = completed.PrimaryTargets };
        versions.Add((cursor, stage));
        return WithAcceptedStage(fact, stage);
    }

    private static CardUseHistoryFact WithAcceptedStage(CardUseHistoryFact fact, CardUseHistoryFact stage) => fact with
    {
        DeclarerSeat = stage.DeclarerSeat, AcceptedActualTurn = stage.AcceptedActualTurn,
        AcceptedTurnOwnerSeat = stage.AcceptedTurnOwnerSeat, AcceptedPhase = stage.AcceptedPhase,
        AcceptedPlayPhaseInstanceId = stage.AcceptedPlayPhaseInstanceId, AcceptedCursor = stage.AcceptedCursor,
        Finalized = stage.Finalized, Completed = stage.Completed
    };

    private void AppendLatestStageStatus(long actionId, CardUseHistoryFact stage, long cursor)
    {
        if (_factActions.TryGetValue(actionId, out var stream) && stream.Versions.Count > 0)
            stream.Versions.Add((cursor, WithAcceptedStage(stream.Versions[^1].Fact, stage)));
    }

}
