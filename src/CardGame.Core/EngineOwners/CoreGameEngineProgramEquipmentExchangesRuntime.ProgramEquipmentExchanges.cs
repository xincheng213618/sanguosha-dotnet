#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramEquipmentExchangesRuntime
{
    internal bool IsProgramEquipmentExchangePair(int ownerSeat, int firstSeat, int secondSeat)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(firstSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(secondSeat) || firstSeat == secondSeat || !_coreMatchParticipantsPlayersAccess._players[firstSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[secondSeat].IsAlive)
            return false;
        var firstCount = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[firstSeat]).Count;
        var secondCount = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[secondSeat]).Count;
        return firstCount + secondCount > 0 && Math.Abs(firstCount - secondCount) <= Math.Max(0, _coreMatchParticipantsPlayersAccess._players[ownerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[ownerSeat].Hp);
    }

    internal SkillProgramStepOutcome ExchangeProgramSelectedTargetEquipment(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.EquipmentPairPayment is not null)
        {
            if (!_coreEquipmentPairPaymentsOperations.CanResumePaidEquipmentPair(active))
            {
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "足额付款已完成，但原来源或交换对象已失效；保留真实成本。");
                return SkillProgramStepOutcome.AwaitChild;
            }

            active = _coreEquipmentPairPaymentsOperations.FreezePaidEquipmentExchangeStart(active);
            if (active.SelectedTargetSeats.All(seat => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Count == 0))
                return SkillProgramStepOutcome.Continue;
        }
        else
        {
            var legalPair = active.SelectedTargetSeats.Count == 2 && (active.SkillId == "boundary:ganlu-mobile" ? _characterWuGuoTaiBoundaryMobileOperations.IsBoundaryWuGuoTaiGanluPair(active.OwnerSeat, active.SelectedTargetSeats[0], active.SelectedTargetSeats[1]) : IsProgramEquipmentExchangePair(active.OwnerSeat, active.SelectedTargetSeats[0], active.SelectedTargetSeats[1]));
            if (!legalPair)
                throw new InvalidOperationException("Equipment exchange requires a current legal equipment pair.");
            if (active.SkillId == "boundary:ganlu-mobile" && active.SelectedTargetSeats.All(seat => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Count == 0))
                return SkillProgramStepOutcome.Continue;
        }

        var first = active.SelectedTargetSeats[0];
        var second = active.SelectedTargetSeats[1];
        var incoming = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[first]).OrderBy(card => card.Id).Select(card => (Card: card, Source: first, Recipient: second)).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[second]).OrderBy(card => card.Id).Select(card => (Card: card, Source: second, Recipient: first))).ToArray();
        var occupied = new Dictionary<(int Seat, EquipmentSlot Slot), int>();
        var equipmentMoves = incoming.Select(item =>
        {
            var slot = _coreFieldEquipmentOperations.FieldEquipmentSlot(item.Card);
            var key = (item.Recipient, slot);
            var count = occupied.GetValueOrDefault(key);
            var destination = item.Card.IsGeneralWeapon || CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(item.Card, CardLocation.Equipment(item.Source), CardLocation.Equipment(item.Recipient)) ? CardLocation.OutsideGame : count < _coreMatchParticipantsPlayersAccess._players[item.Recipient].EquipmentSlotCapacity(slot) ? CardLocation.Equipment(item.Recipient) : CardLocation.DiscardPile;
            destination = _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(item.Card, CardLocation.Equipment(item.Source), destination);
            if (destination.Zone == CardZoneKind.Equipment)
                occupied[key] = count + 1;
            return (item.Card, From: CardLocation.Equipment(item.Source), To: destination);
        }).ToArray();
        // Freeze both Ox stores before either transfers, so swapping two treasures
        // never merges one owner's grains into the other owner's outgoing store.
        var moves = equipmentMoves.Concat(equipmentMoves.Where(move => _coreNativeDamageHandlesOperations.Value.UsesFormalWoodenOx && move.Card.Kind == CardKind.WoodenOx).SelectMany(move => _cardZonesOperations._cardZones.CardsAt(CardLocation.WoodenOxGrain(move.From.OwnerSeat!.Value)).Select(card => (Card: card, From: CardLocation.WoodenOxGrain(move.From.OwnerSeat.Value), To: move.To.Zone == CardZoneKind.Equipment ? CardLocation.WoodenOxGrain(move.To.OwnerSeat!.Value) : CardLocation.DiscardPile)))).ToArray();
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.equipment-exchange");
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { PendingMovementContinuation = new ProgramMovementContinuation(frame.OwnerSeat, 0, null) });
        var batch = _coreCardMovementProgramsOperations.BeginCardMovementBatch(moves.Select(move => move.From), moves.Select(move => move.To));
        var records = new List<CardMovementRecord>(moves.Length);
        var committed = false;
        try
        {
            // Both equipment areas vacate before entering either recipient area. Processing
            // is an internal staging zone, never a separate observable movement or trigger.
            foreach (var move in moves)
                _cardZonesOperations._cardZones.Move(move.Card.Id, move.From, CardLocation.Processing);
            foreach (var move in moves)
                _cardZonesOperations._cardZones.Move(move.Card.Id, CardLocation.Processing, move.To);
            foreach (var move in moves)
            {
                records.Add(_corePhysicalMovementFacadesOperations.RecordMovement(move.Card, move.From, move.To, reason));
                _coreEquipmentSkillsOperations.ResolveEquipmentSkillGrant(move.Card, move.From, move.To);
                _corePhysicalMovementFacadesOperations.ResolveSilverLionRemoval(move.Card, move.From, reason);
            }

            committed = true;
        }
        finally
        {
            _coreCardMovementProgramsOperations.CompleteCardMovementBatch(batch, records, committed);
        }

        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
