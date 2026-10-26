namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool MobileOrdinaryQiangxiRange(int owner, int target, int? excludedWeapon, out int distance, out int range, out bool special)
    {
        distance = ConvertRuleValue(EvaluateDistance(_players[owner], _players[target], excludedWeapon));
        range = ConvertRuleValue(EvaluateAttackRange(_players[owner], excludedWeapon));
        special = HasShenTaishiCiPoweiRange(owner, target) || HasSayingRoundRange(owner, target) || IsGameFactionAttackRangeTarget(owner, target);
        return special || distance <= range;
    }

    private bool AllowsMobileOrdinaryQiangxiWeaponTarget(int owner, int weapon, int target)
    {
        if (!IsValidPlayerSeat(owner) || !IsValidPlayerSeat(target) || owner == target ||
            !_players[owner].IsAlive || !_players[target].IsAlive) return false;
        var from = _cardZones.GetLocation(weapon);
        if (from.OwnerSeat != owner || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) ||
            EquipmentRules.Get(GetAttackCard(weapon).Kind).Slot != EquipmentSlot.Weapon) return false;
        // A hand weapon is only a cost; the presently equipped weapon remains in the range query.
        return MobileOrdinaryQiangxiRange(owner, target, from.Zone == CardZoneKind.Equipment ? weapon : null, out _, out _, out _);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 82800)]
    private void CaptureMobileOrdinaryQiangxi(ProgramSkillStartedEvent started)
    {
        if (started.SkillId != MobileOrdinaryQiangxiComposition.Skill) return;
        var frame = GetActiveProgramFrame(started.FrameId);
        var weapon = frame.ActivationId == MobileOrdinaryQiangxiComposition.Weapon ? frame.SelectedCardIds.Single() : (int?)null;
        var from = weapon is { } id ? _cardZones.GetLocation(id) : (CardLocation?)null;
        if (frame.MobileOrdinaryQiangxi is not null || frame.InstructionIndex != 0 || frame.TriggerId is not null ||
            frame.OwnerSeat != started.OwnerSeat || frame.ActivationId != started.ActivationId ||
            frame.SelectedTargetSeats is not [var target] || target == frame.OwnerSeat || !_players[target].IsAlive ||
            _phase != TurnPhase.Play || _currentSeat != frame.OwnerSeat || _cardUseDebitPhaseInstanceId <= 0 ||
            weapon is { } cost && !AllowsMobileOrdinaryQiangxiWeaponTarget(frame.OwnerSeat, cost, target) ||
            !MobileOrdinaryQiangxiRange(frame.OwnerSeat, target, from?.Zone == CardZoneKind.Equipment ? weapon : null,
                out var distance, out var range, out var special))
            throw new InvalidOperationException("Ordinary mobile Qiangxi lost its original legal cost-target range pair.");
        ReplaceRuntimeFrame(frame.Id, frame with { MobileOrdinaryQiangxi = new(frame.Id,
            new(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash,
            _turnNumber, _cardUseDebitPhaseInstanceId, target, _players[frame.OwnerSeat].Hp, weapon, from, distance, range, special, _movementSequence) });
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 82801)]
    private void CaptureMobileOrdinaryQiangxiPayment(CardMovedEvent moved)
    {
        if (moved.Reason.Value != $"skill-program.{MobileOrdinaryQiangxiComposition.Skill}.DiscardSelected") return;
        var frame = _resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f =>
            f.SkillId == MobileOrdinaryQiangxiComposition.Skill && f.MobileOrdinaryQiangxi?.WeaponCardId == moved.CardId &&
            f.InstructionIndex == 1 && f.SelectedCardPayment is { InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected });
        if (frame?.MobileOrdinaryQiangxi is not { } receipt || moved.From != receipt.WeaponFrom ||
            moved.To != CardLocation.DiscardPile && moved.To != CardLocation.OutsideGame) return;
        var movement = _cardMovements.Single(m => m.Sequence > receipt.MovementBefore && m.CardId == moved.CardId &&
            m.From == receipt.WeaponFrom && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame) && m.Reason == moved.Reason);
        if (receipt.MovementAfter is not null) throw new InvalidOperationException("Ordinary mobile Qiangxi cannot commit its weapon material twice.");
        ReplaceRuntimeFrame(frame.Id, frame with { MobileOrdinaryQiangxi = receipt with { MovementAfter = movement.Sequence } });
    }

    private bool MobileOrdinaryQiangxiPaid(ProgramSkillFrame frame)
    {
        if (frame.SkillId != MobileOrdinaryQiangxiComposition.Skill || frame.TriggerId is not null ||
            frame.MobileOrdinaryQiangxi is not { } receipt || frame.InstructionIndex is not (1 or 2) ||
            receipt.FrameId != frame.Id || receipt.Source != new CardConversionSource(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId) ||
            receipt.GameplayHash != frame.GameplayHash || frame.SelectedTargetSeats is not [var target] || target != receipt.TargetSeat)
            return false;
        if (receipt.WeaponCardId is not { } weapon)
            return frame.ActivationId == MobileOrdinaryQiangxiComposition.Hp && frame.SelectedCardIds.Count == 0 &&
                ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == frame.Id && e.SkillId == frame.SkillId &&
                    e.TargetSeat == frame.OwnerSeat && e.Amount == 1 && e.RemainingHp == receipt.HpBefore - 1) == 1;
        if (frame.ActivationId != MobileOrdinaryQiangxiComposition.Weapon || !frame.SelectedCardIds.SequenceEqual([weapon]) ||
            frame.SelectedCardPayment is not { InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected, MovementCommitted: true } paid ||
            !paid.CardIds.SequenceEqual([weapon]) || paid.RecipientSeat != frame.OwnerSeat ||
            receipt.MovementAfter is not { } after || after <= receipt.MovementBefore || after > _movementSequence) return false;
        return _cardMovements.Count(m => m.Sequence > receipt.MovementBefore && m.Sequence <= after && m.CardId == weapon &&
            m.From == receipt.WeaponFrom && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame) &&
            m.Reason.Value == $"skill-program.{frame.SkillId}.DiscardSelected") == 1;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 82800)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 82800)]
    private bool ContinuePaidMobileOrdinaryQiangxi(ProgramSkillFrame frame) => MobileOrdinaryQiangxiPaid(frame);

    [ProgramContinuation(ProgramContinuationKind.Movement, 82800)]
    private bool ReturnPaidMobileOrdinaryQiangxi(ProgramSkillFrame frame)
    {
        if (_winner != Winner.None || frame.SelectedCardPaymentResult is not null ||
            frame.SelectedCardPayment is not { ActiveChildFrameId: null } payment || !MobileOrdinaryQiangxiPaid(frame)) return false;
        if (frame.InstructionIndex != 1 || frame.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Ordinary mobile Qiangxi lost its drained original weapon-payment return.");
        ReplaceRuntimeTop(frame with { SelectedCardPaymentResult = new(payment.InstructionIndex,
            payment.Operation, payment.LastCompletedChildFrameId, Completed: true) });
        AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 82800)]
    private void AssertMobileOrdinaryQiangxi(ProgramSkillFrame frame)
    {
        if (frame.MobileOrdinaryQiangxi is not { } receipt) return;
        if (frame.SkillId != MobileOrdinaryQiangxiComposition.Skill || frame.TriggerId is not null || receipt.FrameId != frame.Id ||
            receipt.Source != new CardConversionSource(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId) ||
            receipt.GameplayHash != frame.GameplayHash || receipt.TurnNumber < 1 || receipt.PhaseInstanceId <= 0 || receipt.HpBefore < 1 ||
            receipt.MovementBefore < 0 || receipt.MovementBefore > _movementSequence ||
            receipt.MovementAfter is { } after && (after <= receipt.MovementBefore || after > _movementSequence || receipt.WeaponCardId is null) ||
            frame.SelectedTargetSeats is not [var target] || receipt.TargetSeat != target || target == frame.OwnerSeat ||
            frame.InstructionIndex is < 0 or > 2 || !receipt.SpecialRange && receipt.DistanceWithoutCost > receipt.RangeWithoutCost ||
            (receipt.WeaponCardId is null) != (frame.ActivationId == MobileOrdinaryQiangxiComposition.Hp) ||
            receipt.WeaponCardId is { } weapon && (!frame.SelectedCardIds.SequenceEqual([weapon]) ||
                receipt.WeaponFrom is not { OwnerSeat: var owner, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } || owner != frame.OwnerSeat) ||
            ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == frame.Id && e.OwnerSeat == frame.OwnerSeat &&
                e.SkillId == frame.SkillId && e.ActivationId == frame.ActivationId) != 1 ||
            frame.InstructionIndex == 2 && !MobileOrdinaryQiangxiPaid(frame))
            throw new InvalidOperationException("Ordinary mobile Qiangxi lost its frozen cost, range, source or original paid damage tail.");
    }

    [ProgramCapability]
    private sealed class MobileOrdinaryQiangxiQueries(GameEngine engine) : IMobileOrdinaryQiangxiQueries
    {
        public bool AllowsWeaponTarget(int ownerSeat, int weaponId, int targetSeat) =>
            engine.AllowsMobileOrdinaryQiangxiWeaponTarget(ownerSeat, weaponId, targetSeat);
    }
}
