namespace CardGame.Core;

// Source payloads contain shown entity IDs, but not their original Hand holder,
// residency or physical-journal cutoff. Capture those missing facts once here.
[ProgramRuntimeModule]
internal sealed class PublicCardRevealHistoryCaptureRuntime(INativeFactDemand demand,
    IPublicCardRevealQueries reveals, ICurrentHandEntryQueries entries,
    ICardZoneQueries cards, ICardMovementHistoryQueries movements,
    IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramTurnQueries turn, IActualDiscardRecoveryPhaseQueries phases)
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26419)]
    internal void Capture(IGameEvent payload)
    {
        if (!demand.RequiresFact(ProgramTrackedFact.PublicCardRevealHistory)) return;
        var (frameId, ids) = reveals.Describe(payload);
        if (ids.Count == 0) return;
        var selection = frames.Find(frameId)?.PublicHandRevealSelection;
        var sequence = movements.Last()?.Sequence ?? 0;
        var phase = phases.Current();
        var facts = new List<PublicCardRevealHistoryFact>();
        foreach (var id in ids)
        {
            if (cards.GetLocation(id) is not { Zone: CardZoneKind.Hand, OwnerSeat: { } owner } ||
                entries.CurrentHandEntry(owner, id) is not { } entry)
                continue;
            var purpose = selection is { } selected && selected.CardId == id
                ? selected.Purpose : PublicCardRevealPurpose.Explicit;
            facts.Add(new(frameId, turn.TurnNumber, turn.TurnOwnerSeat, phase, sequence,
                id, owner, entry, purpose));
        }
        if (facts.Count > 0)
            events.AdvanceFact(new PublicCardRevealHistoryRecordedEvent(Array.AsReadOnly(facts.ToArray())));
    }
}
