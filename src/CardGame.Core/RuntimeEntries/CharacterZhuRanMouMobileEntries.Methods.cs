#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuRanMouMobileEntries
{
    internal bool MouZhuRanAdvanceChildren() => _resolve().MouZhuRanAdvanceChildren();
    internal bool MouZhuRanBeforeDamage(long id) => _resolve().MouZhuRanBeforeDamage(id);
    internal bool MouZhuRanDamageWindow(long id) => _resolve().MouZhuRanDamageWindow(id);
    internal bool MouZhuRanDying() => _resolve().MouZhuRanDying();
    internal bool MouZhuRanFirstNativeChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouZhuRanFirstNativeChild(f, child);
    internal bool MouZhuRanMovementReturn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouZhuRanMovementReturn(f, effect, p);
    internal bool MouZhuRanNativeEdge(int i) => _resolve().MouZhuRanNativeEdge(i);
    internal global::CardGame.Core.ProgramSkillFrame? MouZhuRanNativeRoot() => _resolve().MouZhuRanNativeRoot();
    internal bool MouZhuRanNativeSuffix(int n) => _resolve().MouZhuRanNativeSuffix(n);
    internal bool MouZhuRanRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouZhuRanRecoveryParent(f, recovery);
    internal global::CardGame.Core.MouZhuRanRuntime MouZhuRan { get => _resolve().MouZhuRan; }
}
