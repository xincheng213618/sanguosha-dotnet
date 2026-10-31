using System.Text.Json.Serialization;

namespace CardGame.Core;

/// <summary>One completed rule instruction to discard its primary materials, independent of movement batching.</summary>
public sealed record NativeLogicalDiscardInvocationRecordedEvent(long InvocationId, int TurnNumber,
    int ActualTurnOwnerSeat, int InitiatorSeat, int ExecutorSeat, int? SubjectSeat,
    int ActualCount, int HandCount, int SequenceBefore, int SequenceAfter,
    long? ParentFrameId, CardMoveReason Reason) : IGameEvent
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ActualDiscardRecoveryPhaseKey? Phase { get; init; }
}

public sealed record NativeLogicalDiscardMaterial(int CardId, CardLocation From);
public sealed record NativeLogicalDiscardMaterialCompletedEvent(long InvocationId, int CardId,
    CardLocation From, CardLocation To, long CompletionSequence) : IGameEvent;

public sealed record NativeLogicalDiscardInvocationDraft
{
    public long InvocationId { get; init; }
    public int TurnNumber { get; init; }
    public int ActualTurnOwnerSeat { get; init; }
    public int InitiatorSeat { get; init; }
    public int ExecutorSeat { get; init; }
    public int SequenceBefore { get; init; }
    public int InstructionIndex { get; init; }
    public long? ParentFrameId { get; init; }
    public CardMoveReason Reason { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ActualDiscardRecoveryPhaseKey? Phase { get; init; }
    private IReadOnlyList<NativeLogicalDiscardMaterial> _materials = Array.AsReadOnly(Array.Empty<NativeLogicalDiscardMaterial>());
    public IReadOnlyList<NativeLogicalDiscardMaterial> Materials
    { get => _materials; init => _materials = Array.AsReadOnly(value.ToArray()); }
}

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeLogicalDiscardInvocationDraft? LogicalDiscardInvocation { get; init; }
}
