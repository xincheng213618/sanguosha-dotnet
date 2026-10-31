#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanJunOlRuntime
{
    internal bool TracksSettledActionCards => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.XunxianGiftUsedCard) == true || _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.TongxieFollowUp) == true;

    internal void CaptureSettledActionCards(long batchId, int turnNumber, IReadOnlyList<CardMovementRecord> movements)
    {
        if (!TracksSettledActionCards)
            return;
        foreach (var movement in movements)
        {
            if (movement.To != CardLocation.DiscardPile || movement.From != CardLocation.Processing || movement.Reason.Value is not ("card.use-finished" or "card.response-finished" or "card.respond.nullification-finished"))
                continue;
            var action = SettledActionContext(movement.CardId);
            if (action is null)
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SettledActionCardDiscardEvent(batchId, movement.Sequence, movement.CardId, action.ActorSeat, turnNumber));
            _characterZhaoYanOlOperations.Value.CaptureTongxieSettledSlashUse(batchId, movement);
        }
    }

    internal CardActionContext? SettledActionContext(int cardId)
    {
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Select(frame => frame.Action).LastOrDefault(action => action is not null && action.Type == CardActionType.Use && action.PhysicalCards.Any(cost => cost.CardId == cardId));
        if (use is not null)
            return use;
        var parent = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(frame => frame.Action is not null)?.Action;
        if (parent is null)
            return null;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Select(accepted => accepted.Action).LastOrDefault(action => action.Type == CardActionType.Response && action.ParentActionId == parent.ActionId && action.PhysicalCards.Any(cost => cost.CardId == cardId));
    }

    internal int[] MatchingOwnSettledActionCardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate)
    {
        if (!TracksSettledActionCards)
            return[];
        var settled = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SettledActionCardDiscardEvent>().Where(e => e.BatchId == batch.Id && e.ActorSeat == candidate.OwnerSeat).Select(e => e.MovementSequence).ToHashSet();
        return batch.Movements.Select((movement, index) => (movement, index)).Where(item => settled.Contains(item.movement.Sequence) && item.movement.To == CardLocation.DiscardPile && _cardZonesOperations._cardZones.GetLocation(item.movement.CardId) == CardLocation.DiscardPile).Select(item => item.index).Order().ToArray();
    }

    internal int CurrentPlayPhaseEventIndex(int ownerSeat)
    {
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return Array.FindLastIndex(facts, e => e is PhaseChangedEvent phase && phase.Phase == TurnPhase.Play && phase.ActorSeat == ownerSeat);
    }

    SkillProgramStepOutcome IYanJunProgramHost.GuanchaoChoosePattern(ProgramSkillFrame frame, SkillProgramEffect effect) => GuanchaoProgramChoosePattern(frame, effect);
    SkillProgramStepOutcome IYanJunProgramHost.GuanchaoRankDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => GuanchaoProgramRankDraw(frame, effect);
    SkillProgramStepOutcome IYanJunProgramHost.XunxianGiftUsedCard(ProgramSkillFrame frame, SkillProgramEffect effect) => XunxianProgramGiftUsedCard(frame, effect);
    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 600)]
    internal int[]? SelectWindowIndexes_MatchingOwnSettledActionCardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.XunxianGiftUsedCard))
            return MatchingOwnSettledActionCardIndexes(batch, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CaptureMovementBatchFacts, 600)]
    internal void CaptureMovementBatchFactsCaptureSettledActionCards(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => CaptureSettledActionCards(batch.Id, batch.TurnNumber, movements);
}
