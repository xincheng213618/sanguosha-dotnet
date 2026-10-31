namespace CardGame.Core;

// 同协 arm evidence: one public record per accepted arm. The member seats are
// public information (the skill text announces the 同协 roles), so the event
// carries them; membership expiry derives from the owner's own TurnStartedEvent
// facts already in history, so cold recovery rebuilds the same active arm.
public sealed record ProgramTongxieArmedEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int TurnNumber, IReadOnlyList<int> MemberSeats) : IGameEvent;

// 同协 follow-up evidence: one record per completed chain window. Used and
// declined responder seats and the consumed physical Slashes are public.
// Chain exclusion uses the exact issued-action facts rather than these IDs.
public sealed record ProgramTongxieFollowUpResolvedEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int ActorSeat, int TargetSeat, IReadOnlyList<int> UsedBy, IReadOnlyList<int> DeclinedBy,
    IReadOnlyList<int> UsedCardIds) : IGameEvent;

// 同协 guard evidence: one record per accepted prevention; the prevented amount
// mirrors the frozen before-damage window.
public sealed record ProgramTongxieGuardedEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, int ProtectedSeat, int PreventerSeat, int PreventedAmount) : IGameEvent;

// Captured while the exact native use is still live, before its movement
// observers run. A reused entity or a later Response cannot borrow this use.
public sealed record TongxieSettledSlashUseEvent(long CardUseFrameId, long ActionId, int CardId,
    int ActorSeat, int TargetSeat, CardKind EffectiveKind, long BatchId, int MovementSequence,
    long? ParentProgramFrameId) : IGameEvent;

public sealed partial record ProgramSkillFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public TongxieFollowUpState? TongxieFollowUp { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public TongxieGuardState? TongxieGuard { get; init; }
}

// One responder at a time: the pending list holds the remaining 同协 members in
// seat order after the slash user; used/declined evidence and the consumed
// slash card ids accumulate on the owning frame across slash children. The
// trigger action id dedupes the twin member/own trigger bindings on one batch.
public sealed record TongxieFollowUpState(int ActorSeat, int TargetSeat, int CardId,
    IReadOnlyList<int> PendingResponders)
{
    private readonly IReadOnlyList<int> _pendingResponders = Array.AsReadOnly(PendingResponders.ToArray());
    private readonly IReadOnlyList<int> _usedBy = Array.AsReadOnly(Array.Empty<int>());
    private readonly IReadOnlyList<int> _declinedBy = Array.AsReadOnly(Array.Empty<int>());
    private readonly IReadOnlyList<int> _usedCardIds = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> PendingResponders
    { get => _pendingResponders; init => _pendingResponders = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<int> UsedBy
    { get => _usedBy; init => _usedBy = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<int> DeclinedBy
    { get => _declinedBy; init => _declinedBy = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<int> UsedCardIds
    { get => _usedCardIds; init => _usedCardIds = Array.AsReadOnly(value.ToArray()); }
    public long TriggerActionId { get; init; }
}

// One prevention candidate at a time; a nonzero accepted preventer marks the
// resume after that member's losing-hp dying child.
public sealed record TongxieGuardState(int ProtectedSeat, int PreventedAmount,
    IReadOnlyList<int> PendingPreventers, int? AcceptedPreventer = null)
{
    private readonly IReadOnlyList<int> _pendingPreventers = Array.AsReadOnly(PendingPreventers.ToArray());
    public IReadOnlyList<int> PendingPreventers
    { get => _pendingPreventers; init => _pendingPreventers = Array.AsReadOnly(value.ToArray()); }
}

// One completed single-target Slash action recovered from the exact settled
// movement fact, so replay rebuilds the identical actor, target and chain.
public sealed record TongxieChainUse(int ActorSeat, int TargetSeat, int CardId, long ActionId);
