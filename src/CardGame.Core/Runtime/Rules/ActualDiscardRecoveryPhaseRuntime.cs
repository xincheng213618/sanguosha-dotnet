namespace CardGame.Core;

// Actual phases are derived from the original fact journal. The action-query
// cache, physical card journal and owning resolution frames stay with their
// existing owners; this module keeps no phase cursor or recovery sidecar.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ActualDiscardRecoveryPhaseRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramContentQueries content, IProgramEventHistoryQueries history, IProgramEventJournalQueries journal,
    ICardZoneQueries zones, ICardMovementHistoryQueries movements, IProgramTurnQueries turn,
    IProgramPhaseQueries phase, ISelectionParticipantQueries participants, IMatchParticipantStateAccess participantState,
    IProgramWindowCandidateQueries candidates, IProgramBindingFacts bindings, IProgramOwnedZoneDiscardFacts discards,
    IActualDiscardRecoveryPhaseFacts facts, IActualPhaseQueryCache cache)
    : IActualDiscardRecoveryPhaseQueries, IActualDiscardRecoveryPhaseOperations
{
    private bool Tracks => content.RequiresFact(ProgramTrackedFact.ActualDiscardRecoveryPhase) ||
        content.UsesDrawPhaseDrawCountHandLimit || facts.ModeTracksActualDiscardRecoveryPhases || facts.HasLishiCapability ||
        content.UsesEqualNameLengthUse || content.HasCardPolicy(SkillProgramCardPolicyKind.HellShiyouIntercept) ||
        content.HasTriggerWindow(SkillProgramTriggerWindow.NativeDrawStarting) || content.HasSkill(MouChenGongComposition.Zhichi);

    public ActualDiscardRecoveryPhaseKey? Current()
    {
        if (cache.Current is not { } query) return Read();
        if (!query.HasActualDiscardRecoveryPhase)
        {
            query.ActualDiscardRecoveryPhase = Read();
            query.HasActualDiscardRecoveryPhase = true;
        }
        return query.ActualDiscardRecoveryPhase;
    }

    private ActualDiscardRecoveryPhaseKey? Read()
    {
        if (!Tracks || turn.TurnNumber < 1 || phase.Phase is TurnPhase.NotStarted or TurnPhase.Finished) return null;
        // Pending facts follow committed facts. The journal owner searches that
        // exact order in reverse without materializing a complete snapshot.
        var current = journal.LastMatching(e => e is ActualDiscardRecoveryPhaseStartedEvent or
            ActualDiscardRecoveryPhaseRestoredEvent or ActualDiscardRecoveryPhaseClearedEvent) switch
        {
            ActualDiscardRecoveryPhaseStartedEvent e => e.Phase,
            ActualDiscardRecoveryPhaseRestoredEvent e => e.Phase,
            _ => null
        };
        return current is not null && current.TurnNumber == turn.TurnNumber && current.ActualTurnOwnerSeat == turn.TurnOwnerSeat
            ? current : null;
    }

    public void Start(ActualDiscardRecoveryPhaseKind kind, int actor)
    {
        if (!Tracks || turn.TurnNumber < 1 || facts.Winner != Winner.None || !participants.IsAlive(actor)) return;
        var token = history.Read<ActualDiscardRecoveryPhaseStartedEvent>()
            .Select(e => e.Phase.Token).DefaultIfEmpty().Max() + 1;
        events.AdvanceFact(new ActualDiscardRecoveryPhaseStartedEvent(new(token, turn.TurnNumber,
            turn.TurnOwnerSeat, actor, kind)));
    }

    public bool IsRecorded(ActualDiscardRecoveryPhaseKey key) =>
        key.Token > 0 && key.TurnNumber > 0 && participants.IsValidSeat(key.ActualTurnOwnerSeat) &&
        participants.IsValidSeat(key.ActorSeat) && Enum.IsDefined(key.Kind) &&
        history.Read<ActualDiscardRecoveryPhaseStartedEvent>().Count(e => e.Phase == key) == 1;

    public void Clear()
    {
        if (Tracks && Current() is not null)
            events.AdvanceFact(new ActualDiscardRecoveryPhaseClearedEvent(turn.TurnNumber));
    }

    public void Restore(ProgramPhaseSchedule schedule)
    {
        if (!Tracks || schedule.DiscardRecoveryReturnPhase is not { } key) return;
        if (key.Kind != ActualDiscardRecoveryPhaseKind.Preparation ||
            key.TurnNumber != turn.TurnNumber || key.ActualTurnOwnerSeat != turn.TurnOwnerSeat ||
            !IsRecorded(key) || schedule.ParentFrame?.OwnerSeat != key.ActorSeat)
            throw new InvalidOperationException("An inserted phase lost its exact original actual-phase return token.");
        events.AdvanceFact(new ActualDiscardRecoveryPhaseRestoredEvent(schedule.Frame.Id, key));
    }

    public void Restore(DrawPhaseObligationFrame parent)
    {
        if (!Tracks || parent.DiscardRecoveryPhase is not { } key) return;
        if (parent.Stage != DrawPhaseObligationStage.Finished || parent.ActiveWindowFrameId is not null ||
            frames.Top?.Id != parent.Id || parent.Skipped || key.Kind != ActualDiscardRecoveryPhaseKind.Draw ||
            key.ActorSeat != parent.OwnerSeat || key.ActualTurnOwnerSeat != turn.TurnOwnerSeat ||
            key.TurnNumber != parent.ActualTurnNumber || key.TurnNumber != turn.TurnNumber || !IsRecorded(key))
            throw new InvalidOperationException("An extra Draw child lost its exact finished original Draw-phase token.");
        if (facts.Winner != Winner.None || !participants.IsAlive(parent.OwnerSeat)) { Clear(); return; }
        events.AdvanceFact(new ActualDiscardRecoveryPhaseRestoredEvent(parent.Id, key));
    }

    internal IReadOnlyList<DiscardRecoveryEntity> Entities(CardMovementBatchContext batch, int source) =>
        Array.AsReadOnly(batch.Movements.Where(m => discards.DiscardSource(m) is { OwnerSeat: var owner } && owner == source)
            .GroupBy(m => m.CardId).Select(g => g.OrderBy(m => m.Sequence).First())
            .OrderBy(m => m.Sequence).Select(m => new DiscardRecoveryEntity(m.CardId, m.Sequence, discards.DiscardSource(m)!.Value)).ToArray());

    internal bool StillAvailable(DiscardRecoveryEntity entity) =>
        zones.GetLocation(entity.CardId) == CardLocation.DiscardPile &&
        !movements.Read().Any(m => m.CardId == entity.CardId && m.Sequence > entity.MovementSequence && m.From == CardLocation.DiscardPile);

    private void CaptureBatch(long batchId, ActualDiscardRecoveryPhaseKey? key, IReadOnlyList<CardMovementRecord> records)
    {
        if (!Tracks || key is null) return;
        foreach (var group in records.Select(m => (Move: m, From: discards.DiscardSource(m)))
            .Where(x => x.From is { OwnerSeat: not null }).GroupBy(x => x.From!.Value.OwnerSeat!.Value))
        {
            var count = group.Select(x => x.Move.CardId).Distinct().Count();
            if (count >= 2) events.AdvanceFact(new ActualDiscardRecoveryBatchQualifiedEvent(batchId, key, group.Key, count));
        }
    }

    internal ActualDiscardRecoveryBatchQualifiedEvent? Qualification(CardMovementBatchContext batch, int source) =>
        journal.Complete().OfType<ActualDiscardRecoveryBatchQualifiedEvent>().SingleOrDefault(e =>
            e.BatchId == batch.Id && e.DiscardOwnerSeat == source && e.Phase == batch.DiscardRecoveryPhase);

    internal bool PhaseUsed(int owner, string skill, string stateId, ActualDiscardRecoveryPhaseKey key) =>
        journal.Complete().OfType<ActualDiscardRecoveryReturnedEvent>().Any(e => e.Source.OwnerSeat == owner &&
            e.Source.SkillId == skill && e.StateId == stateId && e.Phase.Token == key.Token);

    internal int[] MatchingIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate)
    {
        if (batch.DiscardRecoveryPhase is null) return [];
        var op = bindings.Trigger(candidate).Effects.Single(e => e.Op == SkillProgramEffectOp.RestoreActualDiscardBatch);
        if (PhaseUsed(candidate.OwnerSeat, candidate.SkillId, op.StateId!, batch.DiscardRecoveryPhase)) return [];
        return batch.Movements.Select((m, i) => (Move: m, Index: i, From: discards.DiscardSource(m)))
            .Where(x => x.From is { OwnerSeat: { } source } && source != candidate.OwnerSeat && participants.IsAlive(source))
            .GroupBy(x => x.From!.Value.OwnerSeat!.Value)
            .Where(g => Qualification(batch, g.Key) is { OriginalCount: >= 2 } && Entities(batch, g.Key).Any(StillAvailable))
            .Select(g => g.OrderBy(x => x.Move.Sequence).First().Index).ToArray();
    }

    private ProgramSkillWindowContext CreateContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate)
    {
        var source = discards.DiscardSource(window.Batch.Movements[candidate.OccurrenceIndex])!.Value.OwnerSeat!.Value;
        var fact = Qualification(window.Batch, source) ?? throw new InvalidOperationException("Discard recovery lost its frozen batch qualification.");
        return new(SkillProgramTriggerWindow.DiscardPileReceived, window.Id, candidate.OwnerSeat, SourceSeat: source,
            TargetSeat: candidate.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex,
            Facts: candidates.Capture(participantState.At(candidate.OwnerSeat)) with { MovedCardCount = fact.OriginalCount },
            MovementBatch: window.Batch, MovementIndex: candidate.OccurrenceIndex);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 4300)]
    private ProgramSkillWindowContext? TryCreateActualDiscardRecoveryContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var features = ProgramInstructionResolver.Default.Features(bindings.Trigger(candidate));
        return features.HasOperation(SkillProgramEffectOp.RestoreActualDiscardBatch) ? CreateContext(frame, candidate) : null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 800)]
    private int[]? SelectWindowIndexes_MatchingActualDiscardRecoveryIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        return features.HasOperation(SkillProgramEffectOp.RestoreActualDiscardBatch) ? MatchingIndexes(batch, candidate) : null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.BeginMovementBatch, 800)]
    private void CaptureMovementBatchStartDiscardRecoveryPhase(CardMovementBatchBuilder batch) =>
        batch.DiscardRecoveryPhase = Current();

    [ProgramPipelineHook(ProgramPipelinePhase.CaptureMovementBatchFacts, 100)]
    private void CaptureMovementBatchFactsCaptureActualDiscardRecoveryBatch(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> records) =>
        CaptureBatch(batch.Id, batch.DiscardRecoveryPhase, records);
}
