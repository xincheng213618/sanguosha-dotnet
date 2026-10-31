#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMouRuntime
{
    internal void RecordMouLuSuOwnedTransferProducer(CardMovementRecord m, int recipient, long producer)
    {
        if (!HasMouLuSuAnniversary || m.From.OwnerSeat is not { } owner || owner == recipient || m.To != CardLocation.Processing || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || producer <= 0 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuSuAnniversaryTransferInvoiceEvent(m.Sequence, "native-transfer:" + producer, owner, recipient));
    }

    internal void CaptureMouLuSuNativeTransferProducer(CardMovementRecord m)
    {
        if (!HasMouLuSuAnniversary || m.From.OwnerSeat is not { } owner || m.To != CardLocation.Processing)
            return;
        if (m.Reason == CardMoveReasons.Snatch && _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { Action: { Type: CardActionType.Use, EffectiveKind: CardKind.Snatch } a } use && a.ActorSeat == use.SourceSeat && a.ActorSeat != owner && a.EffectiveDesignatedTargetSeats.Contains(owner))
            RecordMouLuSuOwnedTransferProducer(m, a.ActorSeat, use.Id);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame p && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(p)is { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Destination: SkillProgramCardDestination.SelectedTargetHand, CardOwnerRef: { } holder, TargetReference: { } recipient } effect && effect.Zones.Contains(m.From.Zone) && _coreProgramCardPaymentsOperations.ResolveProgramParticipant(p, holder) == owner && m.Reason.Value == $"skill-program.{(p.SkillId)}.{(effect.Op)}")
            RecordMouLuSuOwnedTransferProducer(m, _coreProgramCardPaymentsOperations.ResolveProgramParticipant(p, recipient), p.Id);
    }

    internal int? MouLuSuTransferSource(CardMovementRecord m)
    {
        if (m.To.Zone != CardZoneKind.Hand || m.To.OwnerSeat is not { } recipient || m.From == m.To)
            return null;
        if (m.From.OwnerSeat is { } direct)
            return direct != recipient ? direct : null;
        if (m.From != CardLocation.Processing)
            return null;
        var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(e => e.CardId == m.CardId && e.Sequence < m.Sequence);
        return entry is { From.OwnerSeat: { } owner } && entry.To == CardLocation.Processing && owner != recipient && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLuSuAnniversaryTransferInvoiceEvent>().Any(e => e.EntrySequence == entry.Sequence && e.Owner == owner && e.Recipient == recipient) ? owner : null;
    }

    internal static bool IsMouLuSuGainTrigger(SkillProgramTrigger t) => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMouLuSuMengmou);
    internal (int Peer, int Index)[] MouLuSuGainPeers(CardMovementBatchContext batch, int owner) => batch.Movements.Select((m, i) => (Move: m, Index: i, Source: MouLuSuTransferSource(m))).Where(x => x.Source is not null && (x.Source == owner || x.Move.To.OwnerSeat == owner)).Select(x => (Peer: x.Source == owner ? x.Move.To.OwnerSeat!.Value : x.Source!.Value, x.Index)).Where(x => x.Peer != owner && _coreMatchParticipantsPlayersAccess._players[x.Peer].IsAlive).DistinctBy(x => x.Peer).ToArray();
    internal IEnumerable<ProgramTriggerCandidate> CollectMouLuSuGainCandidates(CardMovementBatchContext batch)
    {
        if (!HasMouLuSuAnniversary)
            return[];
        return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(owner => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsGained).Where(c => IsMouLuSuGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c))).SelectMany(c => MouLuSuGainPeers(batch, owner.Seat).Select(p => c with { OccurrenceIndex = p.Index }))).ToArray();
    }

    internal int? MouLuSuGainPeer(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.CardsGained || w.OwnerSeat != c.OwnerSeat || w.TargetSeat != c.OwnerSeat || w.MovementBatch is not { } batch || w.ParentFrameId != batch.Id || w.MovementIndex != c.OccurrenceIndex || w.OccurrenceIndex != c.OccurrenceIndex || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(p => p.Id == batch.Id)is not { } p || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || p.Candidates[p.CandidateIndex] != c || !p.Batch.Movements.SequenceEqual(batch.Movements))
            return null;
        var peer = MouLuSuGainPeers(batch, c.OwnerSeat).Where(x => x.Index == c.OccurrenceIndex).ToArray();
        return peer is [var one] && w.SourceSeat == one.Peer ? one.Peer : null;
    }

    internal ProgramSkillWindowContext CreateMouLuSuGainContext(CardsMovedTriggerWindowFrame p, ProgramTriggerCandidate c)
    {
        var peer = MouLuSuGainPeers(p.Batch, c.OwnerSeat).Single(x => x.Index == c.OccurrenceIndex);
        return new(SkillProgramTriggerWindow.CardsGained, p.Id, c.OwnerSeat, SourceSeat: peer.Peer, TargetSeat: c.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: p.Batch, MovementIndex: peer.Index);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 2900)]
    internal void CollectWindow_CollectMouLuSuGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectMouLuSuGainCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 3600)]
    internal bool ExcludeOrdinaryMovement_IsMouLuSuGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsMouLuSuGainTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 2600)]
    internal ProgramSkillWindowContext? TryCreateMouLuSuGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsMouLuSuGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateMouLuSuGainContext(frame, candidate);
        return null;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 170, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveMouLuSuMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsMouLuSuGainTrigger(trigger) ? MouLuSuGainPeer(candidate, context)is not null : null;
}
