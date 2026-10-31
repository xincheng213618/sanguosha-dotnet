#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardObserversEntries
{
    internal bool AdvanceImmediatePrepDiscardChildren() => _resolve().AdvanceImmediatePrepDiscardChildren();
    internal bool HasPrepDiscardDamageObserver(long id) => _resolve().HasPrepDiscardDamageObserver(id);
    internal bool IsPrepDiscardProgramDying() => _resolve().IsPrepDiscardProgramDying();
    internal bool IsPrepDiscardRecoveryParent(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsPrepDiscardRecoveryParent(root, recovery);
    internal bool PrepDiscardObserverSuffix(int rootIndex) => _resolve().PrepDiscardObserverSuffix(rootIndex);
    internal global::CardGame.Core.ProgramSkillFrame? PrepDiscardPaidObserverRoot(long? exactDamageWindow = default) => _resolve().PrepDiscardPaidObserverRoot(exactDamageWindow);
}
