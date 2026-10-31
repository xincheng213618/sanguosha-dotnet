using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record XianlueRecordedEvent(long FrameId, int OwnerSeat, string SkillInstanceId, CardKind Kind) : IGameEvent;
public sealed record XianlueRewardIssuedEvent(long FrameId, int OwnerSeat, string SkillId, string SkillInstanceId, string GameplayHash, int TurnNumber, long WindowId, long CardUseId, long CardActionId) : IGameEvent;
public sealed record XianlueMovementIssuedEvent(long FrameId, XianlueStage Stage, long Before, long After, int? GiftCardId, int? GiftTargetSeat) : IGameEvent;
public sealed record ZaowangGrantedEvent(long FrameId, int OwnerSeat, int TargetSeat, string SkillId, string SkillInstanceId, string GameplayHash, int HpBeforeRecovery, int GrantedMaximumHp) : IGameEvent;
public sealed record IdentityExchangedEvent(long DeathFrameId, int FormerLordSeat, int SuccessorSeat) : IGameEvent;
internal sealed record ZaowangIdentityGrant(long FrameId, int OwnerSeat, int TargetSeat, Role OriginalRole, string SkillInstanceId, string GameplayHash);
public enum XianlueStage { Record, DrawChildren, Distribute, GiftChildren }
public sealed record XianluePendingState(XianlueStage Stage, int CardIndex, string SkillId = "", string SkillInstanceId = "", string GameplayHash = "", long WindowId = 0, long UseId = 0, long ActionId = 0,
    long DrawBefore = 0, long DrawAfter = 0, long GiftBefore = 0, long GiftAfter = 0, int? GiftCardId = null, int? GiftTargetSeat = null);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ZaowangPaidTargetSeat { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public XianluePendingState? XianluePending { get; init; }
}
public sealed partial class GameEngine
{
}
