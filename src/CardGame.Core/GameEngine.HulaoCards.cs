using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record HulaoAllianceFeastChoice(int TargetSeat, int TargetIndex, long ActionId);
public sealed record HulaoAllianceFeastBenefitEvent(long CardUseFrameId, int SourceSeat,
    int TargetSeat, string Benefit, int Amount) : IGameEvent;

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HulaoAllianceFeastChoice? AllianceFeastChoice { get; init; }
}
