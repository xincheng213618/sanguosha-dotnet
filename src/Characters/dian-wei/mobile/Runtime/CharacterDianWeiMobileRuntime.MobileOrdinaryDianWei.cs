#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDianWeiMobileRuntime
{
    internal bool MobileOrdinaryQiangxiRange(int owner, int target, int? excludedWeapon, out int distance, out int range, out bool special)
    {
        distance = CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateDistance(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target], excludedWeapon));
        range = CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreActionQueriesOperations.EvaluateAttackRange(_coreMatchParticipantsPlayersAccess._players[owner], excludedWeapon));
        special = _characterTaishiCiMobileGodOperations.HasShenTaishiCiPoweiRange(owner, target) || _characterSunYuanOlOperations.HasSayingRoundRange(owner, target) || _coreProgramPersistentCombatPoliciesOperations.IsGameFactionAttackRangeTarget(owner, target);
        return special || distance <= range;
    }

    internal bool AllowsMobileOrdinaryQiangxiWeaponTarget(int owner, int weapon, int target)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || owner == target || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            return false;
        var from = _cardZonesOperations._cardZones.GetLocation(weapon);
        if (from.OwnerSeat != owner || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _coreEquipmentContentOperations.EquipmentRules.Get(_coreCardAttackStateOperations.GetAttackCard(weapon).Kind).Slot != EquipmentSlot.Weapon)
            return false;
        // A hand weapon is only a cost; the presently equipped weapon remains in the range query.
        return MobileOrdinaryQiangxiRange(owner, target, from.Zone == CardZoneKind.Equipment ? weapon : null, out _, out _, out _);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 82800)]
    internal void CaptureMobileOrdinaryQiangxi(ProgramSkillStartedEvent started)
    {
        if (started.SkillId != MobileOrdinaryQiangxiComposition.Skill)
            return;
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(started.FrameId);
        var weapon = frame.ActivationId == MobileOrdinaryQiangxiComposition.Weapon ? frame.SelectedCardIds.Single() : (int? )null;
        var from = weapon is { } id ? _cardZonesOperations._cardZones.GetLocation(id) : (CardLocation? )null;
        if (frame.MobileOrdinaryQiangxi is not null || frame.InstructionIndex != 0 || frame.TriggerId is not null || frame.OwnerSeat != started.OwnerSeat || frame.ActivationId != started.ActivationId || frame.SelectedTargetSeats is not [var target] || target == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != frame.OwnerSeat || _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId <= 0 || weapon is { } cost && !AllowsMobileOrdinaryQiangxiWeaponTarget(frame.OwnerSeat, cost, target) || !MobileOrdinaryQiangxiRange(frame.OwnerSeat, target, from?.Zone == CardZoneKind.Equipment ? weapon : null, out var distance, out var range, out var special))
            throw new InvalidOperationException("Ordinary mobile Qiangxi lost its original legal cost-target range pair.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { MobileOrdinaryQiangxi = new(frame.Id, new(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, target, _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Hp, weapon, from, distance, range, special, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) });
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 82801)]
    internal void CaptureMobileOrdinaryQiangxiPayment(CardMovedEvent moved)
    {
        if (moved.Reason.Value != $"skill-program.{(MobileOrdinaryQiangxiComposition.Skill)}.DiscardSelected")
            return;
        var frame = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == MobileOrdinaryQiangxiComposition.Skill && f.MobileOrdinaryQiangxi?.WeaponCardId == moved.CardId && f.InstructionIndex == 1 && f.SelectedCardPayment is { InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected });
        if (frame?.MobileOrdinaryQiangxi is not { } receipt || moved.From != receipt.WeaponFrom || moved.To != CardLocation.DiscardPile && moved.To != CardLocation.OutsideGame)
            return;
        var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > receipt.MovementBefore && m.CardId == moved.CardId && m.From == receipt.WeaponFrom && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame) && m.Reason == moved.Reason);
        if (receipt.MovementAfter is not null)
            throw new InvalidOperationException("Ordinary mobile Qiangxi cannot commit its weapon material twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { MobileOrdinaryQiangxi = receipt with { MovementAfter = movement.Sequence } });
    }

    internal bool MobileOrdinaryQiangxiPaid(ProgramSkillFrame frame)
    {
        if (frame.SkillId != MobileOrdinaryQiangxiComposition.Skill || frame.TriggerId is not null || frame.MobileOrdinaryQiangxi is not { } receipt || frame.InstructionIndex is not (1 or 2) || receipt.FrameId != frame.Id || receipt.Source != new CardConversionSource(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId) || receipt.GameplayHash != frame.GameplayHash || frame.SelectedTargetSeats is not [var target] || target != receipt.TargetSeat)
            return false;
        if (receipt.WeaponCardId is not { } weapon)
            return frame.ActivationId == MobileOrdinaryQiangxiComposition.Hp && frame.SelectedCardIds.Count == 0 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == frame.Id && e.SkillId == frame.SkillId && e.TargetSeat == frame.OwnerSeat && e.Amount == 1 && e.RemainingHp == receipt.HpBefore - 1) == 1;
        if (frame.ActivationId != MobileOrdinaryQiangxiComposition.Weapon || !frame.SelectedCardIds.SequenceEqual([weapon]) || frame.SelectedCardPayment is not { InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected, MovementCommitted: true } paid || !paid.CardIds.SequenceEqual([weapon]) || paid.RecipientSeat != frame.OwnerSeat || receipt.MovementAfter is not { } after || after <= receipt.MovementBefore || after > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            return false;
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > receipt.MovementBefore && m.Sequence <= after && m.CardId == weapon && m.From == receipt.WeaponFrom && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame) && m.Reason.Value == $"skill-program.{(frame.SkillId)}.DiscardSelected") == 1;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 82800)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 82800)]
    internal bool ContinuePaidMobileOrdinaryQiangxi(ProgramSkillFrame frame) => MobileOrdinaryQiangxiPaid(frame);
    [ProgramContinuation(ProgramContinuationKind.Movement, 82800)]
    internal bool ReturnPaidMobileOrdinaryQiangxi(ProgramSkillFrame frame)
    {
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || frame.SelectedCardPaymentResult is not null || frame.SelectedCardPayment is not { ActiveChildFrameId: null } payment || !MobileOrdinaryQiangxiPaid(frame))
            return false;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedCardPaymentResult = new(payment.InstructionIndex, payment.Operation, payment.LastCompletedChildFrameId, Completed: true) });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    bool IMobileOrdinaryQiangxiQueries.AllowsWeaponTarget(int ownerSeat, int weaponId, int targetSeat) => AllowsMobileOrdinaryQiangxiWeaponTarget(ownerSeat, weaponId, targetSeat);
}
