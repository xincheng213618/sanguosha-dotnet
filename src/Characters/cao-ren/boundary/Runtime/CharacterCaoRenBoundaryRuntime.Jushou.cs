#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoRenBoundaryRuntime
{
    internal bool CanPayJushouHandCard(CharacterState owner, Card card) => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner.Seat, card, CardLocation.Hand(owner.Seat), OwnedCardMoveIntent.Discard) && (!_coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) || owner.IsAlive && owner.Hp > 0 && owner.EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot) > 0 && !_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, card.Kind, CardActionType.Use) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(owner, card.Kind, [owner.Seat]) && !_coreResponseEntityExchangeOperations.IsResponseEntityRestricted(owner.Seat, card.Id) && !_coreProgramPhaseColorRestrictionsOperations.IsPlayPhasePhysicalCardRestricted(owner, card) && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, card) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnSuitUseForbidden(owner.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card)));
    internal SkillProgramStepOutcome BeginDiscardHandOrUseEquipment(ProgramSkillFrame frame)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var candidates = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(card => CanPayJushouHandCard(owner, card)).Select(card => card.Id).OrderBy(id => id).ToArray();
        if (candidates.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { StrategicDraft = new(SkillProgramEffectOp.DiscardHandOrUseEquipment, -1, null, 1) { Candidates = candidates } });
        _coreStrategicProgramsOperations.PublishStrategicPrompt(frame);
        return _corePendingDecisionPolicyPipelineOperations._pendingDecision is null ? SkillProgramStepOutcome.Continue : SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ResolveDiscardHandOrUseEquipment(ProgramSkillFrame frame, int cardId)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var location = _cardZonesOperations._cardZones.GetLocation(cardId);
        if (location.Zone != CardZoneKind.Hand || location.OwnerSeat != frame.OwnerSeat || !_cardZonesOperations._cardZones.CardsAt(location).Any(item => item.Id == cardId))
            throw new InvalidOperationException("The Jushou cost card left the owner's hand.");
        var card = _cardZonesOperations._cardZones.CardsAt(location).Single(item => item.Id == cardId);
        if (!CanPayJushouHandCard(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card))
            throw new InvalidOperationException("The Jushou hand card can no longer pay its actual discard or equipment use.");
        if (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind))
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
            var source = new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
            var useId = _coreNativeCardUseOperations.BeginCardUse(card, owner.Seat, [], conversionSource: source);
            _corePhysicalMovementFacadesOperations.MoveCard(card, location, CardLocation.Processing, CardMoveReasons.EquipmentUse);
            if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(owner, card, useId))
                _coreNativeEquipmentUseOperations.CompleteEquipmentUse(owner, card, useId);
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(active with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(frame.OwnerSeat, frame.OwnerSeat, [card], location, new CardMoveReason($"skill-program.{(frame.SkillId)}.jushou-discard"));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }
}
