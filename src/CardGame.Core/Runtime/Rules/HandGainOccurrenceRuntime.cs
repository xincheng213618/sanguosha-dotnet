namespace CardGame.Core;

internal interface ICardMovementTimingQueries : IProgramSelectionQuery
{
    CardMovementTiming? Capture();
}

internal interface IHandGainCardSnapshotQueries : IProgramSelectionQuery
{
    HandGainCardSnapshot Capture(int recipient, int cardId, int sequence, CardLocation from, bool actualGain);
}

internal interface IHandGainOccurrences : IProgramRuntimeOperations
{
    CardMovementBatchContext Capture(CardMovementBatchContext batch);
    void Complete(NativeDrawInvocationProof proof);
    void Complete(ProgramSkillFrame frame, NativeDrawAttemptFrame attempt, NativeDrawReplacementResult replacement);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class HandGainOccurrenceRuntime(INativeFactDemand demand, ICardZoneQueries cards,
    ICardMovementHistoryQueries history, IMatchParticipantStateAccess participants,
    IProgramWindowCandidateQueries bindings, IHandGainCardSnapshotQueries snapshots,
    ICardMovementBatchQueue batches, ICardMovementHandGains gains) : IHandGainOccurrences
{
    public CardMovementBatchContext Capture(CardMovementBatchContext batch)
    {
        if (!demand.RequiresFact(ProgramTrackedFact.HandGainOccurrence)) return batch;
        var receipts = batch.Movements.Where(m => m.To.Zone == CardZoneKind.Hand && m.To.OwnerSeat is not null && m.From != m.To)
            .GroupBy(m => m.To.OwnerSeat!.Value).Select(group => Capture(batch, group.Key, group.ToArray())).ToArray();
        return receipts.Length == 0 ? batch : batch with { HandGains = receipts };
    }

    private HandGainOccurrence Capture(CardMovementBatchContext batch, int recipient, IReadOnlyList<CardMovementRecord> incoming)
    {
        var journal = history.Read();
        var owner = participants.At(recipient);
        var ids = incoming.Select(m => m.CardId).ToHashSet();
        var sources = owner.IsAlive ? bindings.Shard(owner).GetTriggerCandidates(SkillProgramTriggerWindow.CardsGained) : [];
        var firstSequence = incoming.Min(g => g.Sequence);
        var first = batch.TurnNumber > 0 && batch.MovementTiming is { } timing &&
            !(timing.Phase == TurnPhase.Draw && timing.PhaseActorSeat == recipient) &&
            !journal.Any(m => m.Sequence < firstSequence && m.TurnNumber == batch.TurnNumber &&
                m.To == CardLocation.Hand(recipient) && m.From != m.To && m.MovementTiming is { } before &&
                before.ActualTurnOwnerSeat == timing.ActualTurnOwnerSeat &&
                !(before.Phase == TurnPhase.Draw && before.PhaseActorSeat == recipient) &&
                ActualHandGainFacts.OriginalSource(m, journal) != CardLocation.Hand(recipient));
        return new(batch.Id, recipient, batch.TurnNumber, batch.MovementTiming, owner.IsAlive, first)
        {
            Incoming = incoming.OrderBy(m => m.Sequence).Select(m => snapshots.Capture(recipient, m.CardId, m.Sequence,
                m.From, ActualHandGainFacts.OriginalSource(m, journal, batch) != CardLocation.Hand(recipient))).ToArray(),
            PreviousHand = cards.CardsAt(CardLocation.Hand(recipient)).Where(c => !ids.Contains(c.Id))
                .Select(c => snapshots.Capture(recipient, c.Id, 0, CardLocation.Hand(recipient), false)).ToArray(),
            Sources = sources, InitialSources = sources, InitiallyAlive = owner.IsAlive
        };
    }

    public void Complete(NativeDrawInvocationProof proof)
    {
        if (!demand.RequiresFact(ProgramTrackedFact.HandGainOccurrence)) return;
        var materials = history.Read().Where(m => proof.Materials.Any(p => p.CardId == m.CardId && p.MovementSequence == m.Sequence)).ToArray();
        Merge(proof.ParentFrameId, proof.OwnerSeat, proof.LastBatchId, materials, proof.InvocationId, null);
    }

    public void Complete(ProgramSkillFrame frame, NativeDrawAttemptFrame attempt, NativeDrawReplacementResult replacement)
    {
        if (!demand.RequiresFact(ProgramTrackedFact.HandGainOccurrence) || replacement.GainedIds.Count == 0) return;
        // The attempt owner validated each replacement's own provenance before
        // calling this shared collector. HE and DrawPileSlash are not conflated.
        var materials = history.Read().Where(m => m.Sequence > replacement.SequenceBefore && m.Sequence <= replacement.SequenceAfter &&
            m.To == CardLocation.Hand(attempt.Context.RecipientSeat) && m.Reason == replacement.Reason && replacement.GainedIds.Contains(m.CardId)).ToArray();
        var last = Pending().Single(batch => batch.ParentFrameId == frame.Id && batch.Movements.Contains(materials[^1]));
        Merge(frame.Id, attempt.Context.RecipientSeat, last.Id, materials, null, attempt.Id);
    }

    private IEnumerable<CardMovementBatchContext> Pending()
    {
        for (var i = 0; i < batches.PendingBatchCount; i++) yield return batches.PendingBatchAt(i);
    }

    private void Merge(long? parent, int recipient, long lastBatchId, IReadOnlyList<CardMovementRecord> materials,
        long? invocation, long? attempt)
    {
        var sequences = materials.Select(m => m.Sequence).ToHashSet();
        var relevant = Pending().Where(batch => batch.ParentFrameId == parent &&
            batch.HandGains?.Any(g => g.RecipientSeat == recipient && g.Incoming.Any(m => sequences.Contains(m.GainSequence))) == true).ToArray();
        if (relevant.Length == 0) return;
        if (relevant.SelectMany(batch => batch.HandGains!).Where(g => g.RecipientSeat == recipient &&
                g.Incoming.Any(m => sequences.Contains(m.GainSequence))).Any(g => g.Incoming.Any(m => !sequences.Contains(m.GainSequence))))
            throw new InvalidOperationException("A logical hand acquisition cannot absorb unrelated physical gains.");
        var first = relevant.SelectMany(batch => batch.HandGains!).Where(g => g.RecipientSeat == recipient &&
            g.Incoming.Any(m => sequences.Contains(m.GainSequence))).MinBy(g => g.Incoming.Min(m => m.GainSequence))!;
        var final = Pending().Single(batch => batch.Id == lastBatchId);
        var receipt = Capture(final, recipient, materials) with
        {
            NativeDrawInvocationId = invocation, NativeDrawAttemptId = attempt,
            InitialSources = first.InitialSources, InitiallyAlive = first.InitiallyAlive,
            FirstOutsideDraw = first.FirstOutsideDraw, Timing = first.Timing
        };
        for (var i = 0; i < batches.PendingBatchCount; i++)
        {
            var batch = batches.PendingBatchAt(i);
            if (!relevant.Contains(batch)) continue;
            var kept = batch.HandGains!.Where(g => g.RecipientSeat != recipient || !g.Incoming.Any(m => sequences.Contains(m.GainSequence))).ToList();
            if (batch.Id == lastBatchId) kept.Add(receipt);
            gains.AssignHandGains(i, kept.Count == 0 ? null : kept);
        }
    }
}
