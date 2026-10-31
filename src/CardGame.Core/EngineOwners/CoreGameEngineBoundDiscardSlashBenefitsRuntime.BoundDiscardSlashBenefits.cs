#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineBoundDiscardSlashBenefitsRuntime
{
    SkillProgramStepOutcome IBoundDiscardSlashBenefitsHost.DiscardBoundCardForTurnSlashBenefits(ProgramSkillFrame frame, string sourceBind) => BeginBoundDiscardSlashBenefits(frame.Id, sourceBind);
    void IBoundDiscardSlashBenefitsHost.ScheduleFirstRoundGameUsageRefund(ProgramSkillFrame frame) => ScheduleProgramFirstRoundRefund(frame);
    internal SkillProgramStepOutcome BeginBoundDiscardSlashBenefits(long id, string bind)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var source = _coreProgramLifecycleOperations.GetProgramCardSet(frame, bind);
        if (frame.BoundDiscardSlashBenefits is not null)
            throw new InvalidOperationException("A paid discard cannot run twice.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || frame.TriggerId is not null || _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || frame.OwnerSeat != _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) || source.CardIds is not [var cardId] || source.SourceLocations is not [var from] || from.OwnerSeat != frame.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _cardZonesOperations._cardZones.GetLocation(cardId) != from)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "弃牌成本或技能来源已失效，未授予回合收益。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == cardId);
        var suit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card);
        var expectedSequence = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count == 0 ? 1 : _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundDiscardSlashBenefits = new(frame.InstructionIndex, bind, card.Id, card.Kind, from, suit, expectedSequence) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, frame.OwnerSeat, card, from, new("skill-program.bound-discard-slash.cost"));
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 770)]
    internal bool ResumeBoundDiscardSlashBenefits(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != id || frame.BoundDiscardSlashBenefits is not { } paid)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && owner.IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId))
        {
            var source = CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame);
            var range = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(_coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, id, paid.InstructionIndex - 1, source, SkillRuleQuery.AttackRange, SkillRuleOperation.Unlimited, 0);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(range));
            var policy = _turnCardUseEffectsOperations._turnCardUseEffects.GrantSlashSuitAllowance(_coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, id, paid.InstructionIndex - 1, source, paid.EffectiveSuit);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnSlashSuitAllowanceGrantedEvent(policy));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { BoundDiscardSlashBenefits = null });
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), "游戏已结束，弃牌后续收益取消。");
            return true;
        }

        return false;
    }

    internal bool HasTurnSlashSuitAllowance(int owner, Suit? suit) => _turnCardUseEffectsOperations._turnCardUseEffects.HasSlashSuitAllowance(_coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, owner, suit) || _characterXinXianYingMobileShiOperations.HasShiXxySuitAllowance(owner, suit);
    internal bool HasTurnAlcoholSuitAllowance(CharacterState actor, Card? material = null, CardConversionSource? conversion = null) => _characterSunJunMobileShiOperations.HasShiSunJunFirstSuitAllowance(actor, material, conversion) || (material is null ? new[]
    {
        Suit.Spade,
        Suit.Heart,
        Suit.Club,
        Suit.Diamond
    }.Any(suit => _characterXinXianYingMobileShiOperations.HasShiXxySuitAllowance(actor.Seat, suit)) : _characterXinXianYingMobileShiOperations.HasShiXxySuitAllowance(actor.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, material, conversion))));
    internal void ScheduleProgramFirstRoundRefund(ProgramSkillFrame frame)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        var activation = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Activation;
        if (frame.TriggerId is not null || activation?.UsesPerGame != 1 || frame.InstructionIndex != 1 || frame.OwnerSeat != _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(frame.OwnerSeat, frame.SkillId, activation.UsageGroup, SkillUsageScope.Game) != 1)
            throw new InvalidOperationException("A limited refund requires the first instruction of the exact consumed game activation.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreActualRoundProgressRoundNumberAccess._roundNumber != 1)
            return;
        var refund = _turnCardUseEffectsOperations._turnCardUseEffects.ScheduleGameUsageRefund(_coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, frame.Id, frame.InstructionIndex - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), activation.UsageGroup);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FirstRoundGameUsageRefundScheduledEvent(refund));
    }

    internal void ResolveFirstRoundGameUsageRefunds(int turn, int turnSeat)
    {
        foreach (var refund in _turnCardUseEffectsOperations._turnCardUseEffects.FirstRoundGameUsageRefunds.Where(r => r.TurnNumber == turn && r.TurnSeat == turnSeat))
        {
            var source = refund.Source;
            var refunded = _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[source.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], source.SkillId, source.SkillInstanceId) && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(source.OwnerSeat, source.SkillId, refund.UsageId, SkillUsageScope.Game) == 1 && _skillRuntimeStateOperations._skillRuntimeState.ClearUsage(source.OwnerSeat, source.SkillId, refund.UsageId, SkillUsageScope.Game);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FirstRoundGameUsageRefundResolvedEvent(refund, refunded));
        }
    }

    internal IReadOnlyList<TurnSlashSuitAllowance>? GetTurnSlashSuitAllowancesSnapshot(int owner)
    {
        var policies = _turnCardUseEffectsOperations._turnCardUseEffects.SlashSuitAllowances.Where(p => p.TurnNumber == _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber && p.TurnSeat == _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && p.Source.OwnerSeat == owner).ToArray();
        return policies.Length == 0 ? null : Array.AsReadOnly(policies);
    }

    internal IReadOnlyList<FirstRoundGameUsageRefund>? GetFirstRoundGameUsageRefundsSnapshot(int owner)
    {
        var refunds = _turnCardUseEffectsOperations._turnCardUseEffects.FirstRoundGameUsageRefunds.Where(p => p.TurnNumber == _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber && p.TurnSeat == _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && p.Source.OwnerSeat == owner).ToArray();
        return refunds.Length == 0 ? null : Array.AsReadOnly(refunds);
    }
}
