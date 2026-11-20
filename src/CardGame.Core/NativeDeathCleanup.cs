using System.Text.Json.Serialization;

namespace CardGame.Core;

internal sealed record PendingNativeDeathCleanup(long? ParentBatchId, IReadOnlyList<CardMovementRecord> Movements);

public sealed partial record DeathFrame
{
    [JsonInclude]
    internal int HpAtDeath { get; init; }

    [JsonInclude, JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    internal bool WillRest { get; init; }

    [JsonIgnore]
    internal PendingNativeDeathCleanup? PendingNativeCleanup { get; init; }

    [JsonInclude, JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal IReadOnlyList<CardMovementRecord>? NativeCleanupDiscardEntries { get; init; }
}

public sealed partial record PlayerSnapshot
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsResting { get; init; }
}
