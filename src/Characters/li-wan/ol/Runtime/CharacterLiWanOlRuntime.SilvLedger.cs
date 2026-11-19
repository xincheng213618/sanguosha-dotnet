#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiWanOlRuntime
{
    internal SilvEntityMarkedEvent? SilvMark(int id) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SilvEntityMarkedEvent>().FirstOrDefault(e => e.CardId == id && ValidSilvMark(e));
    // Registration only; the query still belongs to legacy GameEngine.Silv.Ledger.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 10)]
    internal void ProjectSilvCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HasSilvEntityMark(cardId) ? "闾" : null);
    }

    internal bool HasSilvEntityMark(int cardId) => HasLiWanCapability && SilvMark(cardId)is not null;
    internal bool ValidSilvMark(SilvEntityMarkedEvent e) => e.CardId > 0 && e.Source.SkillId == "ol:silv" && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(e.Source.SkillId)?.Program is { } program && program.GameplayHash == e.GameplayHash && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.GainMovementSequence && m.CardId == e.CardId && m.To.Zone == CardZoneKind.Hand && m.Reason.Value is LianjuGainReason or SilvDrawReason) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiWanPaidEvent>().Any(p => p.FrameId == e.ProducerFrameId && e.GainMovementSequence > p.Before && e.GainMovementSequence <= p.After);
    private int? SilvRefundHolder(CardMovementRecord move)
    {
        if (move.To != CardLocation.DiscardPile ||
            _zoneEntries.CurrentEntry(CardLocation.DiscardPile, move.CardId) != move.Sequence ||
            _coreProgramDiscardOriginsOperations.GetCompletedProgramDiscardSource(move)?.OwnerSeat is not { } holder ||
            !_coreTableSeatQueriesOperations.IsValidPlayerSeat(holder) || !_coreMatchParticipantsPlayersAccess._players[holder].IsAlive)
            return null;
        return holder;
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectSilvRefundCandidates(CardMovementBatchContext batch)
    {
        if (!HasLiWanCapability)
            return[];
        var result = new List<ProgramTriggerCandidate>();
        for (var i = 0; i < batch.Movements.Count; i++)
        {
            var move = batch.Movements[i];
            if (move.To != CardLocation.DiscardPile || SilvMark(move.CardId) is not { } mark || SilvRefundHolder(move) is null ||
                _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiWanStartedEvent>().Any(e => e.RefundSequence == move.Sequence))
                continue;
            var p = _contentRegistryOperations._contentRegistry.GetSkill(mark.Source.SkillId).Program!;
            var t = p.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.ReturnDiscardedSilvEntity }]);
            result.Add(new(mark.Source.OwnerSeat, mark.Source.SkillId, t.Id, mark.Source.SkillInstanceId, mark.GameplayHash, t.Priority, i));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 2500)]
    internal void CollectWindow_CollectSilvRefundCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectSilvRefundCandidates(batch));

    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 4600)]
    internal ProgramSkillWindowContext? CreateSilvRefundContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate)
    {
        if (candidate.SkillId != "ol:silv" ||
            _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects is not [{ Op: SkillProgramEffectOp.ReturnDiscardedSilvEntity }])
            return null;
        var index = candidate.OccurrenceIndex;
        var holder = SilvRefundHolder(window.Batch.Movements[index]);
        var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])
            with { MovedCardCount = 1 };
        return new(SkillProgramTriggerWindow.DiscardPileReceived, window.Id, candidate.OwnerSeat, SourceSeat: holder,
            OccurrenceIndex: index, Facts: facts, MovementBatch: window.Batch, MovementIndex: index);
    }
}
