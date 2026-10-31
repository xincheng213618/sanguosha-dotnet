using System.Text.Json.Serialization;

namespace CardGame.Core;

// This receipt belongs to the real equipment Use. It authorizes only the frozen
// target expansion; the native Slash still owns payment, responses and return.
public sealed record NativeMaterialSlashExtraTargetsOrigin(long CardUseFrameId, long ActionId,
    int ActorSeat, int ProviderSeat, int OrdinaryMaximum, long PaidFirst, long PaidLast = 0)
{
    private readonly IReadOnlyList<int> _physicalCardIds = Array.AsReadOnly(Array.Empty<int>());
    private readonly IReadOnlyList<int> _targetSeats = Array.AsReadOnly(Array.Empty<int>());
    private readonly IReadOnlyList<ProgramSlashTargetExpansion> _claims = Array.AsReadOnly(Array.Empty<ProgramSlashTargetExpansion>());
    public IReadOnlyList<int> PhysicalCardIds
    { get => _physicalCardIds; init => _physicalCardIds = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<int> TargetSeats
    { get => _targetSeats; init => _targetSeats = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<ProgramSlashTargetExpansion> Claims
    { get => _claims; init => _claims = Array.AsReadOnly(value.Select(claim => claim with { TargetSeats = claim.TargetSeats }).ToArray()); }
}

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeMaterialSlashExtraTargetsOrigin? NativeMaterialSlashExtraTargets { get; init; }
}

public sealed record NativeMaterialSlashExtraTargetsIssuedEvent(NativeMaterialSlashExtraTargetsOrigin Origin) : IGameEvent;
