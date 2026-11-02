#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTangTangOlRuntime
{
    internal static bool IsTangTangOwnedZone(CardLocation from) => RuntimeSkills.RuntimeRules_tang_tang_ol_TangTangGains.IsTangTangOwnedZone(from);
    internal IReadOnlyList<TangTangOwnedOrigin>? CaptureTangTangOwnedOrigins(IEnumerable<CardLocation> sources) => !HasTangTangZhiyi ? null : Array.AsReadOnly(sources.Where(IsTangTangOwnedZone).Distinct().SelectMany(from => _cardZonesOperations._cardZones.CardsAt(from).Select(c => new TangTangOwnedOrigin(c.Id, from.OwnerSeat!.Value, from, _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[from.OwnerSeat.Value], c)))).ToArray());
    internal void RecordTangTangOwnedTransferProducer(CardMovementRecord m, int recipientSeat, long producerId)
    {
        if (!HasTangTangZhiyi || !IsTangTangOwnedZone(m.From) || m.To != CardLocation.Processing || m.From.OwnerSeat == recipientSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipientSeat) || producerId <= 0 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhiyiTransferProducerEvent>().Any(e => e.EntrySequence == m.Sequence))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhiyiTransferProducerEvent(m.Sequence, recipientSeat, producerId));
    }

    internal CardMovementBatchContext CaptureTangTangGainBatch(CardMovementBatchContext batch, IReadOnlyList<TangTangOwnedOrigin>? origins)
    {
        if (!HasTangTangZhiyi)
            return batch;
        foreach (var m in batch.Movements.Where(m => IsTangTangOwnedZone(m.From) && m.From != m.To))
        {
            if (origins?.SingleOrDefault(o => o.CardId == m.CardId && o.From == m.From)is not { } original)
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhiyiOwnedOriginEvent(batch.Id, m.Sequence, m.CardId, original.OwnerSeat, original.From, m.To, original.Suit));
            if (m.To != CardLocation.Processing)
                continue;
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { Action: { } a } use && a.EffectiveKind == CardKind.Snatch && a.ActorSeat == use.SourceSeat && a.ActorSeat != original.OwnerSeat && a.EffectiveDesignatedTargetSeats.Contains(original.OwnerSeat) && m.Reason == CardMoveReasons.Snatch)
                RecordTangTangOwnedTransferProducer(m, a.ActorSeat, use.Id);
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame p && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(p)is { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Destination: SkillProgramCardDestination.SelectedTargetHand, CardOwnerRef: { } owner, TargetReference: { } recipient } effect && effect.Zones.Contains(m.From.Zone) && _coreProgramCardPaymentsOperations.ResolveProgramParticipant(p, owner) == original.OwnerSeat && m.Reason.Value == $"skill-program.{(p.SkillId)}.{(effect.Op)}")
                RecordTangTangOwnedTransferProducer(m, _coreProgramCardPaymentsOperations.ResolveProgramParticipant(p, recipient), p.Id);
        }

        var gains = new List<TangTangGain>();
        foreach (var m in batch.Movements.Where(m => m.To.Zone == CardZoneKind.Hand && m.To.OwnerSeat is not null && m.From != m.To))
        {
            var recipient = m.To.OwnerSeat!.Value;
            CardMovementRecord? entry = IsTangTangOwnedZone(m.From) ? m : null;
            if (entry is null && m.From == CardLocation.Processing)
            {
                var previous = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(p => p.CardId == m.CardId && p.Sequence < m.Sequence);
                if (previous is { To.Zone: CardZoneKind.Processing } && IsTangTangOwnedZone(previous.From) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhiyiTransferProducerEvent>().Any(e => e.EntrySequence == previous.Sequence && e.RecipientSeat == recipient))
                    entry = previous;
            }

            if (entry is null || entry.From.OwnerSeat is not { } issuer || issuer == recipient ||
                _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhiyiOwnedOriginEvent>().SingleOrDefault(e => e.Sequence == entry.Sequence && e.CardId == entry.CardId && e.From == entry.From) is not { } original ||
                original.Suit == Suit.None || !_coreMatchParticipantsPlayersAccess._players[issuer].IsAlive)
                continue;
            var witness = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[issuer])
                .Select(c => (Card: c, Suit: _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[issuer], c)))
                .FirstOrDefault(c => c.Suit != Suit.None && CoreNativeCardClassificationsRuntime.IsRedSuit(c.Suit) == CoreNativeCardClassificationsRuntime.IsRedSuit(original.Suit));
            if (witness.Card is null)
                continue;
            var gain = new TangTangGain(batch.Id, issuer, recipient, m.CardId, true, original.Suit,
                entry.Sequence, m.Sequence, entry.From, witness.Card.Id, witness.Suit);
            gains.Add(gain);
        }

        return gains.Count == 0 ? batch : batch with
        {
            TangTangGains = gains
        };
    }

    internal static IReadOnlyList<TangTangGain> ProjectTangTangGains(CardMovementBatchContext batch)
    {
        var gains = (batch.TangTangGains ?? []).Where(gain => gain.Foreign).ToList();
        foreach (var occurrence in batch.HandGains ?? [])
        {
            if (!occurrence.RecipientAlive)
                continue;
            foreach (var incoming in occurrence.Incoming)
            {
                if (incoming.EffectiveSuit == Suit.None)
                    continue;
                var witnesses = occurrence.PreviousHand.Where(card => card.EffectiveSuit != Suit.None &&
                    CoreNativeCardClassificationsRuntime.IsRedSuit(card.EffectiveSuit) == CoreNativeCardClassificationsRuntime.IsRedSuit(incoming.EffectiveSuit)).Take(1).ToArray();
                if (witnesses.Length == 0)
                    continue;
                gains.Add(new(batch.Id, occurrence.RecipientSeat, occurrence.RecipientSeat, incoming.CardId, false,
                    incoming.EffectiveSuit, incoming.GainSequence, incoming.GainSequence, incoming.From,
                    witnesses[0].CardId, witnesses[0].EffectiveSuit, occurrence.NativeDrawInvocationId ?? 0));
            }
        }
        return Array.AsReadOnly(gains.ToArray());
    }

    internal static int TangTangGainOccurrence(CardMovementBatchContext batch, IEnumerable<TangTangGain> gains)
    {
        var selected = gains.ToArray();
        if (selected.Length == 0)
            return -1;
        var owner = selected[0].OwnerSeat;
        if (selected.Any(gain => !gain.Foreign) && batch.HandGains?.FirstOrDefault(gain => gain.RecipientSeat == owner &&
                (gain.NativeDrawInvocationId is not null || gain.NativeDrawAttemptId is not null)) is { } occurrence)
            return batch.Movements.ToList().FindIndex(movement => occurrence.Incoming.Any(card => card.GainSequence == movement.Sequence));
        return batch.Movements.ToList().FindIndex(movement => movement.Sequence == selected.Min(gain => gain.GainSequence));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.BeginMovementBatch, 1000)]
    internal void CaptureMovementBatchStartTangTangOwnedOrigins(CardMovementBatchBuilder batch) => batch.TangTangOwnedOrigins = CaptureTangTangOwnedOrigins(batch.SourceCountsBefore.Keys);
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 2900)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureTangTangGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => CaptureTangTangGainBatch(completed, batch.TangTangOwnedOrigins);
}
