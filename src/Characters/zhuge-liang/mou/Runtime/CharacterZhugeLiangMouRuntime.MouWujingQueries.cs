#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeLiangMouRuntime
{
    internal bool HasMouWujingAllowance(CharacterState owner) => owner.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && _characterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess._mouWujingAllowances.Contains(owner.Seat) && HasMouOperation(owner, SkillProgramEffectOp.MouWujingDraw);
    internal bool MouDiscardNotFromUse(CardMovementRecord move)
    {
        if (move.Reason == CardMoveReasons.UseFinished || move.Reason == CardMoveReasons.FireAttackFinished || move.Reason == CardMoveReasons.IronChainFinished || move.Reason == CardMoveReasons.DelayedCardFinish || move.Reason == CardMoveReasons.NullificationFinished)
            return false;
        if (move.Reason == CardMoveReasons.ResponseFinished && _characterYanJunOlOperations.SettledActionContext(move.CardId)is { } action && (action.Type == CardActionType.Use || action.Type == CardActionType.Response && (action.EffectiveKind == CardKind.Nullification || _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], action.EffectiveKind))))
            return false;
        return true;
    }

    internal void CaptureMouWujingAllowance(long batchId, IReadOnlyList<CardMovementRecord> movements)
    {
        var seat = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || !HasMouOperation(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramEffectOp.MouWujingDraw) || !movements.Any(move => move.To == CardLocation.DiscardPile && MouDiscardNotFromUse(move)))
            return;
        _characterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess._mouWujingAllowances.Add(seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWujingAllowanceGrantedEvent(seat, batchId));
    }

    internal int[] MatchingMouWujingIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _skillRuntimeStateOperations._skillRuntimeState.GetUsage(candidate.OwnerSeat, candidate.SkillId, MouWujingUsage, SkillUsageScope.Turn) != 0 ? [] : batch.Movements.Select((move, index) => (move, index)).Where(item => item.move.To == CardLocation.DiscardPile && item.move.From.Zone != CardZoneKind.Hand).Select(item => item.index).ToArray();
    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 200)]
    internal int[]? SelectWindowIndexes_MatchingMouWujingIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.MouWujingDraw))
            return MatchingMouWujingIndexes(batch, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CaptureMovementBatchFacts, 700)]
    internal void CaptureMovementBatchFactsCaptureMouWujingAllowance(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => CaptureMouWujingAllowance(batch.Id, movements);
}
