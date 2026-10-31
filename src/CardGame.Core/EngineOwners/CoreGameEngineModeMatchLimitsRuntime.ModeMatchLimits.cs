#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineModeMatchLimitsRuntime
{
    internal void RetirePendingMatchAtModeLimit(string? winningTeamId, Func<long, ResolutionFrameKind, IGameEvent> cancellationFact, string processingReason, string judgmentReason)
    {
        _coreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess._winnerSeat = null;
        _coreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess._winnerTeamId = winningTeamId;
        _coreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess._winner = winningTeamId is null ? Winner.Draw : _coreMatchOutcomesOperations.GetWinnerForTeam(winningTeamId);
        var judgments = _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().Where(frame => frame.CardId is not null).Select(frame => (CardId: frame.CardId!.Value, frame.TargetSeat)).Distinct().ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is { } frame)
        {
            _resolutionStackOperations._resolutionStack.CompleteTop(frame.Id, frame.Kind);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(cancellationFact(frame.Id, frame.Kind));
        }

        _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Clear();
        _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Clear();
        _coreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess._pendingCharacterStateChanges.Clear();
        _coreProgramLifecycleOperations.CancelScheduledProgramPhase();
        _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
        _corePhysicalMovementFacadesOperations.MoveAllCards(CardLocation.Processing, CardLocation.DiscardPile, new(processingReason));
        foreach (var judgment in judgments)
            if (_cardZonesOperations._cardZones.GetLocation(judgment.CardId) == CardLocation.Judgment(judgment.TargetSeat))
                _corePhysicalMovementFacadesOperations.MoveCard(_coreCardAttackStateOperations.GetAttackCard(judgment.CardId), CardLocation.Judgment(judgment.TargetSeat), CardLocation.DiscardPile, new(judgmentReason));
        _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Clear();
    }
}
