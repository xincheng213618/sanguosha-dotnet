using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ActualTurnDeathOwnerKillsOnly { get; internal init; }
}
