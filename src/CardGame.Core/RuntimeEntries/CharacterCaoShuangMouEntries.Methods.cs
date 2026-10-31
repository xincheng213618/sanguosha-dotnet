#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoShuangMouEntries
{
    internal bool MouCaoShuangAnniversaryAdvanceChildren() => _resolve().MouCaoShuangAnniversaryAdvanceChildren();
    internal bool MouCaoShuangAnniversaryDying() => _resolve().MouCaoShuangAnniversaryDying();
    internal bool MouCaoShuangAnniversaryEdge(int i) => _resolve().MouCaoShuangAnniversaryEdge(i);
    internal bool MouCaoShuangAnniversaryFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouCaoShuangAnniversaryFirstChild(f, child);
    internal bool MouCaoShuangAnniversaryMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouCaoShuangAnniversaryMovement(f, effect, p);
    internal bool MouCaoShuangAnniversaryRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouCaoShuangAnniversaryRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? MouCaoShuangAnniversaryRoot() => _resolve().MouCaoShuangAnniversaryRoot();
    internal bool MouCaoShuangAnniversarySuffix(int n) => _resolve().MouCaoShuangAnniversarySuffix(n);
    internal global::CardGame.Core.MouCaoShuangAnniversaryRuntime MouCaoShuangAnniversary { get => _resolve().MouCaoShuangAnniversary; }
}
