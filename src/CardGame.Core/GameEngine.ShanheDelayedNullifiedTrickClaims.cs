using System.Text.Json.Serialization;
using CardGame.Core.Campaigns;

namespace CardGame.Core;

// A delayed effect has already completed its original use. Its actual owner is
// this live Nullification window and its immutable native delayed-return token.
public sealed record ShanheDelayedNullifiedTrickClaim(long WindowFrameId, DelayedEffectContinuation Origin,
    int ChainDepth, int RecipientSeat, int ResponseCardId, string SkillId, string SkillInstanceId,
    string GameplayHash, string PolicyId, int MovementBefore, int MovementAfter = 0, bool MovementDrained = false);

public sealed record ShanheDelayedNullifiedTrickMaterialClaimedEvent(long WindowFrameId, int CardId,
    CardKind EffectiveKind, int JudgmentOwnerSeat, int RecipientSeat, int ChainDepth, int MovementSequence) : IGameEvent;

public sealed partial record NullificationWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheDelayedNullifiedTrickClaim? ShanheDelayedNullifiedTrickClaim { get; init; }
}
