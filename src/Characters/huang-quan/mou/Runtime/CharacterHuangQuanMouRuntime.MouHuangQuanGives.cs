#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangQuanMouRuntime
{
    internal bool HasMouHuangQuan => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouHuangQuanTuicheng);

    internal void RecordNativeGivenCards(long producerId, int giver, int recipient, IReadOnlyList<int> intended, long before, HappyWeiYanGainRequestSettlement? requestSettlement = null)
    {
        if ((!HasMouHuangQuan && !_boundaryYiJiOperations.Get().Enabled) || intended.Count == 0 || giver == recipient)
            return;
        if (producerId <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(giver) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || intended.Distinct().Count() != intended.Count || before < 0 || before > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("A native Give requires its original producer and distinct intended physical entities.");
        if (requestSettlement is not null)
            _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainRequestSettlement(requestSettlement, producerId, intended, CardLocation.Hand(recipient), before);
        var retained = requestSettlement?.RetainedGains.Select(receipt => receipt.Plan.CardId).ToHashSet() ?? [];
        var terminal = intended.Where(id => !retained.Contains(id)).Select(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == id && m.Sequence > before)).ToArray();
        if (terminal.Any(m => m is null || m.To != CardLocation.Hand(recipient) && m.To != CardLocation.OutsideGame && !_coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(m, CardLocation.Hand(recipient))))
            return;
        var delivered = terminal.Where(m => m!.To == CardLocation.Hand(recipient)).Cast<CardMovementRecord>().OrderBy(m => m.Sequence).ToArray();
        if (delivered.Length == 0)
            return;
        var last = terminal.Max(m => (long)m!.Sequence);
        var existing = _coreMatchParticipantsPlayersAccess._players[giver].MouHqActualGifts ?? [];
        if (existing.Any(g => g.ProducerId == producerId && g.Recipient == recipient && g.Movements.Any(m => delivered.Any(d => d.Sequence == m.Sequence))))
            return;
        var gift = new MouHqGift(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, producerId, giver, recipient, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, last)
        {
            Movements = delivered
        };
        _coreMatchParticipantsPlayersAccess._players[giver].MouHqActualGifts = Array.AsReadOnly(existing.Append(gift).ToArray());
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeCardsGivenEvent(gift.Id, producerId, giver, recipient, delivered.Length, gift.Turn, gift.TurnOwner, last));
    }

    internal Action<CardMovementRecord> NativeGiveCompletion(long producer, int giver, int recipient, IReadOnlyList<int> ids, long before, Action<CardMovementRecord>? original = null) => m =>
    {
        original?.Invoke(m);
        RecordNativeGivenCards(producer, giver, recipient, ids, before);
    };
    internal Action<long, IReadOnlyList<CardMovementRecord>> NativeGiveCompletionRecords(long producer, int giver, int recipient, IReadOnlyList<int> ids, long before, Action<long, IReadOnlyList<CardMovementRecord>>? original = null) => (batch, moves) =>
    {
        original?.Invoke(batch, moves);
        RecordNativeGivenCards(producer, giver, recipient, ids, before);
    };
    internal Action<HappyWeiYanGainRequestSettlement> NativeGiveRequestCompletion(long producer, int giver, int recipient, IReadOnlyList<int> ids, long before, Action<HappyWeiYanGainRequestSettlement>? original = null) => settlement =>
    {
        _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainRequestSettlement(settlement, producer, ids, CardLocation.Hand(recipient), before);
        original?.Invoke(settlement);
        RecordNativeGivenCards(producer, giver, recipient, ids, before, settlement);
    };
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 35700)]
    internal void ExpireMouHqGivenEntities(TurnEndedEvent ended)
    {
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            if (p.MouHqActualGifts is { } gifts)
                p.MouHqActualGifts = gifts.Where(g => g.Turn != ended.TurnNumber || g.TurnOwner != ended.ActorSeat).ToArray()is { Length: > 0 } left ? Array.AsReadOnly(left) : null;
    }

    internal IReadOnlyList<MouHqGift> MouHqGiftsForBatch(CardMovementBatchContext batch, int giver) => _closedNativeGivenCardQueriesSource.Get().GiftsForBatch(batch, giver);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectMouHqGiveCandidates(CardMovementBatchContext batch)
    {
        var result = new List<ProgramTriggerCandidate>();
        foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.CardsMoved).Where(c => c.SkillId == MouHqTuicheng && c.BindingId == "after-native-give"))
                foreach (var gift in MouHqGiftsForBatch(batch, p.Seat))
                    result.Add(c with { OccurrenceIndex = Array.FindIndex(batch.Movements.ToArray(), m => m.Sequence == gift.CompletionSequence) });
        return Array.AsReadOnly(result.ToArray());
    }

    internal bool IsMouHqGiveTrigger(SkillProgramTrigger trigger) => trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveMouHuangQuanTuicheng }];
    internal ProgramSkillWindowContext CreateMouHqGiveContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c)
    {
        var gift = MouHqGiftsForBatch(frame.Batch, c.OwnerSeat).Single(g => g.CompletionSequence == frame.Batch.Movements[c.OccurrenceIndex].Sequence);
        return new(SkillProgramTriggerWindow.CardsMoved, frame.Id, c.OwnerSeat, SourceSeat: c.OwnerSeat, TargetSeat: gift.Recipient, Amount: gift.Movements.Count, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: frame.Batch, MovementIndex: c.OccurrenceIndex)
        {
            MouHqGift = gift
        };
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 500)]
    internal void CollectWindow_CollectMouHqGiveCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectMouHqGiveCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 1000)]
    internal bool ExcludeOrdinaryMovement_IsMouHqGiveTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsMouHqGiveTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 3400)]
    internal ProgramSkillWindowContext? TryCreateMouHqGiveContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
        if (IsMouHqGiveTrigger(trigger))
            return CreateMouHqGiveContext(frame, candidate);
        return null;
    }
}
