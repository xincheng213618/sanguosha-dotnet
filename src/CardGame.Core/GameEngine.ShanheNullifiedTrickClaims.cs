using System.Text.Json.Serialization;
using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed record ShanheNullifiedTrickClaim(long CardUseFrameId, long ActionId, long WindowFrameId,
    int ChainDepth, int? TargetSeat, int RecipientSeat, int ResponseCardId, string SkillId,
    string SkillInstanceId, string GameplayHash, string PolicyId, IReadOnlyList<int> CardIds,
    int MovementBefore, int MovementAfter = 0, bool MovementDrained = false)
{
    private readonly IReadOnlyList<int> _cardIds = Array.AsReadOnly(CardIds.ToArray());
    public IReadOnlyList<int> CardIds
    {
        get => _cardIds;
        init => _cardIds = Array.AsReadOnly(value.ToArray());
    }
}

// Scalar-only committed fact: no collection can escape through an observer.
public sealed record ShanheNullifiedTrickMaterialClaimedEvent(long CardUseFrameId, long ActionId,
    long WindowFrameId, int ChainDepth, int RecipientSeat, int CardId, int MovementSequence) : IGameEvent;

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheNullifiedTrickClaim? ShanheNullifiedTrickClaim { get; init; }
}

public sealed partial record CardsMovedTriggerWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? ResumeShanheNullifiedClaimWindowId { get; init; }
}
