namespace CardGame.Core;

public sealed partial class GameEngine
{
    private CommandResult SubmitWushuangClock(TickWushuangClockCommand command)
    {
        if (command.ActorSeat != -1)
            return Reject(CommandErrorCode.InvalidActor, "无双计时只能由主机（座位-1）推进。");
        if (!IsWushuangClockRunning)
            return Reject(CommandErrorCode.IllegalAction, "无双计时仅在选将完成后的未结束对局中推进。");
        if (command.ElapsedMilliseconds is <= 0 or > WushuangMode.TimeLimitMilliseconds)
            return Reject(CommandErrorCode.IllegalAction, "计时增量必须为正数且不超过15分钟。");
        return Accept(() =>
        {
            var elapsed = Math.Min(command.ElapsedMilliseconds, WushuangRemainingMilliseconds!.Value);
            _wushuangElapsedMilliseconds += elapsed;
            AdvanceEventRulesAndQueueFact(new WushuangClockAdvancedEvent(elapsed, _wushuangElapsedMilliseconds));
            if (_wushuangElapsedMilliseconds == WushuangMode.TimeLimitMilliseconds) ExpireWushuangMatch();
            AdvanceRulesAndPublishState();
        });
    }

    private void ExpireWushuangMatch() => CompleteWushuangAtSurvivalLimit("15分钟时限已到", clockExpired: true);

    private void CompleteWushuangAtSurvivalLimit(string reason, bool clockExpired = false)
    {
        // Compare current HP only. Equal HP remains a low-value draw, regardless
        // of maximum HP, hand size or shop resources.
        _winnerSeat = null;
        _winnerTeamId = null;
        _winner = Winner.Draw;
        var survivors = _players.Where(player => player.IsAlive).OrderByDescending(player => player.Hp).ToArray();
        if (survivors.Length > 0 && (survivors.Length == 1 || survivors[0].Hp > survivors[1].Hp))
        {
            _winnerTeamId = survivors[0].TeamId!;
            _winner = GetWinnerForTeam(_winnerTeamId);
        }

        var revealedJudgments = _resolutionStack.OfType<JudgmentFrame>().Where(f => f.CardId is not null)
            .Select(f => (CardId: f.CardId!.Value, f.TargetSeat)).Distinct().ToArray();
        _pendingDecision = null;
        while (_resolutionStack.LastOrDefault() is { } frame)
        {
            _resolutionStack.CompleteTop(frame.Id, frame.Kind);
            AdvanceEventRulesAndQueueFact(new WushuangResolutionCancelledEvent(frame.Id, frame.Kind));
        }
        _pendingCardsMovedBatches.Clear();
        _pendingHpChanges.Clear();
        _pendingCharacterStateChanges.Clear();
        CancelScheduledProgramPhase();
        ClearActualDiscardRecoveryPhase();
        MoveAllCards(CardLocation.Processing, CardLocation.DiscardPile, new("mode.wushuang.timeout"));
        foreach (var judgment in revealedJudgments)
            if (_cardZones.GetLocation(judgment.CardId) == CardLocation.Judgment(judgment.TargetSeat))
                MoveCard(GetAttackCard(judgment.CardId), CardLocation.Judgment(judgment.TargetSeat),
                    CardLocation.DiscardPile, new("mode.wushuang.timeout-judgment"));
        _pendingCardsMovedBatches.Clear();
        var outcome = _winner == Winner.Draw ? "双方当前体力相同，和局。" : "当前体力较高的一方获胜。";
        AddLog("WushuangSurvivalLimit", reason + "，" + outcome);
        if (clockExpired)
            AdvanceEventRulesAndQueueFact(new WushuangClockExpiredEvent(_wushuangElapsedMilliseconds, _winnerSeat,
                reason + "，" + outcome));
        AdvanceEventRulesAndQueueFact(new WinnerDeterminedEvent(_winner, _winnerTeamId, null) { WinnerSeat = _winnerSeat });
        CompleteGame();
    }
}
