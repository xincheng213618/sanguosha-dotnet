#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoCaoMobileGodRuntime
{
    internal bool ValidMobileGuixin(ProgramSkillFrame f)
    {
        if (f.MobileGuixin is not { } r || f.SkillId != "mobile:guixin" || f.InstructionIndex != 1 || r.Source != MobileGuixinSource(f) || r.GameplayHash != f.GameplayHash || !Enum.IsDefined(r.Stage) || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out _) || damage.TargetSeat != f.OwnerSeat || window.Id != r.DamageWindowId || damage.Id != r.DamageFrameId || damage.Amount != r.DamageAmount || f.WindowContext!.OccurrenceIndex != r.OccurrenceIndex || r.OccurrenceIndex < 0 || r.OccurrenceIndex >= damage.Amount || r.OtherSeats.Count != _coreMatchParticipantsPlayersAccess._players.Count - 1 || r.OtherSeats.Distinct().Count() != r.OtherSeats.Count || r.OtherSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || seat == f.OwnerSeat) || r.Cursor < 0 || r.Cursor > r.OtherSeats.Count || r.Payments.Select(p => p.Cursor).Distinct().Count() != r.Payments.Count || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileGuixinStartedEvent>().Count(e => e == new MobileGuixinStartedEvent(f.Id, r.Source, r.GameplayHash, window.Id, damage.Id, r.OccurrenceIndex, damage.Amount, string.Join(",", r.OtherSeats))) != 1)
            return false;
        var prior = -1L;
        for (var cursor = 0; cursor <= r.Cursor && cursor < r.OtherSeats.Count; cursor++)
        {
            var payments = r.Payments.Where(p => p.Cursor == cursor).ToArray();
            if (payments.Length == 0)
            {
                if (cursor < r.Cursor && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileGuixinSeatSkippedEvent>().Count(e => e == new MobileGuixinSeatSkippedEvent(f.Id, cursor, r.OtherSeats[cursor])) != 1)
                    return false;
                continue;
            }

            if (payments is not [var p] || p.TargetSeat != r.OtherSeats[cursor] || p.From.OwnerSeat != p.TargetSeat || !CharacterCaoCaoMobileGodDefinitions.MobileGuixinZones.Contains(p.From.Zone) || p.CardId <= 0 || p.BatchId <= 0 || p.Before < prior || p.Before < 0 || p.After <= p.Before || p.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || p.Movements is not [var movement] || movement.CardId != p.CardId || movement.From != p.From || movement.Sequence <= p.Before || !MobileGuixinPaymentTail(f, p, movement) || movement.Reason.Value != MobileGuixinReason || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m == movement) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileGuixinMovementPaidEvent>().Count(e => e == new MobileGuixinMovementPaidEvent(f.Id, cursor, p.TargetSeat, p.BatchId, 1, p.Before, p.After)) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileGuixinSeatSkippedEvent>().Any(e => e.FrameId == f.Id && e.Cursor == cursor))
                return false;
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(p.CardId);
            var expected = card.IsGeneralWeapon && p.From.Zone == CardZoneKind.Equipment || CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, p.From, CardLocation.Hand(f.OwnerSeat)) ? CardLocation.OutsideGame : CardLocation.Hand(f.OwnerSeat);
            if (movement.To != _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, p.From, expected))
                return false;
            prior = p.After;
        }

        if (r.Payments.Any(p => p.Cursor < 0 || p.Cursor >= r.OtherSeats.Count || p.Cursor > r.Cursor) || r.Stage == MobileGuixinStage.MovementChildren && (r.Cursor >= r.OtherSeats.Count || r.Payments.All(p => p.Cursor != r.Cursor)) || r.Stage == MobileGuixinStage.Choosing && r.Payments.Any(p => p.Cursor == r.Cursor))
            return false;
        if (r.TurnoverIssued)
            return r.Cursor == r.OtherSeats.Count && r.Stage is MobileGuixinStage.TurnoverChildren or MobileGuixinStage.Finished && r.FaceDownBefore.HasValue && r.FaceDownAfter.HasValue && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileGuixinTurnoverIssuedEvent>().Count(e => e == new MobileGuixinTurnoverIssuedEvent(f.Id, r.FaceDownBefore.Value, r.FaceDownAfter.Value)) == 1;
        return r.FaceDownBefore is null && r.FaceDownAfter is null && r.Stage != MobileGuixinStage.TurnoverChildren;
    }

    internal bool MobileGuixinPaymentTail(ProgramSkillFrame frame, MobileGuixinPayment payment, CardMovementRecord gain)
    {
        if (gain.Sequence == payment.After)
            return true;
        // A phantom really reaches the recipient's hand, then its original
        // issuer destroys it synchronously after that exact gain event. The
        // frozen payment ends after both records; no extra acquisition occurred.
        return gain.To == CardLocation.Hand(frame.OwnerSeat) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > gain.Sequence && move.Sequence <= payment.After).ToArray()is [var destroyed] && destroyed.Sequence == payment.After && destroyed.CardId == payment.CardId && destroyed.From == gain.To && destroyed.HappyZuoCiPhantomDestination is { OriginalForeignGainSequence: { } original, OriginalRecipient: { } recipient, Issuer: var issuer } && original == gain.Sequence && recipient == frame.OwnerSeat && issuer == payment.TargetSeat && _characterZuoCiHappyGodOperations.IsHappyZuoCiDestroyedMaterial(destroyed);
    }
}
