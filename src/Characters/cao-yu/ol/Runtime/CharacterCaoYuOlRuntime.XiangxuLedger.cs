#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoYuOlRuntime
{
    internal bool IsXiangxuMinimumTrigger(SkillProgramTrigger t) => t.Effects is [{ Op: SkillProgramEffectOp.ResolveXiangxuMinimumTransition }];
    internal bool IsXiangxuEndingTrigger(SkillProgramTrigger t) => t.Effects is [{ Op: SkillProgramEffectOp.ResolveXiangxuIssuedTurnEndAdjustment }];
    internal GameEngine.CaoYuDeathMinimumCapture? CaptureCaoYuDeathMinimumSources(int victimSeat)
    {
        if (!HasCaoYuCapability || !_coreMatchSetupSetupCompleteAccess._setupComplete || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(victimSeat) || !_coreMatchParticipantsPlayersAccess._players[victimSeat].IsAlive || !CaoYuActualTurnOpen(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DeathFrame { ReturnKind: { } kind } death || death.VictimSeat != victimSeat)
            return null;
        var alive = _coreMatchParticipantsPlayersAccess._players.Select(p => p.IsAlive).ToArray();
        var counts = _coreMatchParticipantsPlayersAccess._players.Select(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count).ToArray();
        var remaining = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != victimSeat).ToArray();
        if (remaining.Length == 0)
            return null;
        var before = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Min(p => counts[p.Seat]);
        var after = remaining.Min(p => counts[p.Seat]);
        if (before == after)
            return null;
        var sources = remaining.Where(p => counts[p.Seat] == after && counts[p.Seat] != before).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.CardsMoved)).Where(c => IsXiangxuMinimumTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c))).ToArray();
        return sources.Length == 0 ? null : new(death.Id, death.ParentFrameId, victimSeat, death.KillerSeat, kind, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, sources, counts, alive);
    }

    internal void RecordCaoYuDeathMinimumTransitions(long deathFrameId, int victimSeat, GameEngine.CaoYuDeathMinimumCapture? capture)
    {
        if (capture is not { } r)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DeathFrame death || death.Id != deathFrameId || death.Id != r.DeathFrameId || death.ParentFrameId != r.ParentFrameId || death.VictimSeat != victimSeat || victimSeat != r.VictimSeat || death.KillerSeat != r.KillerSeat || death.ReturnKind != r.ReturnKind || !CaoYuActualTurnOpen(r.ActualTurn, r.TurnOwner) || r.HandCounts.Count != _coreMatchParticipantsPlayersAccess._players.Count || r.Alive.Count != _coreMatchParticipantsPlayersAccess._players.Count || !r.Alive[victimSeat] || _coreMatchParticipantsPlayersAccess._players.Where((p, i) => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count != r.HandCounts[i] || p.IsAlive != (i != victimSeat && r.Alive[i])).Any())
            throw new InvalidOperationException("Xiangxu death minimum requires the exact immediate native alive-to-dead transition with unchanged Hand counts.");
        var before = _coreMatchParticipantsPlayersAccess._players.Where(p => r.Alive[p.Seat]).Min(p => r.HandCounts[p.Seat]);
        var after = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Min(p => r.HandCounts[p.Seat]);
        foreach (var c in r.Sources)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || r.HandCounts[c.OwnerSeat] != after || r.HandCounts[c.OwnerSeat] == before || !IsXiangxuMinimumTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c)))
                throw new InvalidOperationException("Xiangxu death minimum changed its captured original source or causal population edge.");
            var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
            var source = new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId);
            var index = _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuMinimumTransitionEvent>().Count();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuMinimumTransitionEvent(id, index, source, c.GameplayHash, null, r.ActualTurn, r.TurnOwner, r.HandCounts[c.OwnerSeat], r.HandCounts[c.OwnerSeat], before, after, death.Id));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuMinimumDeathRecordedEvent(id, death.Id, death.ParentFrameId, victimSeat, death.KillerSeat, r.ReturnKind));
            foreach (var p in _coreMatchParticipantsPlayersAccess._players)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuMinimumSeatEvent(id, p.Seat, r.HandCounts[p.Seat], r.HandCounts[p.Seat], p.IsAlive, r.Alive[p.Seat]));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuMinimumArmedEvent(death.Id, id, source, c.GameplayHash));
        }
    }

    internal void CaptureCaoYuMinimumTransitions(CardMovementBatchContext batch)
    {
        if (!HasCaoYuCapability || !_coreMatchSetupSetupCompleteAccess._setupComplete || !CaoYuActualTurnOpen(batch.TurnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) || !batch.Movements.Any(m => m.From.Zone == CardZoneKind.Hand || m.To.Zone == CardZoneKind.Hand))
            return;
        var bindings = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.CardsMoved)).Where(c => IsXiangxuMinimumTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c))).ToArray();
        if (bindings.Length == 0)
            return;
        var before = _coreMatchParticipantsPlayersAccess._players.Select(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count).ToArray();
        var after = before.ToArray();
        foreach (var m in batch.Movements)
        {
            if (m.From == m.To)
                continue;
            if (m.From is { Zone: CardZoneKind.Hand, OwnerSeat: { } from })
                before[from]++;
            if (m.To is { Zone: CardZoneKind.Hand, OwnerSeat: { } to })
                before[to]--;
        }

        if (before.Any(n => n < 0))
            throw new InvalidOperationException("CaoYu cannot reconstruct a negative pre-batch Hand count.");
        var alive = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray();
        if (alive.Length == 0)
            return;
        var minimumBefore = alive.Min(s => before[s]);
        var minimumAfter = alive.Min(s => after[s]);
        foreach (var c in bindings.Where(c => after[c.OwnerSeat] == minimumAfter && (before[c.OwnerSeat] != after[c.OwnerSeat] || before[c.OwnerSeat] != minimumBefore)))
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat];
            var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
            var index = _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuMinimumTransitionEvent>().Count();
            var source = new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuMinimumTransitionEvent(id, index, source, c.GameplayHash, batch.Id, batch.TurnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, before[owner.Seat], after[owner.Seat], minimumBefore, minimumAfter));
            foreach (var p in _coreMatchParticipantsPlayersAccess._players)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoYuMinimumSeatEvent(id, p.Seat, before[p.Seat], after[p.Seat], p.IsAlive));
        }
    }

    internal CaoYuMinimumTransitionEvent? CaoYuTransition(ProgramTriggerCandidate c, long? id = null) => _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuMinimumTransitionEvent>().SingleOrDefault(e => (id is null || e.OpportunityId == id) && e.Index == c.OccurrenceIndex && e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId && e.Source.SkillInstanceId == c.SkillInstanceId && e.GameplayHash == c.GameplayHash);
    internal bool ValidCaoYuTransition(CaoYuMinimumTransitionEvent e, CardMovementBatchContext? batch = null)
    {
        if (e.Source.SkillId != "ol:xiangxu" || e.Source.BindingId != "minimum-edge" || _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash != e.GameplayHash || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(e.Source.OwnerSeat) || e.HandAfter != e.MinimumAfter || e.HandBefore == e.HandAfter && e.HandBefore == e.MinimumBefore)
            return false;
        var seats = _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuMinimumSeatEvent>().Where(x => x.OpportunityId == e.OpportunityId).OrderBy(x => x.Seat).ToArray();
        if (seats.Length != _coreMatchParticipantsPlayersAccess._players.Count || !seats.Select(x => x.Seat).SequenceEqual(Enumerable.Range(0, _coreMatchParticipantsPlayersAccess._players.Count)) || seats.Any(x => x.Before < 0 || x.After < 0) || !seats.Any(x => x.Alive) || seats.Where(x => x.Alive).Min(x => x.After) != e.MinimumAfter || seats[e.Source.OwnerSeat] is not { Alive: true } own || own.Before != e.HandBefore || own.After != e.HandAfter)
            return false;
        if (e.DeathFrameId is { } deathId)
        {
            var deaths = _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuMinimumDeathRecordedEvent>().Where(x => x.OpportunityId == e.OpportunityId).ToArray();
            if (batch is not null || e.BatchId is not null || deaths is not [var d] || d.DeathFrameId != deathId || deathId < 1 || d.ParentFrameId < 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(d.VictimSeat) || d.VictimSeat == e.Source.OwnerSeat || !Enum.IsDefined(d.ReturnKind) || seats.Any(s => s.AliveBefore is null || s.Before != s.After || s.Alive != (s.Seat != d.VictimSeat && s.AliveBefore.Value)) || seats[d.VictimSeat].AliveBefore != true || seats.Where(s => s.AliveBefore == true).Min(s => s.Before) != e.MinimumBefore || _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuMinimumArmedEvent>().Count(a => a.FrameId == deathId && a.OpportunityId == e.OpportunityId && a.Source == e.Source && a.GameplayHash == e.GameplayHash) != 1)
                return false;
            return true;
        }

        if (e.BatchId is null || seats.Any(s => s.AliveBefore is not null) || seats.Where(x => x.Alive).Min(x => x.Before) != e.MinimumBefore || _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuMinimumDeathRecordedEvent>().Any(x => x.OpportunityId == e.OpportunityId))
            return false;
        if (batch is null)
            return true;
        if (batch.Id != e.BatchId || batch.TurnNumber != e.ActualTurn || batch.Movements.Count == 0)
            return false;
        foreach (var seat in seats)
        {
            var delta = batch.Movements.Count(m => m.To == CardLocation.Hand(seat.Seat) && m.From != m.To) - batch.Movements.Count(m => m.From == CardLocation.Hand(seat.Seat) && m.From != m.To);
            if (seat.After - seat.Before != delta)
                return false;
        }

        return batch.Movements.All(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 4000)]
    internal void CollectXiangxuMinimumCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result)
    {
        if (!HasCaoYuCapability)
            return;
        foreach (var e in _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuMinimumTransitionEvent>().Where(e => e.BatchId == batch.Id && _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive && ValidCaoYuTransition(e, batch)))
            result.Add(new(e.Source.OwnerSeat, e.Source.SkillId, e.Source.BindingId, e.Source.SkillInstanceId, e.GameplayHash, 0, e.Index));
    }

    internal ProgramSkillWindowContext CreateXiangxuMinimumContext(CardsMovedTriggerWindowFrame f, ProgramTriggerCandidate c)
    {
        var e = CaoYuTransition(c) ?? throw new InvalidOperationException("Xiangxu minimum lost its issued original transition.");
        return new(SkillProgramTriggerWindow.CardsMoved, f.Id, c.OwnerSeat, SourceSeat: e.TurnOwner, TargetSeat: c.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, MovementBatch: f.Batch)
        {
            CaoYuOpportunityId = e.OpportunityId
        };
    }

    [ProgramIssuedTrigger(410)]
    internal bool IsXiangxuMinimumCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != "ol:xiangxu" || c.BindingId != "minimum-edge" || context.Window != SkillProgramTriggerWindow.CardsMoved || context.OwnerSeat != c.OwnerSeat || context.CaoYuOpportunityId is not { } id || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || CaoYuTransition(c, id)is not { } e || !CaoYuActualTurnOpen(e.ActualTurn, e.TurnOwner) || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId)is not { } w || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex] != c || !ValidCaoYuTransition(e, w.Batch))
            return false;
        return CreateXiangxuMinimumContext(w, c) == context;
    }

    internal bool CaoYuOpportunityConsumed(long id) => _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuOpportunityConsumedEvent>().Any(e => e.OpportunityId == id);
    internal IEnumerable<CaoYuMinimumTransitionEvent> CaoYuOutstanding(int turn) => _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuMinimumTransitionEvent>().Where(e => e.ActualTurn == turn && ValidCaoYuTransition(e) && !CaoYuOpportunityConsumed(e.OpportunityId) && _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuMinimumArmedEvent>().Count(a => a.OpportunityId == e.OpportunityId && a.Source == e.Source && a.GameplayHash == e.GameplayHash) == 1);
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeXiangxuAfterTurnEndedItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary)
    {
        if (!HasCaoYuCapability)
            return ordinary;
        var items = ordinary.Where(i => i.Candidate is not { SkillId: "ol:xiangxu", BindingId: "end-own" or "end-foreign" } || parent.AfterTurnEnded is not null).ToList();
        foreach (var e in CaoYuOutstanding(parent.TurnNumber).Where(e => e.TurnOwner == parent.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive && CaoYuActualTurnOpen(e.ActualTurn, e.TurnOwner)).OrderBy(e => e.Index))
        {
            var binding = e.Source.OwnerSeat == parent.OwnerSeat ? "end-own" : "end-foreign";
            var c = new ProgramTriggerCandidate(e.Source.OwnerSeat, e.Source.SkillId, binding, e.Source.SkillInstanceId, e.GameplayHash, 0, e.Index);
            var stable = $"xiangxu:{(e.OpportunityId)}";
            if (!items.Any(i => i.StableIdentity == stable))
                items.Add(new(TurnEndingBoundaryItemKind.Program, 0, stable, c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])));
        }

        return items.SequenceEqual(ordinary) ? ordinary : Array.AsReadOnly(items.ToArray());
    }

    internal ProgramSkillWindowContext CreateXiangxuAfterTurnEndedContext(DeferredTurnEndFrame p, ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == "ol:xiangxu" && c.BindingId is "end-own" or "end-foreign" && CaoYuTransition(c)is { } e ? context with
    {
        CaoYuOpportunityId = e.OpportunityId
    }

    : context;
    [ProgramIssuedTrigger(450)]
    internal bool IsIssuedXiangxuEndingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != "ol:xiangxu" || c.BindingId is not ("end-own" or "end-foreign") || context.Window != SkillProgramTriggerWindow.AfterTurnEnded || context.OwnerSeat != c.OwnerSeat || context.CaoYuOpportunityId is not { } id || CaoYuTransition(c, id)is not { } e || !ValidCaoYuTransition(e) || !CaoYuActualTurnOpen(e.ActualTurn, e.TurnOwner) || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } p || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(p) || _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(p) != c || p.TurnNumber != e.ActualTurn || p.OwnerSeat != e.TurnOwner || context.SourceSeat != p.OwnerSeat || context.TargetSeat != p.OwnerSeat || context.OccurrenceIndex != e.Index || context.Facts != p.AfterTurnEnded!.Items[p.AfterTurnEnded.ItemIndex].Facts || c.BindingId != (c.OwnerSeat == p.OwnerSeat ? "end-own" : "end-foreign"))
            return false;
        return _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && !CaoYuOpportunityConsumed(id) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.CaoYu is { OpportunityId: var key, Op: SkillProgramEffectOp.ResolveXiangxuIssuedTurnEndAdjustment } r && key == id && r.Source == CaoYuSource(f) && f.WindowContext == context && (r.Paid || _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 2000)]
    internal bool ExcludeOrdinaryMovement_IsXiangxuMinimumTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsXiangxuMinimumTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 2100)]
    internal ProgramSkillWindowContext? TryCreateXiangxuMinimumContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsXiangxuMinimumTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateXiangxuMinimumContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 2300)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureCaoYuMinimumTransitions(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureCaoYuMinimumTransitions(completed);
        return completed;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 210, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveXiangxuMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsXiangxuMinimumTrigger(trigger) ? IsXiangxuMinimumCandidate(candidate, context) : null;
}
