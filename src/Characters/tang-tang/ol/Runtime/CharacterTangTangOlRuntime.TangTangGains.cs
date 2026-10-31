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
            // The acquired batch is one set. Its members cannot witness one another
            // for "other this-colour cards", even when two red cards arrive together.
            var acquired = batch.Movements.Where(g => g.To == CardLocation.Hand(recipient) && g.From != g.To).Select(g => g.CardId).ToHashSet();
            var suit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[recipient], _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId));
            // A plain deck-to-hand obtain also counts. DrawCards' finer physical
            // batches are reconciled to its whole invocation before any drain.
            Add(recipient, false, suit, m.Sequence, m.From, acquired);
            CardMovementRecord? entry = IsTangTangOwnedZone(m.From) ? m : null;
            if (entry is null && m.From == CardLocation.Processing)
            {
                var previous = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(p => p.CardId == m.CardId && p.Sequence < m.Sequence);
                if (previous is { To.Zone: CardZoneKind.Processing } && IsTangTangOwnedZone(previous.From) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhiyiTransferProducerEvent>().Any(e => e.EntrySequence == previous.Sequence && e.RecipientSeat == recipient))
                    entry = previous;
            }

            if (entry is not null && entry.From.OwnerSeat is { } issuer && issuer != recipient && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZhiyiOwnedOriginEvent>().SingleOrDefault(e => e.Sequence == entry.Sequence && e.CardId == entry.CardId && e.From == entry.From)is { } original)
                Add(issuer, true, original.Suit, entry.Sequence, entry.From, new HashSet<int>());
            void Add(int ownerSeat, bool foreign, Suit effective, long entrySequence, CardLocation from, IReadOnlySet<int> excluded)
            {
                if (effective == Suit.None || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive)
                    return;
                var witness = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Where(c => !excluded.Contains(c.Id)).Select(c => (Card: c, Suit: _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[ownerSeat], c))).FirstOrDefault(c => c.Suit != Suit.None && CoreNativeCardClassificationsRuntime.IsRedSuit(c.Suit) == CoreNativeCardClassificationsRuntime.IsRedSuit(effective));
                if (witness.Card is null)
                    return;
                var gain = new TangTangGain(batch.Id, ownerSeat, recipient, m.CardId, foreign, effective, entrySequence, m.Sequence, from, witness.Card.Id, witness.Suit);
                gains.Add(gain);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhiyiGainFrozenEvent(gain));
            }
        }

        return gains.Count == 0 ? batch : batch with
        {
            TangTangGains = gains
        };
    }

    internal void CompleteTangTangNativeDrawQualification(NativeDrawInvocationProof proof)
    {
        if (!HasTangTangZhiyi || !_coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(proof))
            return;
        var index = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.FindIndex(b => b.Id == proof.LastBatchId && b.NativeDrawInvocation == proof);
        if (index < 0)
            throw new InvalidOperationException("Zhiyi lost the real native draw's final pending batch.");
        for (var i = 0; i < _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Count; i++)
        {
            var pending = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches[i];
            if (pending.TangTangGains is not { } preliminary)
                continue;
            var kept = preliminary.Where(g => g.Foreign || g.OwnerSeat != proof.OwnerSeat || g.GainSequence <= proof.SequenceBefore || g.GainSequence > proof.SequenceAfter).ToArray();
            if (kept.Length != preliminary.Count)
                _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches[i] = pending with
                {
                    TangTangGains = kept.Length == 0 ? null : kept
                };
        }

        var batch = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches[index];
        var acquired = proof.Materials.Select(m => m.CardId).ToHashSet();
        var gains = (batch.TangTangGains ?? []).ToList();
        foreach (var material in proof.Materials)
        {
            var suit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[proof.OwnerSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(material.CardId));
            if (suit == Suit.None)
                continue;
            var witness = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[proof.OwnerSeat]).Where(c => !acquired.Contains(c.Id)).Select(c => (Card: c, Suit: _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[proof.OwnerSeat], c))).FirstOrDefault(c => c.Suit != Suit.None && CoreNativeCardClassificationsRuntime.IsRedSuit(c.Suit) == CoreNativeCardClassificationsRuntime.IsRedSuit(suit));
            if (witness.Card is null)
                continue;
            var gain = new TangTangGain(batch.Id, proof.OwnerSeat, proof.OwnerSeat, material.CardId, false, suit, material.MovementSequence, material.MovementSequence, CardLocation.DrawPile, witness.Card.Id, witness.Suit, proof.InvocationId);
            gains.Add(gain);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhiyiGainFrozenEvent(gain));
        }

        if (gains.Count > 0)
            _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches[index] = batch with
            {
                TangTangGains = gains
            };
    }

    internal int TangTangGainOccurrence(CardMovementBatchContext batch, IEnumerable<TangTangGain> gains) => gains.Any(g => g.NativeDrawId != 0) ? batch.Movements.ToList().FindIndex(m => batch.NativeDrawInvocation?.Materials.Any(c => c.MovementSequence == m.Sequence) == true) : batch.Movements.ToList().FindIndex(m => m.Sequence == gains.Min(g => g.GainSequence));
    [ProgramPipelineHook(ProgramPipelinePhase.BeginMovementBatch, 1000)]
    internal void CaptureMovementBatchStartTangTangOwnedOrigins(CardMovementBatchBuilder batch) => batch.TangTangOwnedOrigins = CaptureTangTangOwnedOrigins(batch.SourceCountsBefore.Keys);
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 2900)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureTangTangGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => CaptureTangTangGainBatch(completed, batch.TangTangOwnedOrigins);
}
