#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingWindowSkillPoliciesEntries
{
    internal long CaptureDyingWindowQualificationStamp(long underlying) => _resolve().CaptureDyingWindowQualificationStamp(underlying);
    internal bool IsBlackTrickTargetProhibited(CharacterState target, CardKind kind, bool? effectiveIsRed) => _resolve().IsBlackTrickTargetProhibited(target, kind, effectiveIsRed);
    internal bool IsDyingWindowGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsDyingWindowGrantQualified(owner, grant);
    internal bool IsExclusiveTurnPeachUseForbidden(int actorSeat, CardKind kind, CardActionType type) => _resolve().IsExclusiveTurnPeachUseForbidden(actorSeat, kind, type);
    internal bool HasBlackTrickTargetPolicy { get => _resolve().HasBlackTrickTargetPolicy; }
    internal bool HasDyingWindowQualificationPolicy { get => _resolve().HasDyingWindowQualificationPolicy; }
}
