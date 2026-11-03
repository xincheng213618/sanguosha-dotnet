namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeGivenCardRuntime(INativeFactDemand demand, IProgramRuntimeFrames frames,
    IProgramRuntimeEvents events, IProgramTurnQueries turn, ISelectionParticipantQueries participants,
    ICardMovementHistoryQueries movements, INativeGivenCardQueries history, INativeGiveScopeQueries scopes)
    : INativeGivenCardOperations
{
    public void Record(long producerId, int giver, int recipient, IReadOnlyList<int> intended, long before,
        HappyWeiYanGainRequestSettlement? requestSettlement = null) =>
        RecordCore(producerId, giver, recipient, intended, before, requestSettlement, false);

    private void RecordCore(long producerId, int giver, int recipient, IReadOnlyList<int> intended, long before,
        HappyWeiYanGainRequestSettlement? requestSettlement, bool requestValidated)
    {
        if (!demand.RequiresFact(ProgramTrackedFact.NativeGive) || intended.Count == 0 || giver == recipient)
            return;
        if (producerId <= 0 || !participants.IsValidSeat(giver) || !participants.IsValidSeat(recipient) ||
            intended.Distinct().Count() != intended.Count || before < 0 || before > LastMovementSequence)
            throw new InvalidOperationException("A native Give requires its original producer and distinct intended physical entities.");
        var destination = CardLocation.Hand(recipient);
        if (requestSettlement is not null && !requestValidated)
            ValidateRequest(requestSettlement, producerId, intended, destination, before);
        var retained = requestSettlement?.RetainedGains.Select(receipt => receipt.Plan.CardId).ToHashSet() ?? [];
        var terminal = intended.Where(id => !retained.Contains(id))
            .Select(id => movements.Last(movement => movement.CardId == id && movement.Sequence > before)).ToArray();
        if (terminal.Any(move => move is null || move.To != destination &&
            move.To != CardLocation.OutsideGame && !IsRedirected(move, destination)))
            return;
        var delivered = terminal.Where(move => move!.To == destination).Cast<CardMovementRecord>()
            .OrderBy(move => move.Sequence).ToArray();
        if (delivered.Length == 0) return;
        if (history.Read().Any(gift => gift.ProducerId == producerId && gift.Giver == giver &&
            gift.Recipient == recipient && gift.Movements.Any(move => delivered.Any(actual => actual.Sequence == move.Sequence))))
            return;
        var completion = terminal.Max(move => (long)move!.Sequence);
        var gift = new NativeGiveFact(frames.NextId(), producerId, giver, recipient,
            turn.TurnNumber, turn.TurnOwnerSeat, completion)
        {
            Movements = delivered,
            ScopeCutoffAtIssue = scopes.CurrentCutoff(turn.TurnNumber, turn.TurnOwnerSeat)
        };
        events.AdvanceFact(new NativeCardsGivenEvent(gift.Id, producerId, giver, recipient,
            delivered.Length, gift.Turn, gift.TurnOwner, completion) { Receipt = gift });
    }

    public Action<CardMovementRecord> Completion(long producerId, int giver, int recipient,
        IReadOnlyList<int> intended, long before, Action<CardMovementRecord>? original = null) => movement =>
    {
        original?.Invoke(movement);
        Record(producerId, giver, recipient, intended, before);
    };

    public Action<long, IReadOnlyList<CardMovementRecord>> CompletionRecords(long producerId, int giver, int recipient,
        IReadOnlyList<int> intended, long before, Action<long, IReadOnlyList<CardMovementRecord>>? original = null) => (batch, actual) =>
    {
        original?.Invoke(batch, actual);
        Record(producerId, giver, recipient, intended, before);
    };

    public Action<HappyWeiYanGainRequestSettlement> RequestCompletion(long producerId, int giver, int recipient,
        IReadOnlyList<int> intended, long before, Action<HappyWeiYanGainRequestSettlement>? original = null) => settlement =>
    {
        ValidateRequest(settlement, producerId, intended, CardLocation.Hand(recipient), before);
        original?.Invoke(settlement);
        RecordCore(producerId, giver, recipient, intended, before, settlement, true);
    };

    private long LastMovementSequence => movements.Last()?.Sequence ?? 0;

    // Replacement issuance is validated by its native producer. Give consumes
    // only the typed outcome and its frozen original/actual destinations.
    private static bool IsRedirected(CardMovementRecord movement, CardLocation requestedTo) =>
        movement.HappyWeiYanGainDestination is { } plan && plan.RequestedTo == requestedTo &&
        movement.CardId == plan.CardId && movement.CardKind == plan.PrintedKind &&
        movement.From == plan.From && movement.To == plan.ActualTo && movement.From != movement.To &&
        movement.Reason == plan.Reason && movement.Sequence > plan.SequenceBefore;

    private void ValidateRequest(HappyWeiYanGainRequestSettlement settlement, long producerId,
        IReadOnlyList<int> intended, CardLocation requestedTo, long before)
    {
        var actual = settlement.Movements;
        var retained = settlement.RetainedGains;
        if (settlement.ProducerFrameId != producerId || settlement.SequenceBefore != before ||
            before < 0 || before > LastMovementSequence || intended.Count == 0 ||
            intended.Distinct().Count() != intended.Count ||
            (actual.Count == 0 ? settlement.MovementBatchId is not null : settlement.MovementBatchId is not > 0) ||
            !actual.Select(move => move.CardId).Concat(retained.Select(receipt => receipt.Plan.CardId))
                .Order().SequenceEqual(intended.Order()) ||
            actual.Any(move => move.Sequence <= before || move.Reason != settlement.Reason ||
                move.From == move.To || !movements.Read().Contains(move) ||
                move.To != requestedTo && move.To != CardLocation.OutsideGame && !IsRedirected(move, requestedTo)) ||
            retained.Any(receipt => receipt.ProducerFrameId != producerId ||
                receipt.Plan.SequenceBefore != before || receipt.Plan.Reason != settlement.Reason ||
                receipt.Plan.RequestedTo != requestedTo || receipt.Plan.From != receipt.Plan.ActualTo ||
                receipt.Plan.From.Zone != CardZoneKind.Hand))
            throw new InvalidOperationException("A native Give requires the complete typed outcome of its original gain request.");
    }
}
