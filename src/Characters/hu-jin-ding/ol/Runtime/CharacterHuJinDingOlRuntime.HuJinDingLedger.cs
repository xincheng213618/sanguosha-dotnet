#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuJinDingOlRuntime
{
    internal bool HasQingyuanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.SelectQingyuanOpeningPartner);

    internal static CardConversionSource HuJinDingSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_hu_jin_ding_ol_HuJinDingLedger.HuJinDingSource(f);
    internal IEnumerable<QingyuanPartnerSelectedEvent> QingyuanPartners(int owner, string skill, string instance) => _coreProgramEventHistoryOperations.ProgramEventHistory<QingyuanPartnerSelectedEvent>().Where(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.Source.SkillInstanceId == instance);
    internal bool IsLiveQingyuanPartner(QingyuanPartnerSelectedEvent p) => _coreMatchParticipantsPlayersAccess._players[p.PartnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat], p.Source.SkillId, p.Source.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program?.GameplayHash == p.GameplayHash;
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 26820)]
    internal void ObserveHuJinDingFacts(IGameEvent fact)
    {
        if (fact is RoundStartedEvent round && HasZhongshenCapability)
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<ZhongshenRoundBoundaryEvent>().Any(e => e.RoundNumber == round.RoundNumber))
                throw new InvalidOperationException("Zhongshen cannot enter the same actual round twice.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhongshenRoundBoundaryEvent(round.RoundNumber, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }

        if (!HasQingyuanCapability || fact is not DamageAppliedEvent { Amount: > 0 } applied || _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Count(e => e.TargetSeat == applied.TargetSeat) != 1 || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { DamageWasApplied: true } attempt)
            return;
        var damage = _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.ParentFrameId == attempt.ResolutionId && d.TargetSeat == applied.TargetSeat && d.Amount == applied.Amount) ?? throw new InvalidOperationException("Qingyuan lost its exact first actual damage frame.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingyuanFirstDamageEvent(applied.TargetSeat, damage.Id, damage.ParentFrameId, applied.Amount));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 26820)]
    internal void SynchronizeQingyuanPublicMarkers()
    {
        if (!HasQingyuanCapability)
            return;
        var valid = _coreProgramEventHistoryOperations.ProgramEventHistory<QingyuanPartnerSelectedEvent>().Where(IsLiveQingyuanPartner).ToArray();
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
        {
            var marker = PlayerMarkerKind.QingyuanPartner;
            var previous = player.Markers.GetValueOrDefault(marker);
            foreach (var key in player.MarkerSourceCounts.Keys.Where(k => k.Marker == marker).ToArray())
                player.MarkerSourceCounts.Remove(key);
            var sources = valid.Where(p => p.PartnerSeat == player.Seat).GroupBy(p => p.Source.OwnerSeat).ToArray();
            foreach (var source in sources)
                player.MarkerSourceCounts[(marker, source.Key)] = source.Count();
            var count = sources.Sum(s => s.Count());
            if (count == 0)
                player.Markers.Remove(marker);
            else
                player.Markers[marker] = count;
            if (previous != count)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(0, player.Seat, marker, count - previous, count, null, "skill-program.qingyuan.marker"));
        }
    }

    internal static bool IsQingyuanGainTrigger(SkillProgramTrigger t) => global::CardGame.Core.RuntimeSkills.RuntimeRules_hu_jin_ding_ol_HuJinDingLedger.IsQingyuanGainTrigger(t);
    internal void CaptureQingyuanGainBatch(CardMovementBatchContext batch)
    {
        if (!HasQingyuanCapability || batch.TurnNumber < 1)
            return;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsGained).Where(c => IsQingyuanGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(c))))
            {
                var partners = QingyuanPartners(owner.Seat, candidate.SkillId, candidate.SkillInstanceId).Where(IsLiveQingyuanPartner).ToArray();
                var gain = batch.Movements.Select((m, i) => (m, i)).FirstOrDefault(pair => pair.m.To is { Zone: CardZoneKind.Hand, OwnerSeat: { } seat } && partners.Any(p => p.PartnerSeat == seat && p.MovementSequence < pair.m.Sequence) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(pair.m, batch) != CardLocation.Hand(seat));
                if (gain.m is null)
                    continue;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingyuanGainQualifiedEvent(batch.Id, new(candidate.SkillId, candidate.BindingId, owner.Seat, candidate.SkillInstanceId), candidate.GameplayHash, batch.TurnNumber, gain.i, gain.m.To.OwnerSeat!.Value, gain.m.Sequence));
            }
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectQingyuanGainCandidates(CardMovementBatchContext batch)
    {
        if (!HasQingyuanCapability)
            return[];
        return _coreProgramEventHistoryOperations.ProgramEventHistory<QingyuanGainQualifiedEvent>().Where(q => q.BatchId == batch.Id && q.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber && _coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat], q.Source.SkillId, q.Source.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program?.GameplayHash == q.GameplayHash).Select(q => new ProgramTriggerCandidate(q.Source.OwnerSeat, q.Source.SkillId, q.Source.BindingId, q.Source.SkillInstanceId, q.GameplayHash, _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program!.Triggers.Single(t => t.Id == q.Source.BindingId).Priority, q.MovementIndex)).ToArray();
    }

    internal ProgramSkillWindowContext CreateQingyuanGainContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate c)
    {
        var q = _coreProgramEventHistoryOperations.ProgramEventHistory<QingyuanGainQualifiedEvent>().Single(q => q.BatchId == parent.Id && q.Source.OwnerSeat == c.OwnerSeat && q.Source.SkillId == c.SkillId && q.Source.SkillInstanceId == c.SkillInstanceId && q.MovementIndex == c.OccurrenceIndex);
        return new(SkillProgramTriggerWindow.CardsGained, parent.Id, c.OwnerSeat, SourceSeat: q.GainingSeat, TargetSeat: c.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: parent.Batch, MovementIndex: q.MovementIndex);
    }

    internal QingyuanGainQualifiedEvent? QingyuanGainQualification(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardsGained || context.OwnerSeat != c.OwnerSeat || context.TargetSeat != c.OwnerSeat || context.MovementBatch is not { } batch || context.ParentFrameId != batch.Id || context.MovementIndex != c.OccurrenceIndex || context.OccurrenceIndex != c.OccurrenceIndex || c.OccurrenceIndex < 0 || c.OccurrenceIndex >= batch.Movements.Count || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == batch.Id)is not { } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c || !parent.Batch.Movements.SequenceEqual(batch.Movements))
            return null;
        var q = _coreProgramEventHistoryOperations.ProgramEventHistory<QingyuanGainQualifiedEvent>().SingleOrDefault(q => q.BatchId == batch.Id && q.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber && q.Source.OwnerSeat == c.OwnerSeat && q.Source.SkillId == c.SkillId && q.Source.SkillInstanceId == c.SkillInstanceId && q.Source.BindingId == c.BindingId && q.GameplayHash == c.GameplayHash && q.MovementIndex == c.OccurrenceIndex);
        return q is not null && context.SourceSeat == q.GainingSeat && batch.Movements[c.OccurrenceIndex] is { } move && move.Sequence == q.GainSequence && move.To == CardLocation.Hand(q.GainingSeat) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(move, batch) != CardLocation.Hand(q.GainingSeat) && QingyuanPartners(c.OwnerSeat, c.SkillId, c.SkillInstanceId).Any(p => p.PartnerSeat == q.GainingSeat && p.MovementSequence < q.GainSequence) ? q : null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 2600)]
    internal void CollectWindow_CollectQingyuanGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectQingyuanGainCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 3400)]
    internal bool ExcludeOrdinaryMovement_IsQingyuanGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsQingyuanGainTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 2400)]
    internal ProgramSkillWindowContext? TryCreateQingyuanGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsQingyuanGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(candidate)))
            return CreateQingyuanGainContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 3400)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureQingyuanGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureQingyuanGainBatch(completed);
        return completed;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 140, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveQingyuanMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsQingyuanGainTrigger(trigger) ? QingyuanGainQualification(candidate, context)is not null : null;
}
