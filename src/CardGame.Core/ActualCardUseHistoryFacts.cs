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
}

internal static class ActualCardUseTargets
{
    internal static int[] PrincipalTargets(CardActionContext action)
    {
        if (action.EffectiveKind == CardKind.BorrowedSword)
            return action.TargetSeats.Count == 0 || action.TargetSeats.Count % 2 != 0
                ? [] : action.TargetSeats.Where((_, index) => index % 2 == 0).Distinct().ToArray();
        if (action.EffectiveKind == CardKind.DrawTwo && action.EffectiveDesignatedTargetSeats.Count == 0 &&
            action.TargetSeats.Count == 1 && action.TargetSeats[0] == action.ActorSeat)
            return [action.ActorSeat];
        return action.EffectiveDesignatedTargetSeats.Distinct().ToArray();
    }
}
