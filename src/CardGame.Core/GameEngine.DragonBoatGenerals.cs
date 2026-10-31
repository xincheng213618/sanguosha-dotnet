using System.Text.Json.Serialization;

namespace CardGame.Core;
public sealed record DragonBoatActualUseEvent(long UseId, long ActionId, int ActualTurn, int PhaseInstance, int ActorSeat, int TargetCount) : IGameEvent;
public sealed record DragonBoatActualUseTargetEvent(long UseId, long ActionId, int Index, int TargetSeat) : IGameEvent;
public sealed record DragonBoatDianhuArmedEvent(long FrameId, int OwnerSeat, string SkillInstanceId, int TargetSeat, long GameStartingWindowId) : IGameEvent;
public sealed record DragonBoatDrawPaidEvent(long FrameId, string SkillId, int OwnerSeat, int RecipientSeat, int RequestedCount, int ActualCount, long SequenceBefore, long SequenceAfter, long? AttemptFrameId, bool Replaced) : IGameEvent;
public sealed record DragonBoatXingzhaoGrantEvent(int SourceSeat, int RecipientSeat, string GrantId, string SkillId, string SkillInstanceId, bool Added) : IGameEvent;
public enum DragonBoatGeneralStage
{
    Choosing,
    DrawAttempt,
    DrawChildren
}

public sealed record DragonBoatGeneralReceipt(int InstructionIndex, long WindowId, int ActualTurn, DragonBoatGeneralStage Stage, int? RecipientSeat = null, int RequestedCount = 0, NativeDrawOutcome? Draw = null);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DragonBoatGeneralReceipt? DragonBoatGeneral { get; init; }
}
