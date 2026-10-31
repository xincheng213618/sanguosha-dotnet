#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAlternatingBenefitsRuntime
{
    internal IEnumerable<IGameEvent> CompleteProgramEventHistory() => _coreProgramEventHistoryOperations.ProgramHistory.Complete();
    void IAlternatingBenefitsProgramHost.ApplyAlternatingChoiceBenefit(ProgramSkillFrame f, SkillProgramEffect e) => ApplyAlternatingChoiceBenefit(f, e);
    SkillProgramStepOutcome IAlternatingBenefitsProgramHost.ObtainDeckCardWithConsecutiveTarget(ProgramSkillFrame f, int seat, SkillProgramEffect e) => ObtainDeckCardWithConsecutiveTarget(f, seat, e);
    internal int AlternatingOwnerTurnOrdinal(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().Count(e => e.ActorSeat == owner);
    internal AlternatingChoiceBenefitResolvedEvent? GetAlternatingChoicePredecessor(int owner, string skill, string instance, SkillProgramEffect effect)
    {
        var last = _coreProgramEventHistoryOperations.ProgramEventHistory<AlternatingChoiceBenefitResolvedEvent>().LastOrDefault(e => e.OwnerSeat == owner && e.SkillId == skill && e.SkillInstanceId == instance && e.StateId == effect.StateId);
        return effect.AlternatingHistoryScope == SkillProgramAlternatingHistoryScope.PreviousOwnerTurn && last?.OwnerTurnOrdinal != AlternatingOwnerTurnOrdinal(owner) - 1 ? null : last;
    }

    internal void ApplyAlternatingChoiceBenefit(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        var choice = frame.ChoiceBindings.Single(b => b.Name == effect.SourceBind).OptionId;
        if (choice is not ("draw" or "targets"))
            throw new InvalidOperationException("Unknown alternating benefit.");
        var last = GetAlternatingChoicePredecessor(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, effect);
        var switched = last?.PendingOptionId is { } pending && pending != choice;
        var amount = switched ? 2 : 1;
        var ordinal = AlternatingOwnerTurnOrdinal(frame.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlternatingChoiceBenefitResolvedEvent(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, effect.StateId!, choice, amount, switched ? null : choice) { OwnerTurnOrdinal = effect.AlternatingHistoryScope is null ? null : ordinal });
        if (choice == "draw")
        {
            if (effect.AlternatingDrawTiming == SkillProgramAlternatingDrawTiming.NextDrawPhase)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlternatingChoiceDrawBenefitIssuedEvent(frame.Id, frame.InstructionIndex - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), frame.GameplayHash, effect.StateId!, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, ordinal, amount));
                return;
            }

            var grant = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), SkillRuleQuery.DrawCount, SkillRuleOperation.Add, amount);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(grant));
        }
        else
            _coreRedAdditionalTargetsOperations.GrantRedAdditionalTargets(frame, amount, effect.PersistAlternatingTargetsAfterSkillLoss);
    }

    internal IReadOnlyList<ProgramAlternatingChoiceStateSnapshot>? GetAlternatingChoiceStateSnapshot(CharacterState owner)
    {
        var states = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.SelectMany(i => i.Program.Triggers.SelectMany(t => t.Effects).Where(e => e.Op == SkillProgramEffectOp.ApplyAlternatingChoiceBenefit).Select(e => new { Instance = i, Effect = e })).ToArray();
        if (states.Length == 0)
            return null;
        return states.Select(s =>
        {
            var last = _coreProgramEventHistoryOperations.ProgramEventHistory<AlternatingChoiceBenefitResolvedEvent>().LastOrDefault(e => e.OwnerSeat == owner.Seat && e.SkillId == s.Instance.SkillId && e.SkillInstanceId == s.Instance.SkillInstanceId && e.StateId == s.Effect.StateId);
            if (s.Effect.AlternatingHistoryScope == SkillProgramAlternatingHistoryScope.PreviousOwnerTurn)
            {
                var ordinal = AlternatingOwnerTurnOrdinal(owner.Seat);
                var choosing = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(f => f.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && f.OwnerSeat == owner.Seat);
                if (last?.OwnerTurnOrdinal != ordinal && !(choosing && last?.OwnerTurnOrdinal == ordinal - 1))
                    last = null;
            }

            return new ProgramAlternatingChoiceStateSnapshot(s.Instance.SkillId, s.Effect.StateId!, s.Instance.SkillInstanceId, last?.PendingOptionId, last?.PendingOptionId == "targets" ? 2 : 1, last?.PendingOptionId == "draw" ? 2 : 1);
        }).ToArray();
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 81000)]
    internal void AssignAlternatingNextDrawPhase(ActualDiscardRecoveryPhaseStartedEvent e)
    {
        if (e.Phase.Kind != ActualDiscardRecoveryPhaseKind.Draw)
            return;
        var assigned = _coreProgramEventHistoryOperations.ProgramEventHistory<AlternatingChoiceDrawPhaseAssignedEvent>().Select(x => (x.ProgramFrameId, x.InstructionIndex)).ToHashSet();
        foreach (var grant in _coreProgramEventHistoryOperations.ProgramEventHistory<AlternatingChoiceDrawBenefitIssuedEvent>().Where(x => x.Source.OwnerSeat == e.Phase.ActorSeat && x.TurnNumber == e.Phase.TurnNumber && x.Source.OwnerSeat == e.Phase.ActualTurnOwnerSeat && !assigned.Contains((x.ProgramFrameId, x.InstructionIndex))).ToArray())
        {
            if (_contentRegistryOperations._contentRegistry.GetSkill(grant.Source.SkillId).Program?.GameplayHash != grant.GameplayHash)
                throw new InvalidOperationException("An issued alternating Draw benefit lost its immutable source program.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlternatingChoiceDrawPhaseAssignedEvent(grant.ProgramFrameId, grant.InstructionIndex, e.Phase));
        }
    }

    internal int AlternatingNextDrawPhaseBonus(CharacterState player)
    {
        if (_coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Draw } phase || phase.ActorSeat != player.Seat)
            return 0;
        var assigned = _coreProgramEventHistoryOperations.ProgramEventHistory<AlternatingChoiceDrawPhaseAssignedEvent>().Where(e => e.Phase == phase).Select(e => (e.ProgramFrameId, e.InstructionIndex)).ToHashSet();
        return _coreProgramEventHistoryOperations.ProgramEventHistory<AlternatingChoiceDrawBenefitIssuedEvent>().Where(e => assigned.Contains((e.ProgramFrameId, e.InstructionIndex))).Sum(e => e.Amount);
    }

    internal SkillProgramStepOutcome ObtainDeckCardWithConsecutiveTarget(ProgramSkillFrame frame, int targetSeat, SkillProgramEffect effect)
    {
        if (frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || targetSeat == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || frame.WindowContext?.Window != SkillProgramTriggerWindow.TurnEnding)
            throw new InvalidOperationException("A deck gift needs the owner's ending and a living other target.");
        var ordinal = _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().Count(e => e.ActorSeat == frame.OwnerSeat);
        var last = _coreProgramEventHistoryOperations.ProgramEventHistory<ConsecutiveTargetDeckGiftEvent>().LastOrDefault(e => e.OwnerSeat == frame.OwnerSeat && e.SkillId == frame.SkillId && e.SkillInstanceId == frame.SkillInstanceId && e.StateId == effect.StateId);
        var repeated = last is not null && last.TargetSeat == targetSeat && last.OwnerTurnOrdinal == ordinal - 1;
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().FirstOrDefault(c => effect.Suits.Contains(c.Suit) && effect.CardCategories.Any(category => CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(c.Kind, category)));
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, frame with { ChoiceBindings = frame.ChoiceBindings.Append(new ProgramChoiceResultBinding(effect.ResultBind!, repeated ? "repeat" : "new", targetSeat)).ToArray() });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ConsecutiveTargetDeckGiftEvent(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, effect.StateId!, targetSeat, ordinal, card?.Id, repeated));
        if (card is not null)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(targetSeat), new CardMoveReason("skill-program.consecutive-target.deck-gift"));
            return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
        }

        return SkillProgramStepOutcome.Continue;
    }
}
