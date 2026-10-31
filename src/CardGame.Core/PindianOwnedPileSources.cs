using System.Text.Json.Serialization;

namespace CardGame.Core;

/// <summary>An issued original pile entity, never a temporary Hand substitute.</summary>
public sealed record PindianOwnedPileSource(long OriginProgramFrameId,
    CardConversionSource Origin, PublicPersistentPileSource Pile, int CardId,
    int PrintedRank, long SequenceBefore);

public sealed partial record PindianFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PindianOwnedPileSource? OwnedPileSource { get; init; }
}
