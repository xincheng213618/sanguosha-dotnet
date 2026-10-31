#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuRanMouMobileEntries
{
    internal bool MouZhuRanAdvanceChildren() => _resolve().MouZhuRanAdvanceChildren();
    internal bool MouZhuRanDying() => _resolve().MouZhuRanDying();
    internal bool MouZhuRanFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouZhuRanFirstNativeChild(f, child);
    internal bool MouZhuRanMovementReturn(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouZhuRanMovementReturn(f, effect, p);
    internal bool MouZhuRanNativeEdge(int i) => _resolve().MouZhuRanNativeEdge(i);
    internal ProgramSkillFrame? MouZhuRanNativeRoot() => _resolve().MouZhuRanNativeRoot();
    internal bool MouZhuRanNativeSuffix(int n) => _resolve().MouZhuRanNativeSuffix(n);
    internal bool MouZhuRanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouZhuRanRecoveryParent(f, recovery);
    internal MouZhuRanRuntime MouZhuRan { get => _resolve().MouZhuRan; }
}
