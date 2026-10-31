using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ColdDamageOfferedEvent(long FrameId, long AttackId, int SourceSeat,
    int TargetSeat, int Amount) : IGameEvent;
public sealed record ColdDamageChoiceEvent(long FrameId, bool Prevented) : IGameEvent;
public sealed record ColdDamageDiscardedEvent(long FrameId, int Ordinal, int CardId,
    CardLocation From, int MovementSequence) : IGameEvent;
public sealed record ColdDamageCompletedEvent(long FrameId, long AttackId, bool Prevented,
    int PreventedAmount, int DiscardedCount) : IGameEvent;

/// <summary>The direct Cold damage target and the paid sequential discard prefix.</summary>
public sealed record ColdDamageReplacementFrame(long Id, long ParentFrameId, bool CardAttack,
    int SourceSeat, int TargetSeat, int Amount, bool Activated = false,
    bool ChoiceMade = false, ResolutionFrameStep Step = ResolutionFrameStep.ResolvingEffect)
    : ResolutionFrame(Id, ResolutionFrameKind.ColdDamageReplacement, Step), IProgramChildResolutionFrame
{
    private IReadOnlyList<ColdDamageDiscardedEvent> _discards = Array.AsReadOnly(Array.Empty<ColdDamageDiscardedEvent>());
    public IReadOnlyList<ColdDamageDiscardedEvent> Discards
    { get => _discards; init => _discards = Array.AsReadOnly(value.ToArray()); }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeLogicalDiscardInvocationDraft? LogicalDiscard { get; init; }
}
