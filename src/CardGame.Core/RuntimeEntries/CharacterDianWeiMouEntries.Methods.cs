#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDianWeiMouEntries
{
    internal bool IsMouDianWeiSlashUse(long id) => _resolve().IsMouDianWeiSlashUse(id);
    internal bool MouDianWeiAdvanceChildren() => _resolve().MouDianWeiAdvanceChildren();
    internal bool MouDianWeiDying() => _resolve().MouDianWeiDying();
    internal bool MouDianWeiEdge(int i) => _resolve().MouDianWeiEdge(i);
    internal bool MouDianWeiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouDianWeiFirstChild(f, child);
    internal bool MouDianWeiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouDianWeiMovement(f, effect, p);
    internal bool MouDianWeiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouDianWeiRecoveryParent(f, recovery);
    internal ProgramSkillFrame? MouDianWeiRoot() => _resolve().MouDianWeiRoot();
    internal bool MouDianWeiSlashComplete(GameEngine.AttackCompletionReceipt completion) => _resolve().MouDianWeiSlashComplete(completion);
    internal bool MouDianWeiSuffix(int n) => _resolve().MouDianWeiSuffix(n);
    internal MouDianWeiRuntime MouDianWei { get => _resolve().MouDianWei; }
}
