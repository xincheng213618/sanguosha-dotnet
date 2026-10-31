using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record HappyQiaoCardMaximumHealthReceipt(long UseId, long? ActionId,
    CardDamageMaximumHpReplacementReceipt Replacement, long DaQiaoChangeId, long XiaoQiaoChangeId);
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HappyQiaoCardMaximumHealthReceipt? HappyQiaoCardMaximumHealth { get; init; }
}
public sealed record HappyQiaoCardMaximumHealthStartedEvent(HappyQiaoCardMaximumHealthReceipt Receipt) : IGameEvent;
public sealed record HappyQiaoCardMaximumHealthReturnedEvent(HappyQiaoCardMaximumHealthReceipt Receipt, long? LastChangeId) : IGameEvent;
