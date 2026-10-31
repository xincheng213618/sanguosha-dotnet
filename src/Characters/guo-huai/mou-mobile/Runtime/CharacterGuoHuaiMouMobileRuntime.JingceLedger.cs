#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoHuaiMouMobileRuntime
{
    internal bool HasMouGuoHuaiMobile => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouGuoHuaiJingceStart);

    internal static CardConversionSource MouGhSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_guo_huai_mou_mobile_Jingce_Ledger.MouGhSource(f);
    internal static bool IsMouGuoHuaiGainTrigger(SkillProgramTrigger t) => global::CardGame.Core.RuntimeSkills.RuntimeRules_guo_huai_mou_mobile_Jingce_Ledger.IsMouGuoHuaiGainTrigger(t);
    internal bool LiveMouGhSource(CardConversionSource source, string hash) => _coreMatchParticipantsPlayersAccess._players[source.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], source.SkillId, source.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.GameplayHash == hash;
    internal IEnumerable<MouGuoHuaiPredictionIssuedEvent> UnverifiedMouGhPredictions() => _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiPredictionIssuedEvent>().Where(p => _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiPredictionPlacedEvent>().Any(e => e.PredictionId == p.PredictionId && e.CardId == p.CardId && e.To == CardLocation.DrawPile) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiPredictionVerifiedEvent>().Any(e => e.PredictionId == p.PredictionId) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiPredictionDueEvent>().Any(e => e.PredictionId == p.PredictionId) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiPredictionConsumedEvent>().Any(e => e.PredictionId == p.PredictionId));
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 32000)]
    internal void FreezeMouGhActualStart(TurnStartedEvent turn)
    {
        if (!HasMouGuoHuaiMobile)
            return;
        foreach (var p in UnverifiedMouGhPredictions().Where(p => p.Source.OwnerSeat == turn.ActorSeat && p.ActualTurn < turn.TurnNumber).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoHuaiPredictionDueEvent(p.PredictionId, turn.TurnNumber, turn.ActorSeat, p.RecipientSeat is null));
    }

    internal bool TryBeginMouGuoHuaiActualStart(CharacterState owner)
    {
        if (!HasMouGuoHuaiMobile || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiStartedEvent>().Any(e => e.Op == SkillProgramEffectOp.ResolveMouGuoHuaiJingceStart && e.Source.OwnerSeat == owner.Seat && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber))
            return false;
        var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner);
        var candidates = _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(owner, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, facts).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.ResolveMouGuoHuaiJingceStart }]).ToArray();
        if (candidates.Length == 0)
            return false;
        if (_resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || owner.Seat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            throw new InvalidOperationException("Jingce actual-start boundary must retain its clean native turn cursor.");
        var frame = new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, owner.Seat, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, candidates, ProgramLifecycleContinuation.MouGuoHuaiActualStart, facts);
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 32000, SkillProgramEffectOp.ResolveMouGuoHuaiJingceStart, SkillProgramEffectOp.ResolveMouGuoHuaiJingcePrediction, SkillProgramEffectOp.ResolveMouGuoHuaiJingceReward)]
    internal bool CanOfferMouGh(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (IsMouGuoHuaiGainTrigger(t))
            return MouGuoHuaiGainQualification(c, context)is not null;
        if (t.Effects is [{ Op: SkillProgramEffectOp.ResolveMouGuoHuaiJingceStart }])
            return context.SourceSeat == c.OwnerSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiStartedEvent>().Any(e => e.Op == SkillProgramEffectOp.ResolveMouGuoHuaiJingceStart && e.Source.OwnerSeat == c.OwnerSeat && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber);
        return context.Window == SkillProgramTriggerWindow.AfterTurnEnded && context.SourceSeat == c.OwnerSeat && _corePublicPileSourcesOperations.PublicPileSources(c.OwnerSeat).Any(s => s.SkillId == c.SkillId && s.SkillInstanceId == c.SkillInstanceId && _corePublicPileSourcesOperations.PublicPileCards(s).Count != 0);
    }

    internal void CaptureMouGuoHuaiGainBatch(CardMovementBatchContext batch)
    {
        if (!HasMouGuoHuaiMobile)
            return;
        foreach (var p in UnverifiedMouGhPredictions().ToArray())
        {
            var placed = _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiPredictionPlacedEvent>().Single(e => e.PredictionId == p.PredictionId);
            var gain = batch.Movements.Select((m, i) => (m, i)).FirstOrDefault(x => x.m.CardId == p.CardId && x.m.Sequence > placed.Sequence && x.m.To is { Zone: CardZoneKind.Hand, OwnerSeat: not null } && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(x.m, batch) != x.m.To);
            if (gain.m?.To.OwnerSeat is not { } recipient)
                continue;
            var success = p.RecipientSeat == recipient;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoHuaiPredictionVerifiedEvent(p.PredictionId, batch.Id, gain.i, gain.m.Sequence, recipient, success));
            if (!success || !LiveMouGhSource(p.Source, p.GameplayHash))
                continue;
            foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[p.Source.OwnerSeat], SkillProgramTriggerWindow.CardsGained).Where(c => IsMouGuoHuaiGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c)) && c.SkillInstanceId == p.Source.SkillInstanceId))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoHuaiGainQualifiedEvent(p.PredictionId, batch.Id, new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash, gain.i, gain.m.Sequence, recipient));
        }
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectMouGuoHuaiGainCandidates(CardMovementBatchContext batch) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiGainQualifiedEvent>().Where(q => q.BatchId == batch.Id && LiveMouGhSource(q.Source, q.GameplayHash)).Select(q => new ProgramTriggerCandidate(q.Source.OwnerSeat, q.Source.SkillId, q.Source.BindingId, q.Source.SkillInstanceId, q.GameplayHash, _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program!.Triggers.Single(t => t.Id == q.Source.BindingId).Priority, q.MovementIndex)).ToArray();
    internal MouGuoHuaiGainQualifiedEvent? MouGuoHuaiGainQualification(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardsGained || context.OwnerSeat != c.OwnerSeat || context.TargetSeat != c.OwnerSeat || context.MovementBatch is not { } batch || context.ParentFrameId != batch.Id || context.MovementIndex != c.OccurrenceIndex || context.OccurrenceIndex != c.OccurrenceIndex || c.OccurrenceIndex < 0 || c.OccurrenceIndex >= batch.Movements.Count || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == batch.Id)is not { } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c || !parent.Batch.Movements.SequenceEqual(batch.Movements))
            return null;
        var q = _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiGainQualifiedEvent>().SingleOrDefault(q => q.BatchId == batch.Id && q.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) && q.GameplayHash == c.GameplayHash && q.MovementIndex == c.OccurrenceIndex);
        return q is not null && context.SourceSeat == q.GainingSeat && batch.Movements[q.MovementIndex] is { } move && move.CardId == _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiPredictionIssuedEvent>().Single(p => p.PredictionId == q.PredictionId).CardId && move.Sequence == q.Sequence && move.To == CardLocation.Hand(q.GainingSeat) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(move, batch) != move.To && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiPredictionVerifiedEvent>().Count(e => e.PredictionId == q.PredictionId && e.Success && e.BatchId == batch.Id && e.Sequence == q.Sequence && e.GainingSeat == q.GainingSeat) == 1 ? q : null;
    }

    internal ProgramSkillWindowContext CreateMouGuoHuaiGainContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate c)
    {
        var q = _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiGainQualifiedEvent>().Single(q => q.BatchId == parent.Id && q.Source.OwnerSeat == c.OwnerSeat && q.Source.SkillId == c.SkillId && q.Source.SkillInstanceId == c.SkillInstanceId && q.MovementIndex == c.OccurrenceIndex);
        return new(SkillProgramTriggerWindow.CardsGained, parent.Id, c.OwnerSeat, SourceSeat: q.GainingSeat, TargetSeat: c.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: parent.Batch, MovementIndex: q.MovementIndex);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 3000)]
    internal void CollectWindow_CollectMouGuoHuaiGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectMouGuoHuaiGainCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 3700)]
    internal bool ExcludeOrdinaryMovement_IsMouGuoHuaiGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsMouGuoHuaiGainTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 2500)]
    internal ProgramSkillWindowContext? TryCreateMouGuoHuaiGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsMouGuoHuaiGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateMouGuoHuaiGainContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 3600)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouGuoHuaiGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureMouGuoHuaiGainBatch(completed);
        return completed;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 160, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveMouGuoHuaiMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsMouGuoHuaiGainTrigger(trigger) ? MouGuoHuaiGainQualification(candidate, context)is not null : null;
}
