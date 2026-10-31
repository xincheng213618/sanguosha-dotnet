using System.Text.Json.Serialization;
namespace CardGame.Core;

// Trusted movement-ledger metadata, not a public per-loss marker/event or pending sidecar.
public sealed record ActualPlayPhysicalSlashLossStamp(int ActualTurnNumber, int ActualTurnOwnerSeat,
    int PhaseActorSeat, int PhaseInstanceId, long? ProducerFrameId);
public sealed partial record CardMovementRecord
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ActualPlayPhysicalSlashLossStamp? ActualPlaySlashLoss { get; init; }
}
