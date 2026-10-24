namespace CardGame.Core;

/// <summary>Trusted immutable life facts for encounter transitions, without cards or mutable characters.</summary>
public sealed record ModeEncounterPlayerFacts(int Seat, string Name, string GeneralId,
    bool IsAlive, int Hp, int MaxHp, string? TeamId);

public sealed record ModeEncounterFacts(bool SetupComplete, bool HasWinner, int PlayerCount,
    int HumanSeat, int CurrentSeat, int TurnNumber, TurnPhase Phase, int ResolutionCount,
    DecisionKind? PendingDecisionKind, IReadOnlyList<ModeEncounterPlayerFacts> Players)
{
    public IReadOnlyList<ModeEncounterPlayerFacts> Players { get; } =
        Array.AsReadOnly((Players ?? throw new ArgumentNullException(nameof(Players))).ToArray());
}

public sealed record ModeEncounterGeneralFacts(string Id, string Name, int BaseHp, int InitialHp);

/// <summary>Replace a seat's general, either preserving its current state or starting a fresh combatant.</summary>
public sealed record ModeCombatantReplacement(int Seat, string ExpectedGeneralId, string GeneralId,
    bool PreserveCurrentState, int? MaximumHpOverride = null, int? HpOverride = null);

public sealed record ModeCancelledResolution(long FrameId, ResolutionFrameKind Kind, int TurnNumber);

/// <summary>
/// Native atomic operations retain the engine's command, frame, card and skill-state owners.
/// Mode services decide when and how to compose them; no pending state is stored here.
/// </summary>
public interface IModeEncounterHost
{
    ModeEncounterFacts CaptureFacts();
    IReadOnlyList<TEvent> History<TEvent>() where TEvent : IGameEvent;
    ModeEncounterGeneralFacts General(string generalId);
    int NextRandom(int exclusiveMaximum);
    void ReplaceCombatant(ModeCombatantReplacement request);
    void ClearDecision(bool resumeRunning = false);
    void CancelResolutionTree(Func<ModeCancelledResolution, IGameEvent> cancellationFact);
    void ClearTransientBatches();
    void DiscardProcessingCards(string reason);
    void FinishInterruptedTurn(int expectedSeat, int expectedTurnNumber,
        string skillGrantEndReason, IGameEvent interruptionFact);
    void BeginNaturalTurnAt(int seat);
    void EndCurrentTurn();
    void DrawInitialCards(int seat, int count);
    void DeclareTeamWinner(string teamId);
    void PublishFact(IGameEvent fact);
    void PublishState();
    void Log(string kind, string message, int seat);
}
