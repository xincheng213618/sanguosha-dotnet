namespace CardGame.Core;

public sealed record NianParticipantFacts(int Seat, bool IsHuman, bool GeneralSelected, bool HasHpValue);
public sealed record ModeRevealedJudgmentCard(int CardId, int TargetSeat);

/// <summary>Trusted scalar facts and native cleanup/actions for the optional Nian rule service.</summary>
public interface INianChallengeHost
{
    IModeEncounterHost Encounter { get; }
    bool Started { get; }
    IReadOnlyList<NianParticipantFacts> Participants { get; }
    CommandResult Accept(Action action);
    CommandResult Reject(CommandErrorCode code, string message);
    IReadOnlyList<ModeRevealedJudgmentCard> CaptureRevealedJudgments();
    void CancelDeferredPhases();
    void SetWinnerForCleanup(string teamId);
    void RetireRevealedJudgment(ModeRevealedJudgmentCard judgment, string reason);
    void ClearPendingCardMovements();
    void PublishCurrentWinner();
    void CompleteGame();
    bool TryExecuteProgramAction(int seat, string skillId, string activationId, Func<int> selectTarget);
}

public interface INianChallengeFeature : IGameModeFeature
{
    bool ClockRunning { get; }
    int RemainingMilliseconds { get; }
    int UsedRevives { get; }
    int SharedRevives { get; }
    bool IsAlliedSeat(int seat);
    IReadOnlyList<int> DeadAllies(int ownerSeat);
}
