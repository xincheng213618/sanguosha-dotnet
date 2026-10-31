#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiangChongOlRuntime
{
    internal bool IsXiangChongGuyingEquipmentRide(ResolutionFrame ride, ResolutionFrame beneath)
    {
        if (ride is not CardUseFrame { XiangChongGuyingEquipment: { } origin } use || beneath is not ProgramSkillFrame parent || !MatchesXiangChongEquipment(parent, origin))
            return false;
        ValidateXiangChongGuyingEquipment(use);
        return true;
    }

    internal bool CanIssueXiangChongGuyingEquipment(ProgramSkillFrame frame, int cardId, CardLocation from)
    {
        if (frame.XiangChong is not { Lost: { } lost, LossDestination: { } destination } || lost.CardId != cardId || destination != from || !ValidXiang(frame) || !XiangLostStillAvailable(frame) || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || from == CardLocation.OutsideGame || _cardZonesOperations._cardZones.GetLocation(cardId) != from || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(lost.Kind))
            return false;
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        return owner.EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(lost.Kind).Slot) > 0 && !_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, lost.Kind, CardActionType.Use) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(owner.Seat, [cardId]);
    }

    internal void BeginXiangChongGuyingEquipment(ProgramSkillFrame frame, XiangChongEquipmentOrigin origin)
    {
        if (!MatchesXiangChongEquipment(frame, origin) || !CanIssueXiangChongGuyingEquipment(frame, origin.CardId, origin.From) || origin.CardUseFrameId != _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1)
            throw new InvalidOperationException("Guying equipment requires its exact still-available original entity and native issuer.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(origin.CardId);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, origin.ActorSeat, [], physicalCardIds: [card.Id]);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { XiangChongGuyingEquipment = origin });
        RecordXiangChongEquipmentMaterialClaim(frame, origin);
        if (origin.From != CardLocation.Processing)
            _corePhysicalMovementFacadesOperations.MoveCard(card, origin.From, CardLocation.Processing, CardMoveReasons.EquipmentUse);
        if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(_coreMatchParticipantsPlayersAccess._players[origin.ActorSeat], card, id))
            _coreNativeEquipmentUseOperations.CompleteEquipmentUse(_coreMatchParticipantsPlayersAccess._players[origin.ActorSeat], card, id);
    }

    [NativeUseValidation(27140)]
    internal void ValidateXiangChongGuyingEquipment(CardUseFrame use)
    {
        if (use.XiangChongGuyingEquipment is not { } origin)
            return;
        if (use.Id != origin.CardUseFrameId || use.CardId != origin.CardId || use.CardKind != origin.CardKind || use.Action is not { Type: CardActionType.Use, PhysicalCards: [var cost] } action || action.EffectiveKind != origin.CardKind || cost.CardId != origin.CardId || cost.CardKind != origin.CardKind || cost.From != origin.From || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, origin.ActorSeat, origin.ActorSeat) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == origin.ProgramFrameId)is not { } parent || !MatchesXiangChongEquipment(parent, origin))
            throw new InvalidOperationException("Guying equipment lost its original material, actor or owning return.");
    }
}
