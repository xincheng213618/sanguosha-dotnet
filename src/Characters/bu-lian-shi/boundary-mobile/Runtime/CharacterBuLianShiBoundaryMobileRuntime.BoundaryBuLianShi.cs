#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBuLianShiBoundaryMobileRuntime
{
    void IBoundaryBuLianShiHost.Prepare(ProgramSkillFrame f) => PrepareBoundaryBuLianShiAnxu(f);
    void IBoundaryBuLianShiHost.SelectLeast(ProgramSkillFrame f) => SelectBoundaryBuLianShiAnxuLeast(f);
    bool IBoundaryBuLianShiQueries.HasHe(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat).Count > 0;
    IReadOnlyList<int> IBoundaryBuLianShiQueries.Least(int owner) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { OwnerSeat: var seat, BoundaryBuLianShiAnxu: { LeastSelected: true, LeastSeat: { } least } } f && seat == owner && ValidBoundaryBuLianShiAnxu(f) && _coreMatchParticipantsPlayersAccess._players[least].IsAlive ? [least] : [];
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 55900)]
    internal void CaptureBoundaryBuLianShiAnxuObtain(CardMovedEvent fact)
    {
        if (fact.Reason.Value != "skill-program." + BoundaryBuLianShiComposition.Anxu + ".SelectAndMoveOwnedCard" || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SkillId: BoundaryBuLianShiComposition.Anxu, InstructionIndex: 1, SelectedTargetSeats: [var recipient, var donor] } f || recipient == donor || recipient == f.OwnerSeat || donor == f.OwnerSeat || _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()is not { } m || m.CardId != fact.CardId || m.From != fact.From || m.To != fact.To || m.Reason != fact.Reason)
            return;
        if (f.BoundaryBuLianShiAnxu is null && m.From.OwnerSeat == donor && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && (m.To == CardLocation.Processing || m.To == CardLocation.Hand(recipient) || m.To == CardLocation.OutsideGame))
        {
            var r = new BoundaryBuLianShiAnxuReceipt(recipient, donor, m.CardId, m.From, m.Sequence, false);
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { BoundaryBuLianShiAnxu = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryBuLianShiAnxuPreparedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, recipient, donor, m.From.Zone, m.Sequence));
        }

        if (f.BoundaryBuLianShiAnxu is { Delivered: false } captured && captured.CardId == m.CardId && m.To == CardLocation.Hand(recipient) && (m.From == captured.From && m.Sequence == captured.MovementSequence || m.From == CardLocation.Processing && m.Sequence > captured.MovementSequence))
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { BoundaryBuLianShiAnxu = captured with { Delivered = true, DeliverySequence = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryBuLianShiAnxuDeliveredEvent(f.Id, captured.MovementSequence, m.Sequence));
        }
    }

    internal void PrepareBoundaryBuLianShiAnxu(ProgramSkillFrame f)
    {
        if (f.SkillId != BoundaryBuLianShiComposition.Anxu || f.InstructionIndex != 2 || f.SelectedTargetSeats is not [var recipient, var donor] || recipient == donor || recipient == f.OwnerSeat || donor == f.OwnerSeat || _coreProgramLifecycleOperations.GetProgramCardSet(f, "anxu-obtained")is not { CardIds: [var id], SourceLocations: [var from] } || f.BoundaryBuLianShiAnxu is { } prior && prior.CardId != id)
            throw new InvalidOperationException("Anxu requires its exact original recipient/donor and obtained card result.");
        if (f.BoundaryBuLianShiAnxu is null)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { BoundaryBuLianShiAnxu = new(recipient, donor, id, from, 0, false) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryBuLianShiAnxuPreparedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, recipient, donor, from.Zone, 0));
        }

        if (!ValidBoundaryBuLianShiAnxu(f))
            throw new InvalidOperationException("Anxu lost its actual original-source and delivery invoices.");
        var r = f.BoundaryBuLianShiAnxu!;
        // Count the committed original hand-source Obtain, even if a native
        // movement child has already moved that card away from the recipient.
        _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, "anxu-hand-reward", r.Delivered && r.From.Zone == CardZoneKind.Hand ? [id] : [], SkillProgramCardSetVisibility.Private, r.Delivered && r.From.Zone == CardZoneKind.Hand ? [r.From] : []);
    }

    internal bool ValidBoundaryBuLianShiAnxu(ProgramSkillFrame f) => f.SkillId == BoundaryBuLianShiComposition.Anxu && f.BoundaryBuLianShiAnxu is { } r && r.RecipientSeat != r.DonorSeat && r.RecipientSeat != f.OwnerSeat && r.DonorSeat != f.OwnerSeat && r.From.OwnerSeat == r.DonorSeat && r.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryBuLianShiAnxuPreparedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillInstanceId == f.SkillInstanceId && e.GameplayHash == f.GameplayHash && e.RecipientSeat == r.RecipientSeat && e.DonorSeat == r.DonorSeat && e.SourceZone == r.From.Zone && e.MovementSequence == r.MovementSequence) == 1 && (r.MovementSequence == 0 ? !r.Delivered && r.DeliverySequence == 0 : _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence == r.MovementSequence && m.CardId == r.CardId && m.From == r.From && m.Reason.Value == "skill-program." + f.SkillId + ".SelectAndMoveOwnedCard" && (m.To == CardLocation.Processing || m.To == CardLocation.Hand(r.RecipientSeat) || m.To == CardLocation.OutsideGame)) == 1) && (r.Delivered ? r.DeliverySequence >= r.MovementSequence && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryBuLianShiAnxuDeliveredEvent>().Count(e => e.FrameId == f.Id && e.SourceSequence == r.MovementSequence && e.DeliverySequence == r.DeliverySequence) == 1 && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence == r.DeliverySequence && m.CardId == r.CardId && (m.From == CardLocation.Processing || m.From == r.From) && m.To == CardLocation.Hand(r.RecipientSeat) && m.Reason.Value == "skill-program." + f.SkillId + ".SelectAndMoveOwnedCard") == 1 : r.DeliverySequence == 0);
    internal void SelectBoundaryBuLianShiAnxuLeast(ProgramSkillFrame f)
    {
        if (f.InstructionIndex != 4 || !ValidBoundaryBuLianShiAnxu(f) || f.BoundaryBuLianShiAnxu is not { LeastSelected: false } r || f.SelectedTargetSeats is not [var a, var b] || a != r.RecipientSeat || b != r.DonorSeat)
            throw new InvalidOperationException("Anxu must compare its original pair only after its original Obtain and owner reward children return.");
        var first = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[a]).Count;
        var second = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[b]).Count;
        int? least = r.Delivered && _coreMatchParticipantsPlayersAccess._players[a].IsAlive && _coreMatchParticipantsPlayersAccess._players[b].IsAlive && first != second ? first < second ? a : b : null;
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { BoundaryBuLianShiAnxu = r with { LeastSelected = true, LeastSeat = least }, SelectedTargetSeats = least is { } seat ? [seat] : [] });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryBuLianShiAnxuLeastEvent(f.Id, a, b, first, second, least));
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 55900)]
    internal bool KeepIssuedBoundaryBuLianShiAnxu(ProgramSkillFrame f) => f.BoundaryBuLianShiAnxu is { MovementSequence: > 0 } && ValidBoundaryBuLianShiAnxu(f);
}
