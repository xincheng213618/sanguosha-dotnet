using System.Text.Json.Serialization;

namespace CardGame.Core;

/// <summary>Private, detached presentation of the ordering frame's accepted prefix.</summary>
public sealed record DeckReorderView(long FrameId, IReadOnlyList<CardSnapshot> Cards,
    IReadOnlyList<int> TopCardIds, IReadOnlyList<int> BottomCardIds, bool ChoosingBottom, int? RequiredTopCount)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool BottomOnly { get; init; }
}

public sealed partial record PendingDecision
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DeckReorderView? DeckReorder { get; init; }
}
