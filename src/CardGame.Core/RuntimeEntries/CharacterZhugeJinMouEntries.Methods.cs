#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeJinMouEntries
{
    internal bool HasMouZhugeJinZijin(int seat) => _resolve().HasMouZhugeJinZijin(seat);
    internal bool MouZhugeJinAdvanceChildren() => _resolve().MouZhugeJinAdvanceChildren();
    internal IEnumerable<MouZhugeJinBudgetIssuedEvent> MouZhugeJinBudgets(int target) => _resolve().MouZhugeJinBudgets(target);
    internal bool MouZhugeJinCanActivate(ProgramActivationLegalityContext c) => _resolve().MouZhugeJinCanActivate(c);
    internal bool MouZhugeJinCooling(int owner) => _resolve().MouZhugeJinCooling(owner);
    internal bool MouZhugeJinDamageApplied(GameEngine.IDamageAttempt a, long id) => _resolve().MouZhugeJinDamageApplied(a, id);
    internal int MouZhugeJinDamageBonus(GameEngine.IDamageAttempt a) => _resolve().MouZhugeJinDamageBonus(a);
    internal bool MouZhugeJinDying() => _resolve().MouZhugeJinDying();
    internal bool MouZhugeJinEdge(int i) => _resolve().MouZhugeJinEdge(i);
    internal bool MouZhugeJinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouZhugeJinFirstChild(f, child);
    internal bool MouZhugeJinMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouZhugeJinMovement(f, effect, p);
    internal bool MouZhugeJinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouZhugeJinRecoveryParent(f, recovery);
    internal ProgramSkillFrame? MouZhugeJinRoot() => _resolve().MouZhugeJinRoot();
    internal bool MouZhugeJinSuffix(int n) => _resolve().MouZhugeJinSuffix(n);
    internal MouZhugeJinRuntime MouZhugeJin { get => _resolve().MouZhugeJin; }
}
