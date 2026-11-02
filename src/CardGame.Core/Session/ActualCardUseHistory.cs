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
}

/// <summary>A position in the full committed and pending journal, not an event sequence number.</summary>
public sealed record CardUseHistoryPoint(int ActualTurn, int TurnOwnerSeat, long Cursor,
    long? CurrentNativeUseId = null, long? CurrentActionId = null);

internal enum CardUseHistoryView { Latest, Observations }

/// <summary>Rule-neutral facts. Accepted actor and current actor are separate roles.</summary>
public sealed record CardUseHistoryFact(long? NativeUseId, long? ActionId, int ActorSeat,
    int DeclarerSeat, CardActionType Type, bool IsSelfNativeResponseUse,
    IReadOnlyList<int> Targets, bool Established, long DeclarationCursor)
{
    private readonly IReadOnlyList<int> _targets = Array.AsReadOnly(Targets.ToArray());
    public IReadOnlyList<int> Targets
    { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
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
    ActualCardUseHistoryEntry? PreviousFinalizedUse(long nativeUseId);
    IReadOnlyList<ActualCardUseHistoryEntry> AcceptedPairAtCompletionFor(long actionId);
}

/// <summary>A declared native use need not have an accepted Action.</summary>
public sealed record NativeCardUseHistorySnapshot(long NativeUseId, int ActualTurn, int TurnOwnerSeat,
    int ActorSeat, CardKind EffectiveKind, long? ActionId, IReadOnlyList<int> DesignatedTargets)
{
    private readonly IReadOnlyList<int> _designatedTargets = Array.AsReadOnly(DesignatedTargets.ToArray());
    public IReadOnlyList<int> DesignatedTargets
    { get => _designatedTargets; init => _designatedTargets = Array.AsReadOnly(value.ToArray()); }
}

public sealed record NativeCardUseHistoryRecordedEvent(NativeCardUseHistorySnapshot Snapshot,
    int? ActualTurn = null, int? TurnOwnerSeat = null) : IGameEvent;
