#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangWeiMouEntries
{
    internal bool IsMouJiangWeiFuzhanExitContext(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsMouJiangWeiFuzhanExitContext(c, w);
    internal bool MouJiangWeiAnniversaryAdvanceChildren() => _resolve().MouJiangWeiAnniversaryAdvanceChildren();
    internal bool MouJiangWeiAnniversaryDying() => _resolve().MouJiangWeiAnniversaryDying();
    internal bool MouJiangWeiAnniversaryEdge(int i) => _resolve().MouJiangWeiAnniversaryEdge(i);
    internal bool MouJiangWeiAnniversaryFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouJiangWeiAnniversaryFirstChild(f, child);
    internal bool MouJiangWeiAnniversaryMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouJiangWeiAnniversaryMovement(f, effect, p);
    internal bool MouJiangWeiAnniversaryRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouJiangWeiAnniversaryRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? MouJiangWeiAnniversaryRoot() => _resolve().MouJiangWeiAnniversaryRoot();
    internal bool MouJiangWeiAnniversarySuffix(int n) => _resolve().MouJiangWeiAnniversarySuffix(n);
    internal global::CardGame.Core.MouJiangWeiAnniversaryRuntime MouJiangWeiAnniversary { get => _resolve().MouJiangWeiAnniversary; }
}
