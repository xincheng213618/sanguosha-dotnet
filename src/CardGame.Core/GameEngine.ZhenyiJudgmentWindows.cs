using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed partial record JudgmentFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ZhenyiBeforeResultCompleted { get; init; }
}
public sealed partial record ProgramJudgmentTriggerWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ZhenyiBeforeResult { get; init; }
}
