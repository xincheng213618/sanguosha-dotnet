namespace CardGame.Core;

/// <summary>A frozen view of one real action at its recorded resolution stage.</summary>
public sealed record ActualCardUseHistoryEntry(CardActionContext Action, int ActualTurn, int TurnOwner,
    TurnPhase Phase, int PlayPhaseInstanceId, bool IsSelfNativeResponseUse,
    long? NativeUseId = null, IReadOnlyList<int>? PrimaryTargets = null, bool Completed = false)
{
    private readonly IReadOnlyList<int>? _primaryTargets = PrimaryTargets is null ? null : Array.AsReadOnly(PrimaryTargets.ToArray());
    public IReadOnlyList<int>? PrimaryTargets
    {
        get => _primaryTargets;
        init => _primaryTargets = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
    public bool Finalized => PrimaryTargets is not null;
}

internal interface IActualCardUseHistoryQueries : IProgramSelectionQuery
{
    ActualCardUseHistoryEntry? FindAction(long actionId);
    ActualCardUseHistoryEntry? FindFinalizedUse(long nativeUseId);
    ActualCardUseHistoryEntry? FindCompletedUse(long nativeUseId);
    ActualCardUseHistoryEntry? PreviousFinalizedUse(long nativeUseId);
    IReadOnlyList<ActualCardUseHistoryEntry> AcceptedPairFor(long actionId);
    IReadOnlyList<ActualCardUseHistoryEntry> AcceptedPairAtCompletionFor(long actionId);
}
