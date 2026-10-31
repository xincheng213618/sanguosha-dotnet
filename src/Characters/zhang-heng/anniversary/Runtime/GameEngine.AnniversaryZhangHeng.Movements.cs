namespace CardGame.Core;

public sealed record AnniversaryZhangHengMovement(bool SourceBlack, bool DestinationBlack);
public sealed partial record CardMovementRecord
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public AnniversaryZhangHengMovement? AnniversaryZhangHeng { get; init; }
}
