namespace CardGame.Core;
public sealed record IssuedPlayPhaseSuitUseAllowance(int OwnerSeat, string SkillId, string SkillInstanceId, int TurnNumber, int PhaseInstanceId, IReadOnlyList<Suit> Suits)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public SkillProgramTurnSkillExpiry? Expiry { get; init; }

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public int? ActualTurnOwnerSeat { get; init; }
}

public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<(int Turn, int Actor, string Skill, global::CardGame.Core.SkillProgramCardCategory Category), long> _firstTurnCategoryUses => ref _alternatingSuitAndCompletedTop.ReferenceToFirstTurnCategoryUses;
}
