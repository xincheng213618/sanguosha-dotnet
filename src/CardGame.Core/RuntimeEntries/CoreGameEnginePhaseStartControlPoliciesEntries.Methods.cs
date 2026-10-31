#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseStartControlPoliciesEntries
{
    internal RuleQueryEvaluation ApplyOtherPlayRestrictionDistance(CharacterState source, CharacterState target, RuleQueryEvaluation original) => _resolve().ApplyOtherPlayRestrictionDistance(source, target, original);
    internal void GrantOtherPlayRestriction(ProgramSkillFrame f) => _resolve().GrantOtherPlayRestriction(f);
    internal void GrantStartingEquipmentSlashProhibition(ProgramSkillFrame f) => _resolve().GrantStartingEquipmentSlashProhibition(f);
    internal bool HasOtherPlayRestrictionPolicy(int actor) => _resolve().HasOtherPlayRestrictionPolicy(actor);
    internal bool HasPhaseStartCardUseParticipant(int actor) => _resolve().HasPhaseStartCardUseParticipant(actor);
    internal bool IsExactOtherPlayRestrictionGrant(OtherPlayRestrictionGrantedEvent grant) => _resolve().IsExactOtherPlayRestrictionGrant(grant);
    internal bool IsOtherPlayRestrictionUseForbidden(int actor, CardKind kind, CardActionType type) => _resolve().IsOtherPlayRestrictionUseForbidden(actor, kind, type);
}
