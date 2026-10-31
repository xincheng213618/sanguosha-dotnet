#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanFuOlRuntime
{
    internal IReadOnlyList<HanFuMaterial> ShuziBoardCards(int target) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != target).SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Concat(_corePhysicalOwnedCardQueriesOperations.GetJudgment(p))).Where(c => !c.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(_cardZonesOperations._cardZones.GetLocation(c.Id).OwnerSeat!.Value, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Transfer) && _coreProgramCardPaymentsOperations.CanMoveProgramCardToCorrespondingZone(c, _cardZonesOperations._cardZones.GetLocation(c.Id).Zone, target)).Select(c => HanFuMaterialFor(c.Id)).ToArray());
    internal void PayShuziBoard(ProgramSkillFrame f, HanFuMaterial m)
    {
        var r = f.HanFu!;
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId);
        var destination = _coreProgramCardPaymentsOperations.ProgramCorrespondingZoneLocation(card, m.From.Zone, r.TargetSeat);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        int? replacedId = null;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HanFu = r with { Stage = HanFuStage.BoardChildren, Board = m }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (destination.Zone == CardZoneKind.Equipment)
        {
            var old = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]).SingleOrDefault(c => _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot);
            replacedId = old?.Id;
            if (old is not null)
                _corePhysicalMovementFacadesOperations.MoveCard(old, destination, CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        }

        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([m.CardId], destination, new(ShuziBoardReason));
        var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(move => move.Sequence > before && move.CardId == m.CardId && move.Reason.Value == ShuziBoardReason);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HanFuMaterialPaidEvent(f.Id, HanFuStage.BoardChildren, 0, m, destination, movement.Sequence));
        if (destination.Zone == CardZoneKind.Equipment)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(f.Id, r.TargetSeat, _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot, card.Id, card.Kind, replacedId));
        RecordHanFuMovement(f.Id, HanFuStage.BoardChildren, ShuziBoardReason, before, 1);
    }
}
