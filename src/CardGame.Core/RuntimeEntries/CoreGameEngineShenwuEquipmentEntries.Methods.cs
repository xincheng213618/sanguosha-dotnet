#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShenwuEquipmentEntries
{
    internal void AddShenwuDistanceBaseTerms(global::CardGame.Core.ProgramDistanceQuery query, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryBaseTerm> terms) => _resolve().AddShenwuDistanceBaseTerms(query, terms);
    internal bool AllowsShenwuShuraRecovery(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsShenwuShuraRecovery(gate);
    internal bool HasShenwuArmor(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => _resolve().HasShenwuArmor(player, kind);
    internal bool IsShenwuRobeTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind kind) => _resolve().IsShenwuRobeTargetProhibited(actorSeat, targetSeat, kind);
    internal global::CardGame.Core.DamageNature ShenwuDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.DamageNature original) => _resolve().ShenwuDamageNature(attack, original);
    internal int ShenwuDustlessDistanceBonus(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target) => _resolve().ShenwuDustlessDistanceBonus(source, target);
    internal string? ShenwuEquipmentSkillGrantId(global::CardGame.Core.Card card) => _resolve().ShenwuEquipmentSkillGrantId(card);
    internal string? ShenwuEquipmentSkillId(global::CardGame.Core.CardKind kind) => _resolve().ShenwuEquipmentSkillId(kind);
    internal bool ShenwuRobeDirectedTargetProhibited(global::CardGame.Core.ProgramDirectedTargetQuery query) => _resolve().ShenwuRobeDirectedTargetProhibited(query);
    internal int ShenwuShuraDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ShenwuShuraDamageBonus(attack);
    internal bool TryApplyShenwuQimen(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryApplyShenwuQimen(attack);
}
