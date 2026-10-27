namespace CardGame.Core;

public sealed partial class GameEngine
{
    // The closing step uses equipment rather than discarding it, so both its
    // selection and continuation must honour the actual equipment-use rules.
    private bool CanPayJushouHandCard(CharacterState owner, Card card) =>
        !IsSelfHandCategoryDiscardForbidden(owner.Seat, card, CardLocation.Hand(owner.Seat), OwnedCardMoveIntent.Discard) &&
        (!EquipmentRules.IsEquipment(card.Kind) || owner.IsAlive && owner.Hp > 0 &&
            owner.EquipmentSlotCapacity(EquipmentRules.Get(card.Kind).Slot) > 0 &&
            !IsCardUseForbidden(owner.Seat, card.Kind, CardActionType.Use) &&
            !IsSelfTargetForbiddenAction(owner, card.Kind, [owner.Seat]) &&
            !IsResponseEntityRestricted(owner.Seat, card.Id) &&
            !IsPlayPhasePhysicalCardRestricted(owner, card) && !IsTurnHandCardRestricted(owner, card) &&
            !IsTurnSuitUseForbidden(owner.Seat, EffectiveSuit(owner, card)));

    private SkillProgramStepOutcome BeginDiscardHandOrUseEquipment(ProgramSkillFrame frame)
    {
        var owner = _players[frame.OwnerSeat];
        var candidates = GetHand(owner)
            .Where(card => CanPayJushouHandCard(owner, card))
            .Select(card => card.Id).OrderBy(id => id).ToArray();
        if (candidates.Length == 0) return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(frame = frame with
        { StrategicDraft = new(SkillProgramEffectOp.DiscardHandOrUseEquipment, -1, null, 1) { Candidates = candidates } });
        PublishStrategicPrompt(frame);
        return _pendingDecision is null ? SkillProgramStepOutcome.Continue : SkillProgramStepOutcome.AwaitChoice;
    }

    private void ResolveDiscardHandOrUseEquipment(ProgramSkillFrame frame, int cardId)
    {
        var active = GetActiveProgramFrame(frame.Id);
        var location = _cardZones.GetLocation(cardId);
        if (location.Zone != CardZoneKind.Hand || location.OwnerSeat != frame.OwnerSeat ||
            !_cardZones.CardsAt(location).Any(item => item.Id == cardId))
            throw new InvalidOperationException("The Jushou cost card left the owner's hand.");
        var card = _cardZones.CardsAt(location).Single(item => item.Id == cardId);
        if (!CanPayJushouHandCard(_players[frame.OwnerSeat], card))
            throw new InvalidOperationException("The Jushou hand card can no longer pay its actual discard or equipment use.");
        if (EquipmentRules.IsEquipment(card.Kind))
        {
            var owner = _players[frame.OwnerSeat];
            var source = new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat,
                frame.SkillInstanceId);
            var useId = BeginCardUse(card, owner.Seat, [], conversionSource: source);
            MoveCard(card, location, CardLocation.Processing, CardMoveReasons.EquipmentUse);
            if (!TryBeginEquipmentTargetPrograms(owner, card, useId)) CompleteEquipmentUse(owner, card, useId);
            AdvanceRuntimeProgram(frame.Id);
            return;
        }
        ReplaceRuntimeTop(active with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        MoveLogicalDiscardCards(frame.OwnerSeat, frame.OwnerSeat, [card], location,
            new CardMoveReason($"skill-program.{frame.SkillId}.jushou-discard"));
        if (!TryBeginCardsMovedProgramWindow()) ReturnRuntimeProgramMovement(frame.Id);
    }
}
