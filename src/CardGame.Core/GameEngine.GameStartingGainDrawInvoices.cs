using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum GameStartingDrawChildKind { OutsidePhaseDraw, ProgramInstruction, ProgramOperation, NativeGainBenefit, NativeGainColorBenefit }

/// <summary>A paid gain observer derives one request from an earlier real startup draw.</summary>
public sealed record GameStartingOutsideDrawOrigin(long ProgramFrameId, int InstructionIndex, string GameplayHash,
    CardConversionSource Source, long WindowFrameId, long OriginalBatchId, long ParentInvocationId,
    int ParticipantSeat, long SequenceBefore)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] public GameStartingDrawChildKind Kind { get; init; }
    public int RequestedCount { get; init; } = 1;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public SkillProgramEffectOp? Operation { get; init; }
}
public sealed record GameStartingDrawChildRequestedEvent(long InvocationId, GameStartingOutsideDrawOrigin Child) : IGameEvent;
