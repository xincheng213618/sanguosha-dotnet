#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuFuRenOlRuntime
{
    internal bool HasDuFuRenYise => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveYiseOwnedGain) == true;

    internal static bool IsDuFuRenYiseTrigger(SkillProgramTrigger t) => RuntimeSkills.RuntimeRules_du_fu_ren_ol_Yise_Qualifications.IsDuFuRenYiseTrigger(t);
    internal void CaptureDuFuRenOwnedOrigin(CardMovementRecord m)
    {
        if (!HasDuFuRenYise || !IsDuFuRenOwnedZone(m.From) || m.From == m.To || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenOwnedOriginEvent>().Any(p => p.MovementSequence == m.Sequence))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuFuRenOwnedOriginEvent(m.Sequence, m.From.OwnerSeat!.Value, _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[m.From.OwnerSeat.Value], _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId))));
        if (m.To == CardLocation.Processing && _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { Action: { } a } use && a.ActorSeat == use.SourceSeat && a.ActorSeat != m.From.OwnerSeat && a.EffectiveKind == CardKind.Snatch && a.EffectiveDesignatedTargetSeats.Contains(m.From.OwnerSeat.Value) && m.Reason == CardMoveReasons.Snatch)
            RecordDuFuRenOwnedTransferProducer(m, a.ActorSeat, use.Id);
        if (m.To == CardLocation.Processing && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame program && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(program)is { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Destination: SkillProgramCardDestination.SelectedTargetHand, CardOwnerRef: { } cardOwner, TargetReference: { } recipient } effect && effect.Zones.Contains(m.From.Zone) && _coreProgramCardPaymentsOperations.ResolveProgramParticipant(program, cardOwner) == m.From.OwnerSeat && m.Reason.Value == $"skill-program.{(program.SkillId)}.{(effect.Op)}")
            RecordDuFuRenOwnedTransferProducer(m, _coreProgramCardPaymentsOperations.ResolveProgramParticipant(program, recipient), program.Id);
    }

    internal void RecordDuFuRenOwnedTransferProducer(CardMovementRecord m, int recipientSeat, long producerId)
    {
        if (!HasDuFuRenYise || !IsDuFuRenOwnedZone(m.From) || m.To != CardLocation.Processing || m.From.OwnerSeat == recipientSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipientSeat) || producerId <= 0 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenTransferProducerEvent>().Any(p => p.EntrySequence == m.Sequence))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuFuRenTransferProducerEvent(m.Sequence, producerId, m.From.OwnerSeat!.Value, recipientSeat));
    }

    internal CardMovementBatchContext CaptureDuFuRenGainBatch(CardMovementBatchContext batch)
    {
        if (!HasDuFuRenYise)
            return batch;
        var gains = new List<DuFuRenGain>();
        foreach (var m in batch.Movements.Where(m => m.To is { Zone: CardZoneKind.Hand, OwnerSeat: not null }))
        {
            CardMovementRecord? entry = IsDuFuRenOwnedZone(m.From) ? m : null;
            if (entry is null && m.From == CardLocation.Processing)
            {
                var previous = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(p => p.CardId == m.CardId && p.Sequence < m.Sequence);
                if (previous is { To.Zone: CardZoneKind.Processing } && IsDuFuRenOwnedZone(previous.From) && _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenTransferProducerEvent>().Any(p => p.EntrySequence == previous.Sequence && p.OwnerSeat == previous.From.OwnerSeat && p.RecipientSeat == m.To.OwnerSeat))
                    entry = previous;
            }

            if (entry is null || entry.From.OwnerSeat is not { } owner || m.To.OwnerSeat == owner)
                continue;
            var origin = _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenOwnedOriginEvent>().SingleOrDefault(p => p.MovementSequence == entry.Sequence && p.OwnerSeat == owner);
            if (origin is null)
                continue;
            gains.Add(new(batch.Id, owner, m.To.OwnerSeat!.Value, m.CardId, entry.Sequence, m.Sequence, entry.From, origin.Suit));
        }

        return gains.Count == 0 ? batch : batch with
        {
            DuFuRenGains = gains
        };
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectDuFuRenYiseCandidates(CardMovementBatchContext batch)
    {
        if (batch.DuFuRenGains is not { Count: > 0 } gains)
            return[];
        return gains.GroupBy(g => (g.OwnerSeat, g.RecipientSeat)).Where(g => _coreMatchParticipantsPlayersAccess._players[g.Key.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[g.Key.RecipientSeat].IsAlive).SelectMany(g => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[g.Key.OwnerSeat], SkillProgramTriggerWindow.CardsMoved).Where(c => IsDuFuRenYiseTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c))).Select(c => c with { OccurrenceIndex = batch.Movements.ToList().FindIndex(m => m.Sequence == g.Min(g => g.GainSequence)) })).ToArray();
    }

    internal ProgramSkillWindowContext CreateDuFuRenYiseContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c)
    {
        var movement = frame.Batch.Movements[c.OccurrenceIndex];
        var gains = frame.Batch.DuFuRenGains!.Where(g => g.OwnerSeat == c.OwnerSeat && g.RecipientSeat == movement.To.OwnerSeat).ToArray();
        return new(SkillProgramTriggerWindow.CardsMoved, frame.Id, c.OwnerSeat, SourceSeat: c.OwnerSeat, TargetSeat: movement.To.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])with { MovedCardCount = gains.Length }, MovementBatch: frame.Batch, MovementIndex: c.OccurrenceIndex);
    }

    internal IReadOnlyList<DuFuRenGain>? CurrentDuFuRenGains(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsDuFuRenYiseTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c)) || context.Window != SkillProgramTriggerWindow.CardsMoved || context.OwnerSeat != c.OwnerSeat || context.SourceSeat != c.OwnerSeat || context.OccurrenceIndex != c.OccurrenceIndex || context.MovementIndex != c.OccurrenceIndex || context.TargetSeat is not { } target || target == c.OwnerSeat || context.MovementBatch is not { DuFuRenGains: { } gains } batch || batch.Id != context.ParentFrameId || c.OccurrenceIndex < 0 || c.OccurrenceIndex >= batch.Movements.Count || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == batch.Id)is not { } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c || !parent.Batch.Movements.SequenceEqual(batch.Movements) || parent.Batch.DuFuRenGains is not { } parentGains || !parentGains.SequenceEqual(gains))
            return null;
        var selected = gains.Where(g => g.OwnerSeat == c.OwnerSeat && g.RecipientSeat == target).ToArray();
        if (selected.Length == 0 || batch.Movements[c.OccurrenceIndex].Sequence != selected.Min(g => g.GainSequence) || selected.Any(g => g.BatchId != batch.Id || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == g.EntrySequence && m.CardId == g.CardId && m.From == g.From) || !batch.Movements.Any(m => m.Sequence == g.GainSequence && m.CardId == g.CardId && m.To == CardLocation.Hand(target))))
            return null;
        return Array.AsReadOnly(selected);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 2300)]
    internal void CollectWindow_CollectDuFuRenYiseCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectDuFuRenYiseCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 3200)]
    internal bool ExcludeOrdinaryMovement_IsDuFuRenYiseTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsDuFuRenYiseTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 2900)]
    internal ProgramSkillWindowContext? TryCreateDuFuRenYiseContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsDuFuRenYiseTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateDuFuRenYiseContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 1300)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureDuFuRenGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => CaptureDuFuRenGainBatch(completed);
    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 190, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveDuFuRenMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsDuFuRenYiseTrigger(trigger) ? CurrentDuFuRenGains(candidate, context)is not null : null;
}
