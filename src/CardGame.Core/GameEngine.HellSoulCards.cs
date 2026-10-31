using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record HellSoulProgramReturn(long CardUseFrameId, long ActionId, int TargetIndex, int TargetSeat);
public sealed record HellSoulResolution(long ProgramFrameId, long ActionId, int TargetIndex, int TargetSeat);
public sealed record HellSoulRestoreReceipt(int InstructionIndex, int TargetSeat, int RestoredGrants);
public sealed record HellSoulRestoreIssuedEvent(long ProgramFrameId, long CardUseFrameId, int TargetSeat, int RestoredGrants) : IGameEvent;
public sealed record HellSoulGrantRestoredEvent(long ProgramFrameId, long CardUseFrameId, long LossFrameId,
    int TargetSeat, string GrantId, string Outcome) : IGameEvent;
public sealed record HellSoulTargetResolvedEvent(long CardUseFrameId, int TargetSeat, int RestoredGrants, bool Completed) : IGameEvent;
public sealed record HellReturnSoulDestroyedEvent(int CardId, int DiscardSequence, int DestroySequence) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HellSoulProgramReturn? HellSoulReturn { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HellSoulRestoreReceipt? HellSoulRestore { get; init; }
}
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HellSoulResolution? HellSoul { get; init; }
}
