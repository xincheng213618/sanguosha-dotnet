#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhongHuiMobileShiRuntime
{
    internal int ShiZhHuiCharge(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].Markers.GetValueOrDefault(PlayerMarkerKind.ShiZhHuiCharge);
    internal int ShiZhHuiOverflowCount(int seat) => !HasShiZhHuiCapability ? 0 : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiZhHuiXiezhiOverflowEvent>().Count(entry => entry.OwnerSeat == seat);
    internal int ChangeShiZhHuiCharge(long frameId, int owner, int amount, string reason, out int excess)
    {
        var before = ShiZhHuiCharge(owner);
        var raw = checked(before + amount);
        excess = Math.Max(0, raw - ShiZhHuiComposition.ChargeCapacity);
        var after = Math.Clamp(raw, 0, ShiZhHuiComposition.ChargeCapacity);
        if (before != after)
        {
            var player = _coreMatchParticipantsPlayersAccess._players[owner];
            var key = (PlayerMarkerKind.ShiZhHuiCharge, owner);
            if (after == 0)
            {
                player.Markers.Remove(PlayerMarkerKind.ShiZhHuiCharge);
                player.MarkerSourceCounts.Remove(key);
            }
            else
            {
                player.Markers[PlayerMarkerKind.ShiZhHuiCharge] = after;
                player.MarkerSourceCounts[key] = after;
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frameId, owner, PlayerMarkerKind.ShiZhHuiCharge, after - before, after, owner, reason));
        }

        return after;
    }

    // Observes completed engine facts only: slash uses per turn, deaths per round
    // and the owner's own turn start that expires a running sizi effect.
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 67700)]
    internal void ObserveShiZhHuiTurnFacts(IGameEvent payload)
    {
        if (!HasShiZhHuiCapability || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        switch (payload)
        {
            case CardUsedEvent { CardKind: var kind, SourceSeat: var actor }when CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor):
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZhHuiSlashUsedEvent(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, actor));
                break;
            case PlayerDiedEvent { VictimSeat: var victim }:
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZhHuiDeathRecordedEvent(_coreActualRoundProgressRoundNumberAccess._roundNumber, victim));
                break;
            case TurnStartedEvent { ActorSeat: var seat }when _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && ShiZhHuiActiveSizi is { } activation && activation.Source.OwnerSeat == seat:
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZhHuiSiziExpiredEvent(activation.Ordinal, "owner-turn-start"));
                break;
        }
    }

    internal int ShiZhHuiKechangLevelForResponses(int sourceSeat) => HasShiZhHuiKechang(sourceSeat) && _coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive ? ShiZhHuiKechangLevel(sourceSeat) : 0;
    internal bool ShiZhHuiKeChangCannotRespond(int sourceSeat, int responderSeat, CardKind incomingKind, IReadOnlyList<int> targetSeats) => CoreNativeCardClassificationsRuntime.IsSlashCard(incomingKind) && sourceSeat != responderSeat && targetSeats.Contains(responderSeat) && ShiZhHuiKechangLevelForResponses(sourceSeat) >= 2;
    internal void ApplyShiZhHuiFactionChange(ProgramSkillFrame frame, string faction)
    {
        if (!_coreNativeFactionChangesOperations.TracksNativeEffectiveFactionChanges || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || !_coreNativeFactionChangesOperations.AvailableEffectiveFactionIds().Contains(faction, StringComparer.Ordinal))
            throw new InvalidOperationException("Yunan requires its living native program owner and supported faction.");
        _coreNativeFactionChangesOperations.SynchronizeNativeEffectiveFactions();
        var player = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var previous = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(player);
        player.NativeEffectiveFactionOverrideId = faction;
        player.NativeEffectiveFactionWasDeclared = true;
        _coreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess._nativeFactionObservations[player.Seat] = new(faction, _coreNativeFactionChangesOperations.CaptureNativeFactionAvatar(player));
        if (string.Equals(previous, faction, StringComparison.Ordinal))
            return;
        _coreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess._nativeFactionRevision++;
        if (_coreMatchSetupSetupCompleteAccess._setupComplete && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeEffectiveFactionChangedEvent(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, player.Seat, previous, faction, frame.Id, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), "yunan"));
        _coreMatchLogOperations.AddLog("FactionChanged", $"{(player.Name)} 将势力变更为{(CoreMatchPresentationRuntime.GetFactionName(faction))}。", player.Seat);
    }
}
