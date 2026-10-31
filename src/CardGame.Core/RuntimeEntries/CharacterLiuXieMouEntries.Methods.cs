#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuXieMouEntries
{
    internal bool MouLiuXieAdvanceChildren() => _resolve().MouLiuXieAdvanceChildren();
    internal bool MouLiuXieDying() => _resolve().MouLiuXieDying();
    internal bool MouLiuXieFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouLiuXieFirstNativeChild(f, child);
    internal bool MouLiuXieMovementReturn(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouLiuXieMovementReturn(f, effect, p);
    internal bool MouLiuXieNativeEdge(int i) => _resolve().MouLiuXieNativeEdge(i);
    internal ProgramSkillFrame? MouLiuXieNativeRoot() => _resolve().MouLiuXieNativeRoot();
    internal bool MouLiuXieNativeSuffix(int n) => _resolve().MouLiuXieNativeSuffix(n);
    internal bool MouLiuXieRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouLiuXieRecoveryParent(f, recovery);
    internal MouLiuXieRuntime MouLiuXie { get => _resolve().MouLiuXie; }
}
