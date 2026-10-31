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
    internal bool IsRefundableSilvMovement(CardMovementRecord m) => m.To == CardLocation.DiscardPile && _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m)?.OwnerSeat is { } loser && _coreTableSeatQueriesOperations.IsValidPlayerSeat(loser) && _coreMatchParticipantsPlayersAccess._players[loser].IsAlive && SilvMark(m.CardId)is not null && _cardZonesOperations._cardZones.GetLocation(m.CardId) == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(x => x.CardId == m.CardId).Sequence == m.Sequence;
    internal IReadOnlyList<ProgramTriggerCandidate> CollectSilvRefundCandidates(CardMovementBatchContext batch)
    {
        if (!HasLiWanCapability)
            return[];
        var result = new List<ProgramTriggerCandidate>();
        for (var i = 0; i < batch.Movements.Count; i++)
        {
            var move = batch.Movements[i];
            if (!IsRefundableSilvMovement(move) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiWanStartedEvent>().Any(e => e.RefundSequence == move.Sequence))
                continue;
            var mark = SilvMark(move.CardId)!;
            var p = _contentRegistryOperations._contentRegistry.GetSkill(mark.Source.SkillId).Program!;
            var t = p.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.ReturnDiscardedSilvEntity }]);
            result.Add(new(mark.Source.OwnerSeat, mark.Source.SkillId, t.Id, mark.Source.SkillInstanceId, mark.GameplayHash, t.Priority, i));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 2500)]
    internal void CollectWindow_CollectSilvRefundCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectSilvRefundCandidates(batch));
}
