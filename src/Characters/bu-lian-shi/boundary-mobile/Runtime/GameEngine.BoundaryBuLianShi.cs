namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability] private sealed class BoundaryBuLianShiHost(GameEngine engine) : IBoundaryBuLianShiHost
    { public void Prepare(ProgramSkillFrame f) => engine.PrepareBoundaryBuLianShiAnxu(f); public void SelectLeast(ProgramSkillFrame f) => engine.SelectBoundaryBuLianShiAnxuLeast(f); }
    [ProgramCapability] private sealed class BoundaryBuLianShiQueries(GameEngine engine) : IBoundaryBuLianShiQueries
    {
        public bool HasHe(int seat) => engine.IsValidPlayerSeat(seat) && engine._players[seat].IsAlive && engine.GetHand(engine._players[seat]).Count + engine.GetEquipment(seat).Count > 0;
        public IReadOnlyList<int> Least(int owner) => engine._resolutionStack.LastOrDefault() is ProgramSkillFrame { OwnerSeat: var seat, BoundaryBuLianShiAnxu: { LeastSelected: true, LeastSeat: { } least } } f && seat == owner &&
            engine.ValidBoundaryBuLianShiAnxu(f) && engine._players[least].IsAlive ? [least] : [];
    }
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 55900)]
    private void CaptureBoundaryBuLianShiAnxuObtain(CardMovedEvent fact)
    {
        if (fact.Reason.Value != "skill-program." + BoundaryBuLianShiComposition.Anxu + ".SelectAndMoveOwnedCard" ||
            _resolutionStack.LastOrDefault() is not ProgramSkillFrame { SkillId: BoundaryBuLianShiComposition.Anxu, InstructionIndex: 1, SelectedTargetSeats: [var recipient, var donor] } f ||
            recipient == donor || recipient == f.OwnerSeat || donor == f.OwnerSeat || _cardMovements.LastOrDefault() is not { } m || m.CardId != fact.CardId || m.From != fact.From || m.To != fact.To || m.Reason != fact.Reason) return;
        if (f.BoundaryBuLianShiAnxu is null && m.From.OwnerSeat == donor && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment &&
            (m.To == CardLocation.Processing || m.To == CardLocation.Hand(recipient) || m.To == CardLocation.OutsideGame))
        {
            var r = new BoundaryBuLianShiAnxuReceipt(recipient, donor, m.CardId, m.From, m.Sequence, false);
            ReplaceRuntimeFrame(f.Id, f = f with { BoundaryBuLianShiAnxu = r });
            AdvanceEventRulesAndQueueFact(new BoundaryBuLianShiAnxuPreparedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, recipient, donor, m.From.Zone, m.Sequence));
        }
        if (f.BoundaryBuLianShiAnxu is { Delivered: false } captured && captured.CardId == m.CardId && m.To == CardLocation.Hand(recipient) &&
            (m.From == captured.From && m.Sequence == captured.MovementSequence || m.From == CardLocation.Processing && m.Sequence > captured.MovementSequence))
        {
            ReplaceRuntimeFrame(f.Id, GetActiveProgramFrame(f.Id) with { BoundaryBuLianShiAnxu = captured with { Delivered = true, DeliverySequence = m.Sequence } });
            AdvanceEventRulesAndQueueFact(new BoundaryBuLianShiAnxuDeliveredEvent(f.Id, captured.MovementSequence, m.Sequence));
        }
    }
    private void PrepareBoundaryBuLianShiAnxu(ProgramSkillFrame f)
    {
        if (f.SkillId != BoundaryBuLianShiComposition.Anxu || f.InstructionIndex != 2 || f.SelectedTargetSeats is not [var recipient, var donor] ||
            recipient == donor || recipient == f.OwnerSeat || donor == f.OwnerSeat || GetProgramCardSet(f, "anxu-obtained") is not { CardIds: [var id], SourceLocations: [var from] } ||
            f.BoundaryBuLianShiAnxu is { } prior && prior.CardId != id) throw new InvalidOperationException("Anxu requires its exact original recipient/donor and obtained card result.");
        if (f.BoundaryBuLianShiAnxu is null)
        {
            // A native retained-gain replacement can settle without movement.
            // Its result records the original owned source, not an actual gain.
            if (from.OwnerSeat != donor || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) throw new InvalidOperationException("Anxu lost its original native Obtain source receipt.");
            ReplaceRuntimeFrame(f.Id, f = f with { BoundaryBuLianShiAnxu = new(recipient, donor, id, from, 0, false) });
            AdvanceEventRulesAndQueueFact(new BoundaryBuLianShiAnxuPreparedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, recipient, donor, from.Zone, 0));
        }
        if (!ValidBoundaryBuLianShiAnxu(f)) throw new InvalidOperationException("Anxu lost its actual original-source and delivery invoices.");
        var r = f.BoundaryBuLianShiAnxu!;
        // Count the committed original hand-source Obtain, even if a native
        // movement child has already moved that card away from the recipient.
        SetProgramCardSet(f.Id, "anxu-hand-reward", r.Delivered && r.From.Zone == CardZoneKind.Hand ? [id] : [], SkillProgramCardSetVisibility.Private,
            r.Delivered && r.From.Zone == CardZoneKind.Hand ? [r.From] : []);
    }
    private bool ValidBoundaryBuLianShiAnxu(ProgramSkillFrame f) => f.SkillId == BoundaryBuLianShiComposition.Anxu && f.BoundaryBuLianShiAnxu is { } r &&
        r.RecipientSeat != r.DonorSeat && r.RecipientSeat != f.OwnerSeat && r.DonorSeat != f.OwnerSeat && r.From.OwnerSeat == r.DonorSeat && r.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment &&
        ProgramEventHistory<BoundaryBuLianShiAnxuPreparedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillInstanceId == f.SkillInstanceId && e.GameplayHash == f.GameplayHash &&
            e.RecipientSeat == r.RecipientSeat && e.DonorSeat == r.DonorSeat && e.SourceZone == r.From.Zone && e.MovementSequence == r.MovementSequence) == 1 &&
        (r.MovementSequence == 0 ? !r.Delivered && r.DeliverySequence == 0 : _cardMovements.Count(m => m.Sequence == r.MovementSequence && m.CardId == r.CardId && m.From == r.From && m.Reason.Value == "skill-program." + f.SkillId + ".SelectAndMoveOwnedCard" &&
            (m.To == CardLocation.Processing || m.To == CardLocation.Hand(r.RecipientSeat) || m.To == CardLocation.OutsideGame)) == 1) &&
        (r.Delivered ? r.DeliverySequence >= r.MovementSequence && ProgramEventHistory<BoundaryBuLianShiAnxuDeliveredEvent>().Count(e => e.FrameId == f.Id && e.SourceSequence == r.MovementSequence && e.DeliverySequence == r.DeliverySequence) == 1 &&
            _cardMovements.Count(m => m.Sequence == r.DeliverySequence && m.CardId == r.CardId && (m.From == CardLocation.Processing || m.From == r.From) && m.To == CardLocation.Hand(r.RecipientSeat) && m.Reason.Value == "skill-program." + f.SkillId + ".SelectAndMoveOwnedCard") == 1 : r.DeliverySequence == 0);
    private void SelectBoundaryBuLianShiAnxuLeast(ProgramSkillFrame f)
    {
        if (f.InstructionIndex != 4 || !ValidBoundaryBuLianShiAnxu(f) || f.BoundaryBuLianShiAnxu is not { LeastSelected: false } r || f.SelectedTargetSeats is not [var a, var b] || a != r.RecipientSeat || b != r.DonorSeat)
            throw new InvalidOperationException("Anxu must compare its original pair only after its original Obtain and owner reward children return.");
        var first = GetHand(_players[a]).Count; var second = GetHand(_players[b]).Count;
        int? least = r.Delivered && _players[a].IsAlive && _players[b].IsAlive && first != second ? first < second ? a : b : null;
        ReplaceRuntimeFrame(f.Id, f with { BoundaryBuLianShiAnxu = r with { LeastSelected = true, LeastSeat = least }, SelectedTargetSeats = least is { } seat ? [seat] : [] });
        AdvanceEventRulesAndQueueFact(new BoundaryBuLianShiAnxuLeastEvent(f.Id, a, b, first, second, least));
    }
    private bool ValidBoundaryBuLianShiAnxuLeastSelection(ProgramSkillFrame f) => f.InstructionIndex >= 4 && ValidBoundaryBuLianShiAnxu(f) && f.BoundaryBuLianShiAnxu is { LeastSelected: true } r &&
        ProgramEventHistory<BoundaryBuLianShiAnxuLeastEvent>().Count(e => e.FrameId == f.Id && e.RecipientSeat == r.RecipientSeat && e.DonorSeat == r.DonorSeat && e.LeastSeat == r.LeastSeat &&
            (e.LeastSeat is null || e.RecipientHand != e.DonorHand && e.LeastSeat == (e.RecipientHand < e.DonorHand ? r.RecipientSeat : r.DonorSeat))) == 1 &&
        (r.LeastSeat is { } least ? f.SelectedTargetSeats.SequenceEqual([least]) : f.SelectedTargetSeats.Count == 0);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 55900)]
    private bool KeepIssuedBoundaryBuLianShiAnxu(ProgramSkillFrame f) => f.BoundaryBuLianShiAnxu is { MovementSequence: > 0 } && ValidBoundaryBuLianShiAnxu(f);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 55900)]
    private void AssertBoundaryBuLianShiAnxu(ProgramSkillFrame f)
    { if (f.BoundaryBuLianShiAnxu is not null && (!ValidBoundaryBuLianShiAnxu(f) || f.InstructionIndex >= 4 && !ValidBoundaryBuLianShiAnxuLeastSelection(f))) throw new InvalidOperationException("Anxu lost its original source-zone reward or strictly smaller original-pair choice."); }
}
