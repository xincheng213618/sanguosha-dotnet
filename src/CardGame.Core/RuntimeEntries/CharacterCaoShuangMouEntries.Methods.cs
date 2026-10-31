#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoShuangMouEntries
{
    internal bool MouCaoShuangAnniversaryAdvanceChildren() => _resolve().MouCaoShuangAnniversaryAdvanceChildren();
    internal bool MouCaoShuangAnniversaryDying() => _resolve().MouCaoShuangAnniversaryDying();
    internal bool MouCaoShuangAnniversaryEdge(int i) => _resolve().MouCaoShuangAnniversaryEdge(i);
    internal bool MouCaoShuangAnniversaryFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouCaoShuangAnniversaryFirstChild(f, child);
    internal bool MouCaoShuangAnniversaryMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouCaoShuangAnniversaryMovement(f, effect, p);
    internal bool MouCaoShuangAnniversaryRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouCaoShuangAnniversaryRecoveryParent(f, recovery);
    internal ProgramSkillFrame? MouCaoShuangAnniversaryRoot() => _resolve().MouCaoShuangAnniversaryRoot();
    internal bool MouCaoShuangAnniversarySuffix(int n) => _resolve().MouCaoShuangAnniversarySuffix(n);
    internal MouCaoShuangAnniversaryRuntime MouCaoShuangAnniversary { get => _resolve().MouCaoShuangAnniversary; }
}
