namespace CardGame.Core;

public sealed partial class GameEngine
{
    private void RetirePendingMatchAtModeLimit(string? winningTeamId,
        Func<long, ResolutionFrameKind, IGameEvent> cancellationFact, string processingReason, string judgmentReason)
    {
        _winnerSeat = null;
        _winnerTeamId = winningTeamId;
        _winner = winningTeamId is null ? Winner.Draw : GetWinnerForTeam(winningTeamId);
        var judgments = _resolutionStack.OfType<JudgmentFrame>().Where(frame => frame.CardId is not null)
            .Select(frame => (CardId: frame.CardId!.Value, frame.TargetSeat)).Distinct().ToArray();
        _pendingDecision = null;
        while (_resolutionStack.LastOrDefault() is { } frame)
        {
            _resolutionStack.CompleteTop(frame.Id, frame.Kind);
            AdvanceEventRulesAndQueueFact(cancellationFact(frame.Id, frame.Kind));
        }
        _pendingCardsMovedBatches.Clear();
        _pendingHpChanges.Clear();
        _pendingCharacterStateChanges.Clear();
        CancelScheduledProgramPhase();
        ClearActualDiscardRecoveryPhase();
        MoveAllCards(CardLocation.Processing, CardLocation.DiscardPile, new(processingReason));
        foreach (var judgment in judgments)
            if (_cardZones.GetLocation(judgment.CardId) == CardLocation.Judgment(judgment.TargetSeat))
                MoveCard(GetAttackCard(judgment.CardId), CardLocation.Judgment(judgment.TargetSeat),
                    CardLocation.DiscardPile, new(judgmentReason));
        _pendingCardsMovedBatches.Clear();
    }
}
