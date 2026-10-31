#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEquipmentPlacementAndHpPairRuntime
{
    internal bool CanPlaceOwnedEquipment(int ownerSeat, int targetSeat, Card card, CardLocation source) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive && source.OwnerSeat == ownerSeat && source.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _cardZonesOperations._cardZones.GetLocation(card.Id) == source && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && source != CardLocation.Equipment(targetSeat) && !_coreMatchParticipantsPlayersAccess._players[targetSeat].EquipmentAreaAbolished && _coreMatchParticipantsPlayersAccess._players[targetSeat].EquipmentSlotCapacity(_coreFieldEquipmentOperations.FieldEquipmentSlot(card)) > 0;
    internal bool CanPlaceActivationEquipment(int ownerSeat, int targetSeat, int cardId) => CanPlaceOwnedEquipment(ownerSeat, targetSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId), _cardZonesOperations._cardZones.GetLocation(cardId));
    internal SkillProgramStepOutcome PlaceProgramSelectedEquipment(ProgramSkillFrame frame, int targetSeat, string bind)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var source = _coreProgramLifecycleOperations.GetProgramCardSet(active, bind);
        if (source.CardIds.Count != 1 || !source.CardIds.SequenceEqual(active.SelectedCardIds))
            throw new InvalidOperationException("Equipment placement requires its one captured activation cost.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(source.CardIds[0]);
        var from = source.SourceLocations.Single();
        if (!CanPlaceOwnedEquipment(frame.OwnerSeat, targetSeat, card, from) || active.SelectedCardPayment is not null || active.SelectedCardPaymentResult is not null)
            throw new InvalidOperationException("The captured equipment placement cost is no longer legal.");
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { SelectedCardPayment = new(active.InstructionIndex, SkillProgramEffectOp.PlaceSelectedEquipment, Array.AsReadOnly(source.CardIds.ToArray()), targetSeat) { PlacementSourceLocation = from } });
        var slot = _coreFieldEquipmentOperations.FieldEquipmentSlot(card);
        var equipped = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[targetSeat]).Where(item => _coreFieldEquipmentOperations.FieldEquipmentSlot(item) == slot).ToArray();
        Card? replaced = equipped.Length >= _coreMatchParticipantsPlayersAccess._players[targetSeat].EquipmentSlotCapacity(slot) ? equipped[0] : null;
        if (replaced is not null)
        {
            _coreAdvancedWeaponsOperations.CopyFirstReplacedWeapon(card, replaced);
            _corePhysicalMovementFacadesOperations.MoveCard(replaced, CardLocation.Equipment(targetSeat), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        }

        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Equipment(targetSeat), CardMoveReasons.EquipmentEnter);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(frame.Id, targetSeat, slot, card.Id, card.Kind, replaced?.Id));
        _coreMatchLogOperations.AddLog("EquipmentChanged", $"{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 将【{(card.DisplayName)}】置入 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 的装备区。", targetSeat);
        active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { SelectedCardPayment = active.SelectedCardPayment!with { MovementCommitted = true } });
        if (!_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) && !_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void FreezeProgramSelectedHpPair(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.HpPairSnapshot is not null || active.SelectedTargetSeats is not [var selected] || active.PendingMovementContinuation is not null || active.SelectedCardPayment is not null && active.SelectedCardPaymentResult is not { Completed: true })
            throw new InvalidOperationException("An HP pair must freeze once after its payment children return.");
        var ownerHp = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Hp;
        var targetHp = _coreMatchParticipantsPlayersAccess._players[selected].Hp;
        var awards = active.OwnerSeat != selected && _coreMatchParticipantsPlayersAccess._players[selected].IsAlive && ownerHp != targetHp;
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { HpPairSnapshot = new(active.InstructionIndex, active.OwnerSeat, selected, ownerHp, targetHp, _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[selected].IsAlive, awards ? (ownerHp > targetHp ? active.OwnerSeat : selected) : null, awards ? (ownerHp < targetHp ? active.OwnerSeat : selected) : null) });
    }
}
