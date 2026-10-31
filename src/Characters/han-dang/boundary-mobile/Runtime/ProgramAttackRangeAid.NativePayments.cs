using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ProgramAttackRangeAidPaymentStage { Drawing, MovementChildren }
public sealed record ProgramAttackRangeAidResponderSkippedEvent(long FrameId, int ResponderIndex, int ResponderSeat) : IGameEvent;
public sealed record ProgramAttackRangeAidPayment(int ResponderIndex, int ResponderSeat, ProgramAttackRangeAidPaymentStage Stage,
    long Before, long After, int? CardId = null, CardKind? PrintedKind = null, CardLocation? From = null, NativeDrawOutcome? Draw = null);
public sealed partial record ProgramAttackRangeAid
{
    private readonly IReadOnlyList<int> _responderSeats = Array.AsReadOnly(ResponderSeats.ToArray());
    public IReadOnlyList<int> ResponderSeats { get => _responderSeats; init => _responderSeats = Array.AsReadOnly(value.ToArray()); }
    public int InstructionIndex { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public ProgramAttackRangeAidPayment? Payment { get; init; }
}
