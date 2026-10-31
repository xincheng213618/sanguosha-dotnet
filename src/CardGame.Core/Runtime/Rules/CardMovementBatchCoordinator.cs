namespace CardGame.Core;

internal interface ICardMovementBatchFacts : IProgramSelectionQuery
{
    int TurnNumber { get; }
    bool SetupComplete { get; }
    Winner Winner { get; }
    EngineStatus Status { get; }
    ProgramSkillFrame? LastProgram { get; }
}

internal interface ICardMovementBatchPipeline : IProgramRuntimeOperations
{
    void Begin(CardMovementBatchBuilder batch);
    void ObserveCommitted(CardMovementBatchBuilder batch, CardMovementRecord movement);
    void CaptureFacts(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements);
    CardMovementBatchContext Complete(CardMovementBatchBuilder batch, CardMovementBatchContext completed);
}

/// <summary>Owns the ordered start, settlement and publication of atomic movement batches.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class CardMovementBatchCoordinator(IProgramRuntimeFrames frames, ICardZoneQueries cards,
    ICardMovementBatchFacts facts, ICardMovementBatchLifecycle batches, ICardMovementBatchPipeline pipeline,
    IEquipmentMovementCapture equipment)
    : IAtomicCardMovementBatches
{
    public CardMovementBatchBuilder Begin(IEnumerable<CardLocation> sourceLocations,
        IEnumerable<CardLocation> destinationLocations)
    {
        // Source enumeration/counting precedes identity allocation; destination
        // enumeration stays between the origin reads as in the native producer.
        var sources = sourceLocations.Distinct().ToDictionary(location => location,
            location => cards.CardsAt(location).Count);
        var batch = new CardMovementBatchBuilder(
            frames.NextId(),
            frames.Top?.Id,
            batches.ActiveBatchId,
            frames.Top is not (DrawFundedDistinctBasicFrame or SkillInvocationFrame or ColdDamageReplacementFrame) &&
            facts.LastProgram is { } awaited && RuntimeSkills.RuntimeRules_Core_SkillPrograms.IsAwaitingProgramMovement(awaited) ? awaited.Id : null,
            facts.TurnNumber,
            sources,
            destinationLocations.Distinct().ToDictionary(location => location,
                location => cards.CardsAt(location).Count),
            frames.Top is SkillInvocationFrame ? null : facts.LastProgram);
        pipeline.Begin(batch);
        batch.EquipmentDepartures = equipment.Capture(sources.Keys);
        batch.SealStartMetadata();
        batches.PushActiveBatch(batch.Id);
        return batch;
    }

    public void Complete(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements,
        bool committed)
    {
        // Pop before every other action, including a failed/mismatched nesting
        // check. Committed observations also apply during setup and game end.
        if (!batches.TryPopActiveBatch(out var activeBatchId) || activeBatchId != batch.Id)
            throw new InvalidOperationException("The atomic card-movement batch nesting changed unexpectedly.");
        if (committed)
            foreach (var movement in movements)
                pipeline.ObserveCommitted(batch, movement);
        if (!committed || movements.Count == 0 || !facts.SetupComplete ||
            facts.Winner != Winner.None || facts.Status == EngineStatus.Completed)
            return;
        var sourceCounts = batch.SourceCountsBefore
            .OrderBy(item => item.Key.Zone)
            .ThenBy(item => item.Key.OwnerSeat)
            .Select(item => new CardMovementSourceCount(item.Key, item.Value, cards.CardsAt(item.Key).Count))
            .ToArray();
        pipeline.CaptureFacts(batch, movements);
        var completed = new CardMovementBatchContext(
            batch.Id,
            batch.ParentFrameId,
            batch.ParentBatchId,
            batch.TurnNumber,
            Array.AsReadOnly(movements.ToArray()),
            Array.AsReadOnly(sourceCounts),
            batch.AwaitingProgramFrameId,
            batch.DestinationCountsBefore.OrderBy(item => item.Key.Zone).ThenBy(item => item.Key.OwnerSeat)
                .Select(item => new CardMovementSourceCount(item.Key, item.Value, cards.CardsAt(item.Key).Count)).ToArray(),
            batch.InvocationOrigin?.SkillId ?? batch.OriginProgram?.SkillId,
            batch.InvocationOrigin?.SkillInstanceId ?? batch.OriginProgram?.SkillInstanceId,
            batch.InvocationOrigin?.OwnerSeat ?? batch.OriginProgram?.OwnerSeat)
        {
            MovementTiming = batch.MovementTiming,
            DiscardRecoveryPhase = batch.DiscardRecoveryPhase,
            MouYishiSources = batch.MouYishiSources,
            EquipmentDepartures = batch.EquipmentDepartures
        };
        batches.AppendCompletedBatch(pipeline.Complete(batch, completed));
    }
}
