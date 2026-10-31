using System.Text.Json.Serialization;

namespace CardGame.Core;

// The issuing card retains its own entity and target cursor. The ordinary
// program executor owns the nested request, every payment and native Slash.
public sealed record ShenwuTransplantProgramReturn(long CardUseFrameId, long ActionId,
    int TargetIndex, int ActorSeat);
public sealed record ShenwuTransplantResolution(long ProgramFrameId, long ActionId,
    int TargetIndex, int ActorSeat);
public sealed record ShenwuTransplantResolvedEvent(long CardUseFrameId, int SourceSeat,
    int TargetSeat, bool UsedSlash, bool Completed) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShenwuTransplantProgramReturn? ShenwuTransplantReturn { get; init; }
}
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShenwuTransplantResolution? ShenwuTransplant { get; init; }
}
