using System.Text.Json.Serialization;

namespace CardGame.Core;

// Materials are trusted journal data; the public event retains its scalar API.
public sealed record NativeGiveFact(long Id, long ProducerId, int Giver, int Recipient,
    int Turn, int TurnOwner, long CompletionSequence)
{
    private readonly IReadOnlyList<CardMovementRecord> _movements = Array.Empty<CardMovementRecord>();
    public IReadOnlyList<CardMovementRecord> Movements
    { get => _movements; init => _movements = Array.AsReadOnly(value.ToArray()); }
    internal long? ScopeCutoffAtIssue { get; init; }
}

public sealed record NativeCardsGivenEvent(long Id, long ProducerId, int Giver, int Recipient,
    int Count, int Turn, int TurnOwner, long CompletionSequence) : IGameEvent
{
    [JsonIgnore] internal NativeGiveFact? Receipt { get; init; }
    public bool Equals(NativeCardsGivenEvent? other) => other is not null && Id == other.Id &&
        ProducerId == other.ProducerId && Giver == other.Giver && Recipient == other.Recipient &&
        Count == other.Count && Turn == other.Turn && TurnOwner == other.TurnOwner &&
        CompletionSequence == other.CompletionSequence;
    public override int GetHashCode() => HashCode.Combine(Id, ProducerId, Giver, Recipient,
        Count, Turn, TurnOwner, CompletionSequence);
}

public sealed partial record ProgramSkillWindowContext
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeGiveFact? NativeGive { get; init; }
}

internal interface INativeGivenCardOperations : IProgramRuntimeOperations
{
    void Record(long producerId, int giver, int recipient, IReadOnlyList<int> intended, long before,
        HappyWeiYanGainRequestSettlement? requestSettlement = null);
    Action<CardMovementRecord> Completion(long producerId, int giver, int recipient,
        IReadOnlyList<int> intended, long before, Action<CardMovementRecord>? original = null);
    Action<long, IReadOnlyList<CardMovementRecord>> CompletionRecords(long producerId, int giver, int recipient,
        IReadOnlyList<int> intended, long before, Action<long, IReadOnlyList<CardMovementRecord>>? original = null);
    Action<HappyWeiYanGainRequestSettlement> RequestCompletion(long producerId, int giver, int recipient,
        IReadOnlyList<int> intended, long before, Action<HappyWeiYanGainRequestSettlement>? original = null);
}

internal interface INativeGivenCardQueries : IProgramSelectionQuery
{
    IReadOnlyList<NativeGiveFact> Read();
    IReadOnlyList<NativeGiveFact> GiftsForBatch(CardMovementBatchContext batch, int giver);
    IReadOnlyList<NativeGiveFact> GiftsForCurrentScope(int giver, int turn, int turnOwner);
}
