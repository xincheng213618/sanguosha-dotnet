using System.Text.Json.Serialization;

namespace CardGame.Core;

internal sealed record NativeDodgeDiscardEntry(int CardId, int Sequence);

internal sealed record NativeDodgeResponseSettlement(CardActionContext Action, IReadOnlyList<NativeDodgeDiscardEntry> Entries);

public sealed partial record CardUseFrame
{
    [JsonInclude, JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal IReadOnlyList<NativeDodgeResponseSettlement>? NativeDodgeResponses { get; init; }
}
