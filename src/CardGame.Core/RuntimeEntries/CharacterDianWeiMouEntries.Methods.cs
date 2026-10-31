#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDianWeiMouEntries
{
    internal bool IsMouDianWeiSlashUse(long id) => _resolve().IsMouDianWeiSlashUse(id);
    internal bool MouDianWeiAdvanceChildren() => _resolve().MouDianWeiAdvanceChildren();
    internal bool MouDianWeiDying() => _resolve().MouDianWeiDying();
    internal bool MouDianWeiEdge(int i) => _resolve().MouDianWeiEdge(i);
    internal bool MouDianWeiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouDianWeiFirstChild(f, child);
    internal bool MouDianWeiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouDianWeiMovement(f, effect, p);
    internal bool MouDianWeiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouDianWeiRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? MouDianWeiRoot() => _resolve().MouDianWeiRoot();
    internal bool MouDianWeiSlashComplete(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().MouDianWeiSlashComplete(completion);
    internal bool MouDianWeiSuffix(int n) => _resolve().MouDianWeiSuffix(n);
    internal global::CardGame.Core.MouDianWeiRuntime MouDianWei { get => _resolve().MouDianWei; }
}
