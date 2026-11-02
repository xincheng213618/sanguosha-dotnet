namespace CardGame.Core;

public sealed record ActualCardActionRecordedEvent(CardActionContext Action, int ActualTurn,
    int TurnOwner, TurnPhase Phase, int PlayPhaseInstanceId, bool IsSelfNativeResponseUse) : IGameEvent;

public sealed record ActualCardUseActionUpdatedEvent(long NativeUseId, CardActionContext Action,
    int ActualTurn, int TurnOwner) : IGameEvent;

public sealed record ActualCardUseTargetsFinalizedEvent(long NativeUseId, CardActionContext Action,
    IReadOnlyList<int> PrimaryTargets, int ActualTurn, int TurnOwner) : IGameEvent
{
    private readonly IReadOnlyList<int> _primaryTargets = Array.AsReadOnly(PrimaryTargets.ToArray());
    public IReadOnlyList<int> PrimaryTargets
    { get => _primaryTargets; init => _primaryTargets = Array.AsReadOnly(value.ToArray()); }
}

public sealed record ActualCardUseTargetsCompletedEvent(long NativeUseId, CardActionContext Action,
    IReadOnlyList<int> PrimaryTargets, int ActualTurn, int TurnOwner) : IGameEvent
{
    private readonly IReadOnlyList<int> _primaryTargets = Array.AsReadOnly(PrimaryTargets.ToArray());
    public IReadOnlyList<int> PrimaryTargets
    { get => _primaryTargets; init => _primaryTargets = Array.AsReadOnly(value.ToArray()); }
}

internal interface IActualCardUseHistoryCapture : IProgramRuntimeOperations
{
    bool IsRequired { get; }
    void RecordFinalized(CardUseFrame use, CardActionContext action);
    void RecordCompleted(CardUseFrame use, CardActionContext action);
    void RecordUpdated(CardUseFrame use, CardActionContext action);
    void RecordNativeChanged(CardUseFrame use);
}

internal static class ActualCardUseTargets
{
    internal static int[] PrincipalTargets(CardActionContext action) => PrincipalTargets(action.EffectiveKind,
        action.ActorSeat, action.TargetSeats, action.EffectiveDesignatedTargetSeats);

    internal static int[] PrincipalTargets(CardKind kind, int actorSeat,
        IReadOnlyList<int> targets, IReadOnlyList<int> designatedTargets)
    {
        if (kind == CardKind.BorrowedSword)
            return targets.Count == 0 || targets.Count % 2 != 0
                ? [] : targets.Where((_, index) => index % 2 == 0).Distinct().ToArray();
        if (kind == CardKind.DrawTwo && designatedTargets.Count == 0 &&
            targets.Count == 1 && targets[0] == actorSeat)
            return [actorSeat];
        return designatedTargets.Distinct().ToArray();
    }
}
