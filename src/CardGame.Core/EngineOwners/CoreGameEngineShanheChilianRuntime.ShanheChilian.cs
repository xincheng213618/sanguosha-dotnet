#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheChilianRuntime
{
    internal const string ChilianLiangyin = "shanhe:chilian-liangyin";
    internal static bool IsChilianLiangyinMovement(ProgramTriggerCandidate candidate) => candidate.SkillId == ChilianLiangyin && candidate.BindingId is "game-out-draw" or "game-in-discard";
    internal static int[] ChilianLiangyinMovementIndexes(CardMovementBatchContext batch, string bindingId) => batch.Movements.Select((movement, index) => (movement, index)).Where(item => bindingId == "game-out-draw" ? !CoreGameEngineGameDomainsRuntime.IsRuleOutsideGame(item.movement.From) && CoreGameEngineGameDomainsRuntime.IsRuleOutsideGame(item.movement.To) : CoreGameEngineGameDomainsRuntime.IsRuleOutsideGame(item.movement.From) && item.movement.To.Zone == CardZoneKind.Hand).Select(item => item.index).ToArray();
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 78200)]
    internal void CollectChilianLiangyinCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates)
    {
        if (!_contentRegistryOperations._contentRegistry.Skills.ContainsKey(ChilianLiangyin))
            return;
        // This is a global domain-crossing ability. Ordinary own-zone dispatch
        // would miss another player's movement and duplicate the owner's batch.
        candidates.RemoveAll(IsChilianLiangyinMovement);
        foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(player, SkillProgramTriggerWindow.CardsMoved).Where(IsChilianLiangyinMovement))
            {
                var indexes = ChilianLiangyinMovementIndexes(batch, candidate.BindingId);
                if (indexes.Length > 0 && ChilianLiangyinTargets(candidate.OwnerSeat, candidate.BindingId).Count > 0)
                    candidates.Add(candidate with { OccurrenceIndex = indexes[0] });
            }
    }

    internal IReadOnlyList<int> ChilianLiangyinTargets(int ownerSeat, string bindingId) => _coreProgramLifecycleOperations.GetProgramTargetSeats(ownerSeat, bindingId == "game-out-draw" ? SkillProgramTargetKind.OtherLivingWithMoreHandCards : SkillProgramTargetKind.OtherLivingWithFewerHandCardsAndOwnedCards);
    internal ProgramSkillWindowContext CreateChilianLiangyinContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var indexes = ChilianLiangyinMovementIndexes(frame.Batch, candidate.BindingId);
        var movement = frame.Batch.Movements[candidate.OccurrenceIndex];
        return new(SkillProgramTriggerWindow.CardsMoved, frame.Id, candidate.OwnerSeat, SourceSeat: movement.From.OwnerSeat, TargetSeat: movement.To.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: frame.Batch, MovementIndex: candidate.OccurrenceIndex);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 78200)]
    internal bool IsIssuedChilianLiangyinCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (!IsChilianLiangyinMovement(candidate) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat))
            return false;
        return _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program?.GameplayHash == candidate.GameplayHash && context.Window == SkillProgramTriggerWindow.CardsMoved && context.MovementBatch is { } batch && context.ParentFrameId == batch.Id && context.OwnerSeat == candidate.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()is { } window && window.Id == batch.Id && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] == candidate && ChilianLiangyinMovementIndexes(batch, candidate.BindingId)is { Length: > 0 } indexes && indexes[0] == candidate.OccurrenceIndex && context.MovementIndex == candidate.OccurrenceIndex && batch.Movements.All(movement => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(movement)) && ChilianLiangyinTargets(candidate.OwnerSeat, candidate.BindingId).Count > 0;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 300)]
    internal ProgramSkillWindowContext? TryCreateChilianLiangyinContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsChilianLiangyinMovement(candidate))
            return CreateChilianLiangyinContext(frame, candidate);
        return null;
    }
}
