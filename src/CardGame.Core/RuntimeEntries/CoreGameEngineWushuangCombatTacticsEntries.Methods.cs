#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangCombatTacticsEntries
{
    internal bool AllowsWushuangCombatTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsWushuangCombatTrigger(gate);
    internal bool IsWushuangActualDraw(ProgramSkillWindowContext? c, int owner, scoped out CardMovementRecord? movement) => _resolve().IsWushuangActualDraw(c, owner, out movement);
    internal void RecordWushuangCombatDamage(DamageAppliedEvent e) => _resolve().RecordWushuangCombatDamage(e);
    internal void RecordWushuangCombatUse(CardUseDeclaredEvent e) => _resolve().RecordWushuangCombatUse(e);
    internal bool WushuangArmorBypass(GameEngine.IDamageAttempt attack) => _resolve().WushuangArmorBypass(attack);
    internal int WushuangArmorDamageMultiplier(GameEngine.IDamageAttempt attack) => _resolve().WushuangArmorDamageMultiplier(attack);
    internal int WushuangCombatDamageAmount(GameEngine.IDamageAttempt attack) => _resolve().WushuangCombatDamageAmount(attack);
    internal RuleQueryContribution? WushuangCombatExtraTargets(ProgramTargetCountQuery q) => _resolve().WushuangCombatExtraTargets(q);
    internal int WushuangDrawTwoCount(int sourceSeat) => _resolve().WushuangDrawTwoCount(sourceSeat);
    internal bool WushuangDrunkenSlashUnrespondable(NativeOwners.CardAttackHandle attack) => _resolve().WushuangDrunkenSlashUnrespondable(attack);
    internal void WushuangExtraAlcoholQuota(JianjiOrdinaryAlcoholPaidEvent e) => _resolve().WushuangExtraAlcoholQuota(e);
    internal bool WushuangFixedDamage(GameEngine.IDamageAttempt attack) => _resolve().WushuangFixedDamage(attack);
    internal bool WushuangHasVisibleTargetHand(int source, CardKind kind) => _resolve().WushuangHasVisibleTargetHand(source, kind);
    internal ProgramDamageInterception WushuangNormalizeFixedDamage(GameEngine.IDamageAttempt attack, int amount, DamageNature nature) => _resolve().WushuangNormalizeFixedDamage(attack, amount, nature);
    internal IEnumerable<CardSnapshot> WushuangVisibleTargetHand(int viewer) => _resolve().WushuangVisibleTargetHand(viewer);
}
