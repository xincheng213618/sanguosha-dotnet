using System.Text.Json.Serialization;

namespace CardGame.Core;

internal sealed record PendingNativeDeathCleanup(long? ParentBatchId, IReadOnlyList<CardMovementRecord> Movements);

public sealed partial record DeathFrame
{
    [JsonIgnore]
    internal PendingNativeDeathCleanup? PendingNativeCleanup { get; init; }

    [JsonInclude, JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal IReadOnlyList<CardMovementRecord>? NativeCleanupDiscardEntries { get; init; }
}
