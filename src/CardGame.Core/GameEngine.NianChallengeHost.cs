namespace CardGame.Core;

public sealed partial class GameEngine
{
    private INianChallengeHost CreateNianChallengeHost() => new NianChallengeHost(this);
    private INianChallengeFeature? NianFeature => _modeRuntime.Find<INianChallengeFeature>();

    private sealed class NianChallengeHost(GameEngine engine) : INianChallengeHost
    {
        private readonly IModeEncounterHost _encounter = new ModeEncounterHost(engine);
        public IModeEncounterHost Encounter => _encounter;
        public bool Started => engine._started;
        public IReadOnlyList<NianParticipantFacts> Participants => Array.AsReadOnly(engine._players.Select(player =>
            new NianParticipantFacts(player.Seat, player.IsHuman, player.GeneralSelected, player.HasHpValue)).ToArray());
        public CommandResult Accept(Action action) => engine.Accept(action);
        public CommandResult Reject(CommandErrorCode code, string message) => engine.Reject(code, message);
        public IReadOnlyList<ModeRevealedJudgmentCard> CaptureRevealedJudgments() => Array.AsReadOnly(engine._resolutionStack
            .OfType<JudgmentFrame>().Where(frame => frame.CardId is not null)
            .Select(frame => new ModeRevealedJudgmentCard(frame.CardId!.Value, frame.TargetSeat)).Distinct().ToArray());
        public void CancelDeferredPhases()
        { engine.CancelScheduledProgramPhase(); engine.ClearActualDiscardRecoveryPhase(); }
        public void SetWinnerForCleanup(string teamId)
        { engine._winnerTeamId = teamId; engine._winner = engine.GetWinnerForTeam(teamId); }
        public void RetireRevealedJudgment(ModeRevealedJudgmentCard judgment, string reason)
        {
            if (engine._cardZones.GetLocation(judgment.CardId) == CardLocation.Judgment(judgment.TargetSeat))
                engine.MoveCard(engine.GetAttackCard(judgment.CardId), CardLocation.Judgment(judgment.TargetSeat),
                    CardLocation.DiscardPile, new CardMoveReason(reason));
        }
        public void ClearPendingCardMovements() => engine._pendingCardsMovedBatches.Clear();
        public void PublishCurrentWinner() => engine.AdvanceEventRulesAndQueueFact(
            new WinnerDeterminedEvent(engine._winner, engine._winnerTeamId, null));
        public void CompleteGame() => engine.CompleteGame();
        public bool TryExecuteProgramAction(int seat, string skillId, string activationId, Func<int> selectTarget)
        {
            var action = engine.BuildLegalActions(engine._players[seat]).SingleOrDefault(action =>
                action.Kind == LegalActionKind.UseProgramSkill && action.ProgramSkillId == skillId && action.ProgramActivationId == activationId);
            if (action is null) return false;
            engine.ExecuteProgramSkill(engine._players[seat], action, [], [selectTarget()]);
            return true;
        }
    }
}
