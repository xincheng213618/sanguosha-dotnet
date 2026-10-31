#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePaidColorDamageClaimObserversEntries
{
    internal bool IsPaidColorDamageClaimProgramDying() => _resolve().IsPaidColorDamageClaimProgramDying();
    internal bool IsValidPaidColorDamageClaimFrame(ProgramSkillFrame root) => _resolve().IsValidPaidColorDamageClaimFrame(root);
    internal bool PaidColorDamageClaimObserverEdge(int index) => _resolve().PaidColorDamageClaimObserverEdge(index);
    internal ProgramSkillFrame? PaidColorDamageClaimObserverRoot(long? exactDamageWindowId = default) => _resolve().PaidColorDamageClaimObserverRoot(exactDamageWindowId);
}
