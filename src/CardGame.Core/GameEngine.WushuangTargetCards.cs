using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record WushuangTargetCardReceipt(long ActionId, int TargetSeat, int Remaining, bool ExtraSelection = false);
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WushuangTargetCardReceipt? WushuangTargetCards { get; init; }
}
