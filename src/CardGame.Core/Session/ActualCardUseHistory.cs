namespace CardGame.Core;

/// <summary>A frozen view of one real action at its recorded resolution stage.</summary>
public sealed record ActualCardUseHistoryEntry(CardActionContext Action, int ActualTurn, int TurnOwner,
    TurnPhase Phase, int PlayPhaseInstanceId, bool IsSelfNativeResponseUse,
    long? NativeUseId = null, IReadOnlyList<int>? PrimaryTargets = null, bool Completed = false)
{
    // Replacing a native role does not issue another accepted action.
    public int AcceptedActorSeat { get; } = Action.ActorSeat;
    private readonly IReadOnlyList<int>? _primaryTargets = PrimaryTargets is null ? null : Array.AsReadOnly(PrimaryTargets.ToArray());
    public IReadOnlyList<int>? PrimaryTargets
    {
        get => _primaryTargets;
        init => _primaryTargets = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
    public bool Finalized => PrimaryTargets is not null;
    public CardUseHistoryPoint? HistoryPoint { get; init; }
    public CardUseHistoryPoint? CompletionPoint { get; init; }
}

/// <summary>A position in the full committed and pending journal, not an event sequence number.</summary>
public sealed record CardUseHistoryPoint(int ActualTurn, int TurnOwnerSeat, long Cursor,
    long? CurrentNativeUseId = null, long? CurrentActionId = null);

internal enum CardUseHistoryView { Latest, Observations, Confirmations, Accepted, Finalizations }

/// <summary>Rule-neutral facts. Accepted actor and current actor are separate roles.</summary>
public sealed record CardUseHistoryFact(long? NativeUseId, long? ActionId, int ActorSeat,
    int DeclarerSeat, CardActionType Type, bool IsSelfNativeResponseUse,
    IReadOnlyList<int> Targets, bool Established, long DeclarationCursor)
{
    private readonly IReadOnlyList<int> _targets = Array.AsReadOnly(Targets.ToArray());
    public IReadOnlyList<int> Targets
    { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
    public int? AcceptedActualTurn { get; init; }
    public int? AcceptedTurnOwnerSeat { get; init; }
    public TurnPhase? AcceptedPhase { get; init; }
    public int? AcceptedPlayPhaseInstanceId { get; init; }
    public long? AcceptedCursor { get; init; }
    public bool Finalized { get; init; }
    public bool Completed { get; init; }
    public CardKind? DeclaredKind { get; init; }
    public CardKind? EffectiveKind { get; init; }
    public TurnPhase? Phase { get; init; }
    public long? ConfirmationCursor { get; init; }
    private readonly IReadOnlyList<int>? _confirmedTargets;
    public IReadOnlyList<int>? ConfirmedTargets
    { get => _confirmedTargets; init => _confirmedTargets = value is null ? null : Array.AsReadOnly(value.ToArray()); }
    private readonly IReadOnlyList<int>? _primaryTargets;
    public IReadOnlyList<int>? PrimaryTargets
    { get => _primaryTargets; init => _primaryTargets = value is null ? null : Array.AsReadOnly(value.ToArray()); }
}

public sealed partial record ProgramSkillWindowContext
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public CardUseHistoryPoint? HistoryPoint { get; init; }
}

internal interface IActualCardUseHistoryQueries : IProgramSelectionQuery
{
    NativeCardUseHistorySnapshot? FindNativeUse(long nativeUseId);
    long CardUseHistoryCursor { get; }
    IReadOnlyList<CardUseHistoryFact> ReadCardUseFacts(CardUseHistoryPoint point, CardUseHistoryView view);
    ActualCardUseHistoryEntry? FindAction(long actionId);
    CardActionContext? FindLatestAction(long actionId);
    ActualCardUseHistoryEntry? FindFinalizedUse(long nativeUseId);
    ActualCardUseHistoryEntry? FindCompletedUse(long nativeUseId);
}

/// <summary>A declared native use need not have an accepted Action.</summary>
public sealed record NativeCardUseHistorySnapshot(long NativeUseId, int ActualTurn, int TurnOwnerSeat,
    int ActorSeat, CardKind EffectiveKind, long? ActionId, IReadOnlyList<int> DesignatedTargets)
{
    private readonly IReadOnlyList<int> _designatedTargets = Array.AsReadOnly(DesignatedTargets.ToArray());
    public IReadOnlyList<int> DesignatedTargets
    { get => _designatedTargets; init => _designatedTargets = Array.AsReadOnly(value.ToArray()); }
    public CardKind DeclaredKind { get; init; } = EffectiveKind;
}

public sealed record CardUseConfirmation(IReadOnlyList<int> Targets, TurnPhase Phase, int PlayPhaseInstanceId)
{
    private readonly IReadOnlyList<int> _targets = Array.AsReadOnly(Targets.ToArray());
    public IReadOnlyList<int> Targets
    { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
}

public sealed record NativeCardUseHistoryRecordedEvent(NativeCardUseHistorySnapshot Snapshot,
    int? ActualTurn = null, int? TurnOwnerSeat = null, CardUseConfirmation? Confirmation = null) : IGameEvent;
