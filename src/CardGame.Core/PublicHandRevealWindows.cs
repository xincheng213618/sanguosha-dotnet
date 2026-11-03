using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record PublicHandRevealClaim(long RevealFrameId, CardConversionSource Source,
    string GameplayHash, int Round, Suit Suit, int CardId, bool RetainOwnerDeath = false);
public enum PublicHandRevealReturnKind { Program, FireAttack, NativeFrame, Registered }
public sealed record PublicHandRevealWindowFrame(long Id, long ParentFrameId,
    PublicHandRevealReturnKind ReturnKind, int ClaimIndex = 0,
    ResolutionFrameStep Step = ResolutionFrameStep.ResolvingEffect)
    : ResolutionFrame(Id, ResolutionFrameKind.PublicHandRevealWindow, Step), IProgramChildResolutionFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? ReturnKey { get; init; }
    private readonly IReadOnlyList<PublicHandRevealClaim> _claims = Array.AsReadOnly(Array.Empty<PublicHandRevealClaim>());
    public IReadOnlyList<PublicHandRevealClaim> Claims
    { get => _claims; init => _claims = Array.AsReadOnly(value.ToArray()); }
}

public abstract partial record ResolutionFrame
{
    private readonly IReadOnlyList<PublicHandRevealClaim>? _publicHandRevealClaims;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<PublicHandRevealClaim>? PublicHandRevealClaims
    { get => _publicHandRevealClaims; init => _publicHandRevealClaims = value is null ? null : Array.AsReadOnly(value.ToArray()); }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int PublicHandRevealConsumedCount { get; init; }
}

public sealed partial record ProgramSkillWindowContext
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public PublicHandRevealClaim? PublicHandReveal { get; init; }
}

public sealed record PublicHandRevealWindowStartedEvent(long WindowId, long ParentFrameId,
    PublicHandRevealReturnKind ReturnKind, int Count, string? ReturnKey = null) : IGameEvent;
public sealed record PublicHandRevealClaimReturnedEvent(long WindowId, int Index, PublicHandRevealClaim Claim) : IGameEvent;
public sealed record PublicHandRevealWindowReturnedEvent(long WindowId, long ParentFrameId) : IGameEvent;
