#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramPersistentCombatPoliciesRuntime
{
    void IPersistentCombatPolicyProgramHost.SetTurnHandLimitFromPlayDamage(ProgramSkillFrame frame) => SetProgramTurnHandLimitFromPlayDamage(frame);
    void IPersistentCombatPolicyProgramHost.GrantGameFactionAttackRangeTargets(ProgramSkillFrame frame, string factionId) => GrantProgramGameFactionAttackRangeTargets(frame, factionId);
    internal void SetProgramTurnHandLimitFromPlayDamage(ProgramSkillFrame frame)
    {
        if (frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || frame.WindowContext?.Window != SkillProgramTriggerWindow.PlayPhaseStarting)
            throw new InvalidOperationException("Damage-derived hand limit requires the owner's current play phase start.");
        var state = new GameEngine.ProgramPlayDamageHandLimit(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, _coreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerAccess._playPhaseDamageDealtByCurrentPlayer);
        _coreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess._programPlayDamageHandLimits[frame.OwnerSeat] = state;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDamageHandLimitChangedEvent(state.OwnerSeat, state.SkillId, state.TurnNumber, state.PhaseInstanceId, state.DamageCount));
    }

    internal void AccumulateProgramPlayDamageHandLimit(int sourceSeat, int amount)
    {
        if (amount <= 0 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || sourceSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || !_coreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess._programPlayDamageHandLimits.TryGetValue(sourceSeat, out var state) || state.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || state.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId)
            return;
        state = state with
        {
            DamageCount = checked(state.DamageCount + amount)
        };
        _coreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess._programPlayDamageHandLimits[sourceSeat] = state;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDamageHandLimitChangedEvent(state.OwnerSeat, state.SkillId, state.TurnNumber, state.PhaseInstanceId, state.DamageCount));
    }

    internal IEnumerable<RuleQueryContribution> ProgramDamageHandLimitContributions(CharacterState owner)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == owner.Seat && _coreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess._programPlayDamageHandLimits.TryGetValue(owner.Seat, out var state) && state.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
            yield return new FiniteRuleQueryContribution($"turn:{(state.TurnNumber)}:{(state.SkillId)}:{(state.SkillInstanceId)}:play-damage-hand-limit", SkillRuleOperation.Set, state.DamageCount);
    }

    internal void GrantProgramGameFactionAttackRangeTargets(ProgramSkillFrame frame, string factionId)
    {
        if (!_coreLordSkillProjectionsOperations.HasSkillRoleQualification(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId, Role.Lord) || frame.SelectedTargetSeats.Count is < 1 or > 2 || frame.SelectedTargetSeats.Distinct().Count() != frame.SelectedTargetSeats.Count || frame.SelectedTargetSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive))
            throw new InvalidOperationException("A game-long faction range grant requires a lord and one or two distinct living targets.");
        using var declaration = System.Text.Json.JsonDocument.Parse(System.Text.Json.JsonSerializer.Serialize(new[] { "isFaction", "eventSource", factionId }));
        var condition = ProgramExpressions.Parse(declaration.RootElement, "attack-range.faction");
        var options = new DirectedCardPolicyOptions
        {
            RequiresEffectiveSource = false, CaptureOnUse = false, Until = [], While = condition
        };
        foreach (var actor in _coreMatchParticipantsPlayersAccess._players.Where(actor => actor.Seat != frame.OwnerSeat))
            foreach (var target in frame.SelectedTargetSeats.Where(target => target != actor.Seat))
                _coreDirectedTurnCardPoliciesOperations.GrantDirectedCardPolicy(frame, actor.Seat, target, [],
                    DirectedTurnCardPolicyEffect.InAttackRange, options,
                    issuanceSlot: actor.Seat * _coreMatchParticipantsPlayersAccess._players.Count + target);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramGameFactionAttackRangeTargetsGrantedEvent(frame.OwnerSeat, frame.SkillId, factionId, Array.AsReadOnly(frame.SelectedTargetSeats.ToArray())));
    }

}
