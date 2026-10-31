#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuXieMouEntries
{
    internal bool MouLiuXieAdvanceChildren() => _resolve().MouLiuXieAdvanceChildren();
    internal bool MouLiuXieDying() => _resolve().MouLiuXieDying();
    internal bool MouLiuXieFirstNativeChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouLiuXieFirstNativeChild(f, child);
    internal bool MouLiuXieMovementReturn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouLiuXieMovementReturn(f, effect, p);
    internal bool MouLiuXieNativeEdge(int i) => _resolve().MouLiuXieNativeEdge(i);
    internal global::CardGame.Core.ProgramSkillFrame? MouLiuXieNativeRoot() => _resolve().MouLiuXieNativeRoot();
    internal bool MouLiuXieNativeSuffix(int n) => _resolve().MouLiuXieNativeSuffix(n);
    internal bool MouLiuXieRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouLiuXieRecoveryParent(f, recovery);
    internal global::CardGame.Core.MouLiuXieRuntime MouLiuXie { get => _resolve().MouLiuXie; }
}
