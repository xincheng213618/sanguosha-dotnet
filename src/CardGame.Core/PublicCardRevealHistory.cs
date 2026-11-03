namespace CardGame.Core;

public enum PublicCardRevealPurpose { Explicit, MaterialDisclosure }

/// <summary>The physical Hand witness at the original public show, before its observers settle.</summary>
public sealed record PublicCardRevealHistoryFact(long RevealFrameId, int ActualTurn, int TurnOwnerSeat,
    ActualDiscardRecoveryPhaseKey? ActualPhase, long MovementSequence, int CardId, int OwnerSeat,
    long HandEntryMovementSequence, PublicCardRevealPurpose Purpose);

public sealed record PublicCardRevealHistoryRecordedEvent : IGameEvent
{
    public PublicCardRevealHistoryRecordedEvent(IReadOnlyList<PublicCardRevealHistoryFact> facts) => Facts = facts;
    private readonly IReadOnlyList<PublicCardRevealHistoryFact> _facts = Array.AsReadOnly(Array.Empty<PublicCardRevealHistoryFact>());
    public IReadOnlyList<PublicCardRevealHistoryFact> Facts
    {
        get => _facts;
        init => _facts = Array.AsReadOnly(value.ToArray());
    }
}

internal interface IPublicCardRevealHistoryQueries : IProgramSelectionQuery
{
    IReadOnlyList<PublicCardRevealHistoryFact> Read();
}
