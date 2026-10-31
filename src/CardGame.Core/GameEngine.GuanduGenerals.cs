using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record GuanduGeneralIssuedEvent(long FrameId, string SkillId, SkillProgramEffectOp Operation,
    int OwnerSeat, int ActualTurn, long? WindowId) : IGameEvent;
public sealed record GuanduGeneralCompletedEvent(long FrameId, string SkillId, bool Completed) : IGameEvent;
public sealed record GuanduGeneralDrawEvent(long FrameId, int Ordinal, int RecipientSeat, int Requested,
    int Actual, long Before, long After, long? AttemptId, bool Replaced) : IGameEvent;
public sealed record GuanduShicaiObtainedEvent(long FrameId, int OwnerSeat, string InstanceId, int CardId, int MovementSequence) : IGameEvent;
public sealed record GuanduDiscardFactEvent(long BatchId, int ActualTurn, int PhaseActor, long PhaseToken,
    int CardId, int MovementSequence, CardLocation From) : IGameEvent;
public sealed record GuanduJieliangPaidEvent(long FrameId, int OwnerSeat, int TargetSeat, int ActualTurn) : IGameEvent;
public sealed record GuanduHengjiangPaidEvent(long FrameId, int OwnerSeat, string InstanceId, int TargetSeat,
    int ActualTurn, long DamageFrameId, int OccurrenceIndex) : IGameEvent;
public sealed record GuanduBeizhanArmedEvent(long FrameId, int OwnerSeat, int TargetSeat, int CreatedTurn,
    CardUseEffectSource Source) : IGameEvent;
public sealed record GuanduBeizhanConsumedEvent(long ProducerFrameId, int TargetSeat, int ActualTurn, bool Restricted) : IGameEvent;
public sealed record GuanduMoushiGiftedEvent(long FrameId, int OwnerSeat, string InstanceId, int RecipientSeat,
    int CardId, int CreatedTurn) : IGameEvent;
public sealed record GuanduMoushiDrawnEvent(long ProducerFrameId, int OwnerSeat, int ActualTurn, int PhaseInstance,
    int VictimSeat, long DamageFrameId, long ActualPlayToken = 0) : IGameEvent;

public sealed record GuanduGeneralReceipt(SkillProgramEffectOp Operation, int InstructionIndex, int ActualTurn,
    long? WindowId, string Stage, int TargetSeat = -1, int CardId = 0, int Index = 0, int Count = 0,
    int DrawOrdinal = 0, int DrawRecipient = -1, int DrawRequested = 0, string? NextStage = null,
    NativeDrawOutcome? Draw = null, IReadOnlyList<int>? Seats = null, CardKind? FrozenKind = null,
    int MovementBefore = 0, int MovementAfter = 0, bool Used = false);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GuanduGeneralReceipt? GuanduGeneral { get; init; }
}

public sealed partial class GameEngine
{
}
