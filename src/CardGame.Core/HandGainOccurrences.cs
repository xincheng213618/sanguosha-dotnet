using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record HandGainCardSnapshot(int CardId, CardKind PrintedKind, CardKind EffectiveKind,
    Suit EffectiveSuit, int GainSequence = 0, CardLocation From = default, bool IsActualGain = true);

// A rules acquisition may span physical batches. Its receipt stays on the
// final real batch; ordinary per-card/per-batch observers retain every batch.
public sealed record HandGainOccurrence(long BatchId, int RecipientSeat, int ActualTurn,
    CardMovementTiming? Timing, bool RecipientAlive, bool FirstOutsideDraw)
{
    private readonly IReadOnlyList<HandGainCardSnapshot> _incoming = [];
    public IReadOnlyList<HandGainCardSnapshot> Incoming
    { get => _incoming; init => _incoming = Array.AsReadOnly(value.ToArray()); }
    private readonly IReadOnlyList<HandGainCardSnapshot> _previousHand = [];
    public IReadOnlyList<HandGainCardSnapshot> PreviousHand
    { get => _previousHand; init => _previousHand = Array.AsReadOnly(value.ToArray()); }
    private readonly IReadOnlyList<ProgramTriggerCandidate> _sources = [];
    public IReadOnlyList<ProgramTriggerCandidate> Sources
    { get => _sources; init => _sources = Array.AsReadOnly(value.ToArray()); }
    private readonly IReadOnlyList<ProgramTriggerCandidate> _initialSources = [];
    public IReadOnlyList<ProgramTriggerCandidate> InitialSources
    { get => _initialSources; init => _initialSources = Array.AsReadOnly(value.ToArray()); }
    public bool InitiallyAlive { get; init; }
    public long? NativeDrawInvocationId { get; init; }
    public long? NativeDrawAttemptId { get; init; }
}

public sealed partial record CardMovementBatchContext
{
    private readonly IReadOnlyList<HandGainOccurrence>? _handGains;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HandGainOccurrence>? HandGains
    { get => _handGains; init => _handGains = value is null ? null : Array.AsReadOnly(value.ToArray()); }
}

public sealed partial record CardMovementRecord
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CardMovementTiming? MovementTiming { get; init; }
}

// Resolve processing transits from the native movement journal, never from a
// character ledger. A return to one's own hand is distinct from an acquisition.
internal static class ActualHandGainFacts
{
    internal static CardLocation OriginalSource(CardMovementRecord first,
        IReadOnlyList<CardMovementRecord> journal, CardMovementBatchContext? batch = null)
    {
        var from = first.From;
        var sequence = first.Sequence;
        while (from == CardLocation.Processing)
        {
            var prior = batch?.Movements.Where(m => m.CardId == first.CardId && m.Sequence < sequence).MaxBy(m => m.Sequence);
            var recorded = journal.LastOrDefault(m => m.CardId == first.CardId && m.Sequence < sequence);
            if (recorded is not null && (prior is null || recorded.Sequence > prior.Sequence)) prior = recorded;
            if (prior?.To != CardLocation.Processing) break;
            from = prior.From;
            sequence = prior.Sequence;
        }
        return from;
    }
}
