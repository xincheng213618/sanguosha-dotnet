using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record HappyQiaoDamageHealthReceipt(long DamageFrameId, long AttackFrameId, long ChangeId,
    int SourceSeat, int TargetSeat, int Amount, DamageNature Nature, bool SourceLess,
    int HpBefore, int HpAfter, long? UseId, long? ActionId, HappyQiaoPool? ChangedPool,
    CardConversionSource? ShutuSource, bool RedUseQualified);
public sealed partial record DamageFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HappyQiaoDamageHealthReceipt? HappyQiaoDamageHealth { get; init; }
}
public sealed record HappyQiaoDamageHealthStartedEvent(HappyQiaoDamageHealthReceipt Receipt) : IGameEvent;
public sealed record HappyQiaoDamageHealthReturnedEvent(HappyQiaoDamageHealthReceipt Receipt) : IGameEvent;
