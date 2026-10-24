namespace CardGame.Core;

/// <summary>Native parent/candidate identity proof for an arrival program's virtual ordinary trick.</summary>
public sealed record ModeNpcArrivalSourceFacts(string SkillId, bool IsInitialGameStarting,
    long? AppearanceId, IReadOnlyList<int> CandidateOwnerSeats)
{
    public IReadOnlyList<int> CandidateOwnerSeats { get; } = Array.AsReadOnly(CandidateOwnerSeats.ToArray());
}

/// <summary>Native arrival-window and dormant-seat operations, without a second encounter state store.</summary>
public interface IHellEncounterHost
{
    IModeEncounterHost Encounter { get; }
    bool GameStartingProgramsResolved { get; }
    int ActualTurnOwnerSeat { get; }
    int InitialHandSize { get; }
    long NextResolutionId();
    bool TryOpenNpcGameStartingWindow(HellNpcAppearedEvent appearance);
    void RestoreDormantCombatant(int seat, int maximumHp, int hp);
    ModeNpcArrivalSourceFacts? ArrivalSource(long resolutionId);
}

public interface IHellEncounterFeature : IGameModeFeature
{
    bool IsNpcTeam(string? teamId);
    bool AllowsTierTrigger(string skillId, string bindingId, string? teamId);
    bool IsPendingAppearance(long appearanceId, IReadOnlyList<int> candidateOwnerSeats);
}
