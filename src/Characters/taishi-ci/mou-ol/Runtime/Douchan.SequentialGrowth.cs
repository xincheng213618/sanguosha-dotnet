using System.Text.Json.Serialization;

namespace CardGame.Core;

// An optional policy on the existing sequential physical-acquisition primitive.
// Missing metadata retains the original maximum-HP, uncapped activity behavior.
public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CapGrowthByInitialPlayerCount { get; internal init; }
}
