namespace CardGame.Core;
public sealed record DirectedTurnCardPolicy(long GrantSequence, int TurnNumber, int TurnSeat, long ParentFrameId, int EffectIndex, CardUseEffectSource Source, int ActorSeat, int TargetSeat, IReadOnlyList<CardKind> CardKinds, DirectedTurnCardPolicyEffect Effects)
{
    public bool RequiresEffectiveSource { get; init; } = true;
    public bool CaptureOnUse { get; init; }
    private readonly IReadOnlyList<SkillStateBoundaryEnd> _until = Array.AsReadOnly(Array.Empty<SkillStateBoundaryEnd>());
    public IReadOnlyList<SkillStateBoundaryEnd> Until
    { get => _until; init => _until = Array.AsReadOnly(value.ToArray()); }
}

public sealed record DirectedCardPolicyOptions
{
    public bool RequiresEffectiveSource { get; init; } = true;
    public bool CaptureOnUse { get; init; }
    private readonly IReadOnlyList<SkillStateBoundarySpec> _until = Array.AsReadOnly(new[]
        { new SkillStateBoundarySpec(SkillStateBoundaryEvent.TurnCleanup, SkillStateSubject.TurnOwner, SkillStateBoundaryTiming.Before) });
    public IReadOnlyList<SkillStateBoundarySpec> Until
    { get => _until; init => _until = Array.AsReadOnly(value.ToArray()); }
}

public sealed partial class SkillProgramEffect
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public DirectedCardPolicyOptions? DirectedPolicyOptions { get; internal init; }
}
public sealed record DirectedTurnCardPolicyGrantedEvent(DirectedTurnCardPolicy Policy) : IGameEvent;
