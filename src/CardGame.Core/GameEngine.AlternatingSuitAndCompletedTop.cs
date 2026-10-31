namespace CardGame.Core;
public sealed record IssuedPlayPhaseSuitUseAllowance(int OwnerSeat, string SkillId, string SkillInstanceId, int TurnNumber, int PhaseInstanceId, IReadOnlyList<Suit> Suits)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public SkillProgramTurnSkillExpiry? Expiry { get; init; }

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public int? ActualTurnOwnerSeat { get; init; }
}
