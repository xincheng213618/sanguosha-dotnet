#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXieLingYuOlRuntime
{
    internal int YuandiRemainingMask(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<YuandiNameRemovedEvent>().Where(e => e.OwnerSeat == owner).Aggregate(7, (mask, e) => mask & ~XieLingYuNameBit(e.Name));
    internal (long Sequence, bool NonUse)? YuandiFirstGroup(int owner, int turn, int turnOwner, int mask)
    {
        var found = 0;
        var nonUse = false;
        foreach (var e in _coreProgramEventHistoryOperations.ProgramEventHistory<YuandiDiscardEnteredEvent>().Where(e => e.ActualTurn == turn && e.TurnOwner == turnOwner).OrderBy(e => e.MovementSequence))
        {
            var bit = XieLingYuNameBit(e.Name);
            if ((bit & mask) == 0)
                continue;
            found |= bit;
            nonUse |= !e.FromTrueUse;
            if ((found & mask) == mask)
                return (e.MovementSequence, nonUse);
        }

        return null;
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectYuandiCandidates(CardMovementBatchContext batch)
    {
        if (!HasXieLingYuCapability || batch.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)
            return[];
        var result = new List<ProgramTriggerCandidate>();
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<YuandiPaidEvent>().Any(e => e.OwnerSeat == owner.Seat && e.ActualTurn == batch.TurnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat))
                continue;
            var binding = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.ResolveYuandiDiscardNames }]).OrderBy(c => c.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
            if (binding is null)
                continue;
            var mask = YuandiRemainingMask(owner.Seat);
            if (YuandiFirstGroup(owner.Seat, batch.TurnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, mask)is not { } first)
                continue;
            var index = batch.Movements.ToList().FindIndex(m => m.Sequence == first.Sequence);
            if (index < 0)
                continue;
            var candidate = binding with
            {
                OccurrenceIndex = index
            };
            var fact = new YuandiOpportunityEvent(batch.Id, owner.Seat, candidate.SkillInstanceId, candidate.GameplayHash, batch.TurnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, first.Sequence, mask, first.NonUse, index);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<YuandiOpportunityEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
            result.Add(candidate);
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal YuandiOpportunityEvent? CurrentYuandiOpportunity(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!HasXieLingYuCapability || c.SkillId != "ol:yuandi" || c.BindingId != "discard-name-group" || context.Window != SkillProgramTriggerWindow.CardsMoved || context.OwnerSeat != c.OwnerSeat || context.MovementBatch is not { } batch || context.ParentFrameId != batch.Id || context.OccurrenceIndex != c.OccurrenceIndex || c.OccurrenceIndex < 0 || c.OccurrenceIndex >= batch.Movements.Count || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == batch.Id)is not { } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c || !parent.Batch.Movements.SequenceEqual(batch.Movements))
            return null;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<YuandiOpportunityEvent>().SingleOrDefault(e => e.BatchId == batch.Id && e.OwnerSeat == c.OwnerSeat && e.SkillInstanceId == c.SkillInstanceId && e.GameplayHash == c.GameplayHash && e.OccurrenceIndex == c.OccurrenceIndex && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && batch.Movements[c.OccurrenceIndex].Sequence == e.CompletionSequence);
    }

    internal ProgramSkillWindowContext CreateYuandiMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (_coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects is not [{ Op: SkillProgramEffectOp.ResolveYuandiDiscardNames }] || candidate.OccurrenceIndex < 0 || candidate.OccurrenceIndex >= frame.Batch.Movements.Count || !_coreProgramEventHistoryOperations.ProgramEventHistory<YuandiOpportunityEvent>().Any(e => e.BatchId == frame.Id && e.OwnerSeat == candidate.OwnerSeat && e.SkillInstanceId == candidate.SkillInstanceId && e.GameplayHash == candidate.GameplayHash && e.OccurrenceIndex == candidate.OccurrenceIndex && e.CompletionSequence == frame.Batch.Movements[candidate.OccurrenceIndex].Sequence))
            throw new InvalidOperationException("Yuandi requires its original whole discard batch and issued name-group occurrence.");
        return new(SkillProgramTriggerWindow.CardsMoved, frame.Id, candidate.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: frame.Batch, MovementIndex: candidate.OccurrenceIndex);
    }

    [ProgramIssuedTrigger(300)]
    internal bool IsIssuedYuandiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => CurrentYuandiOpportunity(c, context)is { } e && (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.XieLingYu is { Paid: true } r && r.Op == SkillProgramEffectOp.ResolveYuandiDiscardNames && r.OpportunitySequence == e.CompletionSequence && f.OwnerSeat == c.OwnerSeat && f.WindowContext == context) || _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && !_coreProgramEventHistoryOperations.ProgramEventHistory<YuandiPaidEvent>().Any(p => p.OwnerSeat == c.OwnerSeat && p.ActualTurn == e.ActualTurn && p.TurnOwner == e.TurnOwner));
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 3800)]
    internal void CollectWindow_CollectYuandiCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectYuandiCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 1100)]
    internal bool ExcludeOrdinaryMovement_ResolveYuandiDiscardNames(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveYuandiDiscardNames);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 3000)]
    internal ProgramSkillWindowContext? TryCreateYuandiMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (_coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveYuandiDiscardNames))
            return CreateYuandiMovementContext(frame, candidate);
        return null;
    }
}
