#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangCombatTacticsEntries
{
    internal bool AllowsWushuangCombatTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsWushuangCombatTrigger(gate);
    internal bool IsWushuangActualDraw(global::CardGame.Core.ProgramSkillWindowContext? c, int owner, scoped out global::CardGame.Core.CardMovementRecord? movement) => _resolve().IsWushuangActualDraw(c, owner, out movement);
    internal void RecordWushuangCombatDamage(global::CardGame.Core.DamageAppliedEvent e) => _resolve().RecordWushuangCombatDamage(e);
    internal void RecordWushuangCombatUse(global::CardGame.Core.CardUseDeclaredEvent e) => _resolve().RecordWushuangCombatUse(e);
    internal bool WushuangArmorBypass(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().WushuangArmorBypass(attack);
    internal int WushuangArmorDamageMultiplier(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().WushuangArmorDamageMultiplier(attack);
    internal int WushuangCombatDamageAmount(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().WushuangCombatDamageAmount(attack);
    internal global::CardGame.Core.RuleQueryContribution? WushuangCombatExtraTargets(global::CardGame.Core.ProgramTargetCountQuery q) => _resolve().WushuangCombatExtraTargets(q);
    internal int WushuangDrawTwoCount(int sourceSeat) => _resolve().WushuangDrawTwoCount(sourceSeat);
    internal bool WushuangDrunkenSlashUnrespondable(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().WushuangDrunkenSlashUnrespondable(attack);
    internal void WushuangExtraAlcoholQuota(global::CardGame.Core.JianjiOrdinaryAlcoholPaidEvent e) => _resolve().WushuangExtraAlcoholQuota(e);
    internal bool WushuangFixedDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().WushuangFixedDamage(attack);
    internal bool WushuangHasVisibleTargetHand(int source, global::CardGame.Core.CardKind kind) => _resolve().WushuangHasVisibleTargetHand(source, kind);
    internal global::CardGame.Core.ProgramDamageInterception WushuangNormalizeFixedDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount, global::CardGame.Core.DamageNature nature) => _resolve().WushuangNormalizeFixedDamage(attack, amount, nature);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> WushuangVisibleTargetHand(int viewer) => _resolve().WushuangVisibleTargetHand(viewer);
}
