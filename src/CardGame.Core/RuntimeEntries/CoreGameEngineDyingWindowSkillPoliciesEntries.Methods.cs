#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingWindowSkillPoliciesEntries
{
    internal long CaptureDyingWindowQualificationStamp(long underlying) => _resolve().CaptureDyingWindowQualificationStamp(underlying);
    internal bool IsBlackTrickTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, bool? effectiveIsRed) => _resolve().IsBlackTrickTargetProhibited(target, kind, effectiveIsRed);
    internal bool IsDyingWindowGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsDyingWindowGrantQualified(owner, grant);
    internal bool IsExclusiveTurnPeachUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => _resolve().IsExclusiveTurnPeachUseForbidden(actorSeat, kind, type);
    internal bool HasBlackTrickTargetPolicy { get => _resolve().HasBlackTrickTargetPolicy; }
    internal bool HasDyingWindowQualificationPolicy { get => _resolve().HasDyingWindowQualificationPolicy; }
}
