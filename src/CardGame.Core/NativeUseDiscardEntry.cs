using System.Text.Json.Serialization;

namespace CardGame.Core;

internal readonly record struct NativeUseDiscardEntry(int CardId, int Sequence);

public sealed partial record CardUseFrame
{
    [JsonInclude, JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal IReadOnlyList<NativeUseDiscardEntry>? NativeUseDiscardEntries { get; init; }
}
