#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeJinMouEntries
{
    internal bool HasMouZhugeJinZijin(int seat) => _resolve().HasMouZhugeJinZijin(seat);
    internal bool MouZhugeJinAdvanceChildren() => _resolve().MouZhugeJinAdvanceChildren();
    internal bool MouZhugeJinBeforeDamage(long id) => _resolve().MouZhugeJinBeforeDamage(id);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MouZhugeJinBudgetIssuedEvent> MouZhugeJinBudgets(int target) => _resolve().MouZhugeJinBudgets(target);
    internal bool MouZhugeJinCanActivate(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().MouZhugeJinCanActivate(c);
    internal bool MouZhugeJinCooling(int owner) => _resolve().MouZhugeJinCooling(owner);
    internal bool MouZhugeJinDamageApplied(global::CardGame.Core.GameEngine.IDamageAttempt a, long id) => _resolve().MouZhugeJinDamageApplied(a, id);
    internal int MouZhugeJinDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().MouZhugeJinDamageBonus(a);
    internal bool MouZhugeJinDamageWindow(long id) => _resolve().MouZhugeJinDamageWindow(id);
    internal bool MouZhugeJinDying() => _resolve().MouZhugeJinDying();
    internal bool MouZhugeJinEdge(int i) => _resolve().MouZhugeJinEdge(i);
    internal bool MouZhugeJinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouZhugeJinFirstChild(f, child);
    internal bool MouZhugeJinMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouZhugeJinMovement(f, effect, p);
    internal bool MouZhugeJinRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouZhugeJinRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? MouZhugeJinRoot() => _resolve().MouZhugeJinRoot();
    internal bool MouZhugeJinSuffix(int n) => _resolve().MouZhugeJinSuffix(n);
    internal global::CardGame.Core.MouZhugeJinRuntime MouZhugeJin { get => _resolve().MouZhugeJin; }
}
