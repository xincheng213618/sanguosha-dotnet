using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record WenyiDamageHealthReceipt(long DamageFrameId, long AttackFrameId, long HealthChangeId,
    int SourceSeat, int TargetSeat, int Amount, DamageNature Nature, bool SourceLess);
public sealed partial record DamageFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WenyiDamageHealthReceipt? WenyiHealth { get; init; }
}
public sealed record WenyiDamageHealthStartedEvent(WenyiDamageHealthReceipt Receipt) : IGameEvent;
public sealed record WenyiDamageHealthReturnedEvent(WenyiDamageHealthReceipt Receipt) : IGameEvent;
