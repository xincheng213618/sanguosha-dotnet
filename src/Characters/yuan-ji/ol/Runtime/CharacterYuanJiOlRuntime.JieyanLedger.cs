#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanJiOlRuntime
{
    internal bool JieyanDisabled(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<YuanJiJieyanDisabledEvent>().Any(e => e.OwnerSeat == owner && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber);
    internal YuanJiLossOpportunityEvent? JieyanOpportunity(ProgramTriggerCandidate c) => _coreProgramEventHistoryOperations.ProgramEventHistory<YuanJiLossOpportunityEvent>().SingleOrDefault(e => e.Index == c.OccurrenceIndex && e.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) && e.GameplayHash == c.GameplayHash);
    internal bool ValidJieyanOpportunity(YuanJiLossOpportunityEvent e, CardMovementBatchContext batch)
    {
        if (e.BatchId != batch.Id || e.ActualTurn != batch.TurnNumber || e.Source.SkillId != "ol:jieyan" || e.Source.BindingId != "hand-pair" || e.Round != _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber || e.FirstCardId == e.SecondCardId || e.FirstSequence >= e.SecondSequence || _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash != e.GameplayHash)
            return false;
        var cards = batch.Movements.Where(m => m.From == CardLocation.Hand(e.LoserSeat) && m.From != m.To).DistinctBy(m => m.CardId).OrderBy(m => m.Sequence).ToArray();
        return cards is [var first, var second] && first.CardId == e.FirstCardId && second.CardId == e.SecondCardId && first.Sequence == e.FirstSequence && second.Sequence == e.SecondSequence && cards.All(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 3900)]
    internal void CollectJieyanCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result)
    {
        if (!HasYuanJiCapability)
            return;
        foreach (var e in _coreProgramEventHistoryOperations.ProgramEventHistory<YuanJiLossOpportunityEvent>().Where(e => e.BatchId == batch.Id && _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[e.LoserSeat].IsAlive && !JieyanDisabled(e.Source.OwnerSeat) && ValidJieyanOpportunity(e, batch)))
            result.Add(new(e.Source.OwnerSeat, e.Source.SkillId, e.Source.BindingId, e.Source.SkillInstanceId, e.GameplayHash, 0, e.Index));
    }

    internal ProgramSkillWindowContext CreateJieyanContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c)
    {
        var e = JieyanOpportunity(c) ?? throw new InvalidOperationException("Jieyan lost its captured atomic Hand pair.");
        return new(SkillProgramTriggerWindow.CardsMoved, frame.Id, c.OwnerSeat, SourceSeat: e.LoserSeat, TargetSeat: e.LoserSeat, OccurrenceIndex: c.OccurrenceIndex, MovementBatch: frame.Batch);
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 190)]
    internal bool IsIssuedJieyanCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != "ol:jieyan" || c.BindingId != "hand-pair" || context.Window != SkillProgramTriggerWindow.CardsMoved || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || JieyanDisabled(c.OwnerSeat) || JieyanOpportunity(c)is not { } e || !_coreMatchParticipantsPlayersAccess._players[e.LoserSeat].IsAlive || e.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } w || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex] != c || !ValidJieyanOpportunity(e, w.Batch))
            return false;
        return CreateJieyanContext(w, c) == context;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 1900)]
    internal bool ExcludeOrdinaryMovement_ResolveJieyanOppositeEndDrawPair(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveJieyanOppositeEndDrawPair);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 3200)]
    internal ProgramSkillWindowContext? TryCreateJieyanContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (_coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveJieyanOppositeEndDrawPair))
            return CreateJieyanContext(frame, candidate);
        return null;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 110, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveJieyanMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveJieyanOppositeEndDrawPair) ? IsIssuedJieyanCandidate(candidate, context) : null;
}
