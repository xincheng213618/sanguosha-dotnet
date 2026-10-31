#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseStartControlPoliciesEntries
{
    internal global::CardGame.Core.RuleQueryEvaluation ApplyOtherPlayRestrictionDistance(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.RuleQueryEvaluation original) => _resolve().ApplyOtherPlayRestrictionDistance(source, target, original);
    internal void GrantOtherPlayRestriction(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GrantOtherPlayRestriction(f);
    internal void GrantStartingEquipmentSlashProhibition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GrantStartingEquipmentSlashProhibition(f);
    internal bool HasOtherPlayRestrictionPolicy(int actor) => _resolve().HasOtherPlayRestrictionPolicy(actor);
    internal bool HasPhaseStartCardUseParticipant(int actor) => _resolve().HasPhaseStartCardUseParticipant(actor);
    internal bool IsExactOtherPlayRestrictionGrant(global::CardGame.Core.OtherPlayRestrictionGrantedEvent grant) => _resolve().IsExactOtherPlayRestrictionGrant(grant);
    internal bool IsOtherPlayRestrictionUseForbidden(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => _resolve().IsOtherPlayRestrictionUseForbidden(actor, kind, type);
}
