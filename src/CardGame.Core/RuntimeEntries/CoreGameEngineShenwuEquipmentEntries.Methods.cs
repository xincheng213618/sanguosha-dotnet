#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShenwuEquipmentEntries
{
    internal void AddShenwuDistanceBaseTerms(ProgramDistanceQuery query, List<RuleQueryBaseTerm> terms) => _resolve().AddShenwuDistanceBaseTerms(query, terms);
    internal bool AllowsShenwuShuraRecovery(ProgramTriggerLegalityContext gate) => _resolve().AllowsShenwuShuraRecovery(gate);
    internal bool HasShenwuArmor(CharacterState player, CardKind kind) => _resolve().HasShenwuArmor(player, kind);
    internal bool IsShenwuRobeTargetProhibited(int actorSeat, int targetSeat, CardKind kind) => _resolve().IsShenwuRobeTargetProhibited(actorSeat, targetSeat, kind);
    internal DamageNature ShenwuDamageNature(GameEngine.IDamageAttempt attack, DamageNature original) => _resolve().ShenwuDamageNature(attack, original);
    internal int ShenwuDustlessDistanceBonus(CharacterState source, CharacterState target) => _resolve().ShenwuDustlessDistanceBonus(source, target);
    internal string? ShenwuEquipmentSkillGrantId(Card card) => _resolve().ShenwuEquipmentSkillGrantId(card);
    internal string? ShenwuEquipmentSkillId(CardKind kind) => _resolve().ShenwuEquipmentSkillId(kind);
    internal bool ShenwuRobeDirectedTargetProhibited(ProgramDirectedTargetQuery query) => _resolve().ShenwuRobeDirectedTargetProhibited(query);
    internal int ShenwuShuraDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ShenwuShuraDamageBonus(attack);
    internal bool TryApplyShenwuQimen(NativeOwners.CardAttackHandle attack) => _resolve().TryApplyShenwuQimen(attack);
}
