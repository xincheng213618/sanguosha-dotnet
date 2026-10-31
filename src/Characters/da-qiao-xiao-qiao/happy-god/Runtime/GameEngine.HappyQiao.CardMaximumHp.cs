using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record HappyQiaoCardMaximumProof(HappyQiaoDualHealth Before, HappyQiaoDualHealth After);

public sealed partial record CardDamageMaximumHpReplacementReceipt
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HappyQiaoCardMaximumProof? HappyQiaoMaximumProof { get; init; }
}
