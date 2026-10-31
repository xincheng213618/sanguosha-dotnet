namespace CardGame.Core;

public sealed partial record ProgramSkillFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public PanShuZhirenFieldState? PanShuZhirenField { get; init; }
}

// The two 织纴 field-discarding choices ride the owning program frame: stage one
// asks for one field equipment, stage two for one field delayed trick; each
// stage may be skipped ("至多").
public sealed record PanShuZhirenFieldState(bool AwaitingDelayedTrick, int NameLength);

public sealed partial class GameEngine
{
}
