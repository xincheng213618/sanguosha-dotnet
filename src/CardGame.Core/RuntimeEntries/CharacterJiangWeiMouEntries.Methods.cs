#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangWeiMouEntries
{
    internal bool IsMouJiangWeiFuzhanExitContext(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsMouJiangWeiFuzhanExitContext(c, w);
    internal bool MouJiangWeiAnniversaryAdvanceChildren() => _resolve().MouJiangWeiAnniversaryAdvanceChildren();
    internal bool MouJiangWeiAnniversaryDying() => _resolve().MouJiangWeiAnniversaryDying();
    internal bool MouJiangWeiAnniversaryEdge(int i) => _resolve().MouJiangWeiAnniversaryEdge(i);
    internal bool MouJiangWeiAnniversaryFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouJiangWeiAnniversaryFirstChild(f, child);
    internal bool MouJiangWeiAnniversaryMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouJiangWeiAnniversaryMovement(f, effect, p);
    internal bool MouJiangWeiAnniversaryRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouJiangWeiAnniversaryRecoveryParent(f, recovery);
    internal ProgramSkillFrame? MouJiangWeiAnniversaryRoot() => _resolve().MouJiangWeiAnniversaryRoot();
    internal bool MouJiangWeiAnniversarySuffix(int n) => _resolve().MouJiangWeiAnniversarySuffix(n);
    internal MouJiangWeiAnniversaryRuntime MouJiangWeiAnniversary { get => _resolve().MouJiangWeiAnniversary; }
}
