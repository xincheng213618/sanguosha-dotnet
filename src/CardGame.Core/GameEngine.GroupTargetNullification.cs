using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record GroupTargetNullificationReturn(int TargetIndex, int TargetSeat);

public sealed partial record NullificationWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GroupTargetNullificationReturn? GroupTargetReturn { get; init; }
}
