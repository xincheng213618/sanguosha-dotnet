#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardObserversEntries
{
    internal bool AdvanceImmediatePrepDiscardChildren() => _resolve().AdvanceImmediatePrepDiscardChildren();
    internal bool IsPrepDiscardProgramDying() => _resolve().IsPrepDiscardProgramDying();
    internal bool IsPrepDiscardRecoveryParent(ProgramSkillFrame root, RecoveryReplacementFrame recovery) => _resolve().IsPrepDiscardRecoveryParent(root, recovery);
    internal bool PrepDiscardObserverSuffix(int rootIndex) => _resolve().PrepDiscardObserverSuffix(rootIndex);
    internal ProgramSkillFrame? PrepDiscardPaidObserverRoot(long? exactDamageWindow = default) => _resolve().PrepDiscardPaidObserverRoot(exactDamageWindow);
}
