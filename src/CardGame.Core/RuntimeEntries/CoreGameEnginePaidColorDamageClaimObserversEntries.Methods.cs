#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePaidColorDamageClaimObserversEntries
{
    internal bool HasPaidColorDamageClaimDamageObserver(long exactDamageWindowId) => _resolve().HasPaidColorDamageClaimDamageObserver(exactDamageWindowId);
    internal bool IsPaidColorDamageClaimMovement(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.SkillProgramEffect? op, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsPaidColorDamageClaimMovement(root, op, pending);
    internal bool IsPaidColorDamageClaimProgramDying() => _resolve().IsPaidColorDamageClaimProgramDying();
    internal bool IsValidPaidColorDamageClaimFrame(global::CardGame.Core.ProgramSkillFrame root) => _resolve().IsValidPaidColorDamageClaimFrame(root);
    internal bool PaidColorDamageClaimObserverEdge(int index) => _resolve().PaidColorDamageClaimObserverEdge(index);
    internal global::CardGame.Core.ProgramSkillFrame? PaidColorDamageClaimObserverRoot(long? exactDamageWindowId = default) => _resolve().PaidColorDamageClaimObserverRoot(exactDamageWindowId);
}
