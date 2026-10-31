#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangXiuMouRuntime
{
    internal void CaptureMouZhangXiuAnniversaryDiscards(CardMovementBatchContext batch)
    {
        if (!HasMouZhangXiuAnniversary)
            return;
        foreach (var m in batch.Movements.Where(m => m.To == CardLocation.DiscardPile && m.From != m.To))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuAnniversaryDiscardEvent(batch.TurnNumber, m.CardId, m.Sequence));
    }

    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 36600)]
    internal bool MouZhangXiuAnniversaryFinalizedDamageUse(ProgramTargetCardWindowQuery query) => HasMouZhangXiuAnniversary && MouZhangXiuAnniversaryRuntime.DamageCard(query.Kind) && CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(query.Kind);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 36600)]
    internal void ObserveMouZhangXiuAnniversaryDamage(IGameEvent payload)
    {
        if (!HasMouZhangXiuAnniversary || payload is not DamageAppliedEvent { Amount: > 0 } applied || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not NativeOwners.CardAttackHandle card || !card.DamageWasApplied || card.IsProgramSkillDamage || card.IsProgramJudgmentDamage || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.ParentFrameId != card.ResolutionId || damage.TargetSeat != applied.TargetSeat || damage.SourceSeat != applied.SourceSeat || damage.Amount != applied.Amount || !_coreProgramEventHistoryOperations.ProgramEventHistory<DamageRequestedEvent>().Any(e => e.ResolutionId == damage.Id && e.TargetSeat == applied.TargetSeat && e.SourceSeat == applied.SourceSeat && e.Amount == applied.Amount && e.SourceCard == card.EffectiveCardKind && e.SourceLess == card.IsSourceLess))
            return;
        if (card.EffectiveCardKind is not { } effectiveKind)
            return;
        IReadOnlyList<int> physical;
        if (card.IsDelayedJudgmentDamage)
        {
            if (card.EffectiveCardKind != CardKind.Lightning || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == card.ResolutionId)is not JudgmentFrame { Continuation: JudgmentContinuationKind.Lightning, DelayedCardId: { } id } judgment || card.Card?.Id != id || card.PhysicalCards is not [var entity] || entity.Id != id || !_coreProgramEventHistoryOperations.ProgramEventHistory<LightningResolvedEvent>().Any(e => e.ResolutionId == judgment.Id && e.CardId == id && e.Hit && e.JudgmentTargetSeat == judgment.TargetSeat))
                return;
            physical = [id];
        }
        else
        {
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(card.ResolutionId)is not { Action: { Type: CardActionType.Use } a } use || use.CardKind != card.EffectiveCardKind || !_coreCompletedUndamagedTargetRevealOperations.CompletedUndamagedNativeUseActorMatches(use, card) || !_coreCompletedUndamagedTargetRevealOperations.HasExactAcceptedCompletedUndamagedUse(use, a) || use.PhysicalCardIds is not { } ids || !card.PhysicalCards.Select(c => c.Id).SequenceEqual(ids) && !_coreCompletedUndamagedTargetRevealOperations.HasExactCompletedUndamagedTunanVirtualAttack(use, a, card))
                return;
            physical = ids;
        }

        foreach (var id in physical.Where(id => id > 0).Distinct())
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuAnniversaryDamageEvent>().Any(e => e.DamageFrameId == damage.Id && e.CardId == id))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuAnniversaryDamageEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, card.ResolutionId, damage.Id, id, effectiveKind, applied.TargetSeat, applied.Amount, card.IsDelayedJudgmentDamage));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 4600)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouZhangXiuAnniversaryDiscards(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureMouZhangXiuAnniversaryDiscards(completed);
        return completed;
    }
}
