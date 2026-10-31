using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum GameStartingNativeMovementKind { OpeningHandGift }

/// <summary>An issued native startup movement. It is not a draw invocation.</summary>
public sealed record GameStartingNativeMovementOrigin(long WindowFrameId, long ProgramFrameId, int InstructionIndex,
    string GameplayHash, CardConversionSource Source, SkillProgramEffectOp Operation, long BatchId, int RecipientSeat,
    long SequenceBefore, long SequenceAfter)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] public GameStartingNativeMovementKind Kind { get; init; }
    public int ActualTurnOwnerSeat { get; init; } = -1;
    private IReadOnlyList<CardMovementRecord> _materials = Array.AsReadOnly(Array.Empty<CardMovementRecord>());
    public IReadOnlyList<CardMovementRecord> Materials
    { get => _materials; init => _materials = Array.AsReadOnly(value.ToArray()); }
}
public sealed record GameStartingMovementDrawOrigin(GameStartingNativeMovementOrigin MovementRoot, GameStartingOutsideDrawOrigin DrawChild);
public sealed record GameStartingNativeMovementRecordedEvent(GameStartingNativeMovementOrigin Origin) : IGameEvent;
public sealed partial record CardMovementBatchContext
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GameStartingNativeMovementOrigin? GameStartingMovementOrigin { get; init; }
}
