using System.Text.Json.Serialization;

namespace CardGame.Core;

/// <summary>The payment of one real damage segment, before any damage children run.</summary>
public sealed record DamageArmorPaymentReceipt(long DamageFrameId, long AttackFrameId,
    int SourceSeat, int TargetSeat, int DamageAmount, DamageNature Nature, bool SourceLess,
    int ArmorBefore, int ArmorAbsorbed, int ArmorAfter, int HpBefore, int HpLossAmount,
    int UnclampedHpAfter, int HpAfter)
{
    public int ActualHpLost => Math.Max(0, HpBefore - HpAfter);
    public int ArmorDamageMultiplier { get; init; } = 1;
}

public sealed partial record DamageFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DamageArmorPaymentReceipt? ArmorPayment { get; init; }
}
