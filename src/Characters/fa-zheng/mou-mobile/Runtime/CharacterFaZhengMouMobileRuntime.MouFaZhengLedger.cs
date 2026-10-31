#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFaZhengMouMobileRuntime
{
    internal bool HasMouFaZhengCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.TakeMouFaZhengXuanhuoGain);

    internal static CardConversionSource MouFaZhengSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_fa_zheng_mou_mobile_MouFaZhengLedger.MouFaZhengSource(f);
    internal IEnumerable<MouFaZhengMarkEvent> MouFaZhengMarks() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouFaZhengMarkEvent>().Where(m => !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouFaZhengRemovedEvent>().Any(e => e.MarkId == m.MarkId));
    internal int MouFaZhengExtracted(long mark) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouFaZhengObtainedEvent>().Where(e => e.MarkId == mark).Sum(e => e.Actual);
    internal int MouFaZhengReceived(long mark) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouFaZhengActualGainEvent>().Count(e => e.MarkId == mark);
    internal bool IsLiveMouFaZhengSource(MouFaZhengMarkEvent mark) => _coreMatchParticipantsPlayersAccess._players[mark.Source.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[mark.TargetSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[mark.Source.OwnerSeat], MouFaZhengComposition.Xuanhuo) && _contentRegistryOperations._contentRegistry.GetSkill(mark.Source.SkillId).Program?.GameplayHash == mark.GameplayHash;
    internal static bool IsMouFaZhengGainTrigger(SkillProgramTrigger t) => RuntimeSkills.RuntimeRules_fa_zheng_mou_mobile_MouFaZhengLedger.IsMouFaZhengGainTrigger(t);
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 31800)]
    internal void SynchronizeMouFaZhengMarks()
    {
        if (!HasMouFaZhengCapability)
            return;
        var marks = MouFaZhengMarks().ToArray();
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
        {
            var kind = PlayerMarkerKind.MouFaZhengXuan;
            var previous = player.Markers.GetValueOrDefault(kind);
            foreach (var key in player.MarkerSourceCounts.Keys.Where(k => k.Marker == kind).ToArray())
                player.MarkerSourceCounts.Remove(key);
            var sources = marks.Where(m => m.TargetSeat == player.Seat).GroupBy(m => m.Source.OwnerSeat).ToArray();
            foreach (var source in sources)
                player.MarkerSourceCounts[(kind, source.Key)] = source.Count();
            var count = sources.Sum(s => s.Count());
            if (count == 0)
                player.Markers.Remove(kind);
            else
                player.Markers[kind] = count;
            if (previous != count)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(0, player.Seat, kind, count - previous, count, null, "skill-program.mou-mobile:xuanhuo.marker"));
        }
    }

    internal void CaptureMouFaZhengGainBatch(CardMovementBatchContext batch)
    {
        if (!HasMouFaZhengCapability)
            return;
        var marks = MouFaZhengMarks().ToArray();
        foreach (var mark in marks)
            foreach (var move in batch.Movements.Where(m => m.Sequence > mark.GiftSequence && m.To == CardLocation.Hand(mark.Source.OwnerSeat) && m.From.OwnerSeat == mark.TargetSeat && m.From != m.To))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouFaZhengActualGainEvent(mark.MarkId, batch.Id, move.Sequence, mark.TargetSeat, mark.Source.OwnerSeat));
        if (batch.MovementTiming is not { Phase: not TurnPhase.Draw })
            return;
        foreach (var mark in marks.Where(m => IsLiveMouFaZhengSource(m) && MouFaZhengExtracted(m.MarkId) < 5))
        {
            var gain = batch.Movements.Select((m, i) => (m, i)).FirstOrDefault(x => x.m.Sequence > mark.GiftSequence && x.m.To == CardLocation.Hand(mark.TargetSeat) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(x.m, batch) != x.m.To);
            if (gain.m is null)
                continue;
            foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[mark.Source.OwnerSeat], SkillProgramTriggerWindow.CardsGained).Where(c => IsMouFaZhengGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c))).OrderBy(c => c.SkillInstanceId == mark.Source.SkillInstanceId ? 0 : 1).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).Take(1))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouFaZhengGainQualifiedEvent(batch.Id, new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash, mark.MarkId, gain.i, gain.m.Sequence, mark.TargetSeat));
        }
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectMouFaZhengGainCandidates(CardMovementBatchContext batch) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouFaZhengGainQualifiedEvent>().Where(q => q.BatchId == batch.Id && _coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat], q.Source.SkillId, q.Source.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program?.GameplayHash == q.GameplayHash).Select(q => new ProgramTriggerCandidate(q.Source.OwnerSeat, q.Source.SkillId, q.Source.BindingId, q.Source.SkillInstanceId, q.GameplayHash, _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program!.Triggers.Single(t => t.Id == q.Source.BindingId).Priority, q.MovementIndex)).ToArray();
    internal MouFaZhengGainQualifiedEvent? MouFaZhengQualification(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardsGained || context.OwnerSeat != c.OwnerSeat || context.TargetSeat != c.OwnerSeat || context.MovementBatch is not { MovementTiming.Phase: not TurnPhase.Draw } batch || context.ParentFrameId != batch.Id || context.MovementIndex != c.OccurrenceIndex || context.OccurrenceIndex != c.OccurrenceIndex || c.OccurrenceIndex < 0 || c.OccurrenceIndex >= batch.Movements.Count || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == batch.Id)is not { } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c || !parent.Batch.Movements.SequenceEqual(batch.Movements))
            return null;
        var q = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouFaZhengGainQualifiedEvent>().SingleOrDefault(q => q.BatchId == batch.Id && q.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) && q.GameplayHash == c.GameplayHash && q.MovementIndex == c.OccurrenceIndex);
        return q is not null && context.SourceSeat == q.TargetSeat && batch.Movements[q.MovementIndex] is { } move && move.Sequence == q.GainSequence && move.To == CardLocation.Hand(q.TargetSeat) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(move, batch) != move.To && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouFaZhengMarkEvent>().Any(m => m.MarkId == q.MarkId && m.TargetSeat == q.TargetSeat && m.Source.OwnerSeat == c.OwnerSeat && m.GiftSequence < q.GainSequence) ? q : null;
    }

    internal ProgramSkillWindowContext CreateMouFaZhengGainContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate c)
    {
        var q = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouFaZhengGainQualifiedEvent>().Single(q => q.BatchId == parent.Id && q.Source.OwnerSeat == c.OwnerSeat && q.Source.SkillId == c.SkillId && q.Source.SkillInstanceId == c.SkillInstanceId && q.MovementIndex == c.OccurrenceIndex);
        return new(SkillProgramTriggerWindow.CardsGained, parent.Id, c.OwnerSeat, SourceSeat: q.TargetSeat, TargetSeat: c.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: parent.Batch, MovementIndex: q.MovementIndex);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 2800)]
    internal void CollectWindow_CollectMouFaZhengGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectMouFaZhengGainCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 3500)]
    internal bool ExcludeOrdinaryMovement_IsMouFaZhengGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsMouFaZhengGainTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 2700)]
    internal ProgramSkillWindowContext? TryCreateMouFaZhengGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsMouFaZhengGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateMouFaZhengGainContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 3500)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouFaZhengGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureMouFaZhengGainBatch(completed);
        return completed;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 180, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveMouFaZhengMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsMouFaZhengGainTrigger(trigger) ? MouFaZhengQualification(candidate, context)is not null : null;
}
