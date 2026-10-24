namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IReadOnlyList<NativeDrawInvocationRecordedEvent>? _zhaoZhongDrawInvoiceFacts;
    private Dictionary<(int Turn, int Recipient), NativeDrawInvocationRecordedEvent[]> _zhaoZhongDrawInvoices = [];
    private Dictionary<long, int> _zhaoZhongDrawInvocationCounts = [];

    private void IndexZhaoZhongNativeDrawInvoices()
    {
        // Typed history snapshots are immutable and change identity whenever
        // their committed/pending facts change. This index stores facts only;
        // every query still verifies the actual materials and active proof.
        var facts = ProgramEventHistory<NativeDrawInvocationRecordedEvent>();
        if (ReferenceEquals(facts, _zhaoZhongDrawInvoiceFacts)) return;
        _zhaoZhongDrawInvoices = facts.GroupBy(fact => (fact.TurnNumber, fact.OwnerSeat))
            .ToDictionary(group => group.Key, group => group.ToArray());
        _zhaoZhongDrawInvocationCounts = facts.GroupBy(fact => fact.InvocationId)
            .ToDictionary(group => group.Key, group => group.Count());
        _zhaoZhongDrawInvoiceFacts = facts;
    }

    private bool IsNativeDrawGainMovement(CardMovementRecord movement)
    {
        if (movement.From != CardLocation.DrawPile || movement.To.Zone != CardZoneKind.Hand ||
            movement.To.OwnerSeat is not { } recipient || !_cardMovements.Contains(movement)) return false;
        IndexZhaoZhongNativeDrawInvoices();
        var invoices = (_zhaoZhongDrawInvoices.GetValueOrDefault((movement.TurnNumber, recipient)) ?? []).Where(e =>
            movement.Sequence > e.SequenceBefore && movement.Sequence <= e.SequenceAfter).ToArray();
        if (invoices is not [var invoice] || invoice.InvocationId <= 0 || invoice.LastBatchId <= 0 ||
            invoice.ActualCount <= 0 || invoice.Requested < invoice.ActualCount ||
            !IsValidPlayerSeat(invoice.OwnerSeat) || !IsValidPlayerSeat(invoice.ActualTurnOwnerSeat) ||
            !IsValidPlayerSeat(invoice.PhaseActorSeat) || invoice.TurnNumber < 1 ||
            invoice.SequenceBefore < 0 || invoice.SequenceAfter <= invoice.SequenceBefore ||
            invoice.SequenceAfter > _movementSequence || invoice.Reason == CardMoveReasons.InitialDeal ||
            _zhaoZhongDrawInvocationCounts.GetValueOrDefault(invoice.InvocationId) != 1)
            return false;
        // This native completion fact was issued only after DrawCards verified
        // every actual ID against its structured invocation. Rebuild that exact
        // material proof even after its final movement window has drained.
        var materials = _cardMovements.Where(m => m.TurnNumber == invoice.TurnNumber &&
            m.Sequence > invoice.SequenceBefore && m.Sequence <= invoice.SequenceAfter &&
            m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(recipient) &&
            m.Reason == invoice.Reason).OrderBy(m => m.Sequence).ToArray();
        if (materials.Length != invoice.ActualCount || materials.Select(m => m.CardId).Distinct().Count() != materials.Length ||
            materials.Select(m => m.Sequence).Distinct().Count() != materials.Length || !materials.Contains(movement)) return false;
        var currentProof = _pendingCardsMovedBatches.Select(b => b.NativeDrawInvocation)
            .Concat(_resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(f => f.Batch.NativeDrawInvocation))
            .FirstOrDefault(p => p?.InvocationId == invoice.InvocationId);
        return currentProof is null || ValidNativeDrawInvocation(currentProof) &&
            currentProof.Materials.Select(m => new NativeDrawMaterial(m.CardId, m.PrintedKind, m.MovementSequence))
                .SequenceEqual(materials.Select(m => new NativeDrawMaterial(m.CardId, m.CardKind, m.Sequence)));
    }
}
