#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualDiscardRecoveryObserversEntries
{
    internal bool ActualEquipmentOrDiscardFirstObserver(int index, global::CardGame.Core.ProgramSkillFrame root) => _resolve().ActualEquipmentOrDiscardFirstObserver(index, root);
    internal global::CardGame.Core.ProgramSkillFrame? ActualEquipmentOrDiscardObserverRoot(long id) => _resolve().ActualEquipmentOrDiscardObserverRoot(id);
    internal void AssertActualEquipmentOrDiscardPrograms() => _resolve().AssertActualEquipmentOrDiscardPrograms();
    internal bool HasActualDiscardRecoveryBeforeDamageObserver(long beforeDamageId) => _resolve().HasActualDiscardRecoveryBeforeDamageObserver(beforeDamageId);
    internal bool HasActualDiscardRecoveryDamageObserver(long damageWindowId) => _resolve().HasActualDiscardRecoveryDamageObserver(damageWindowId);
    internal bool IsActualEquipmentOrDiscardMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? op, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsActualEquipmentOrDiscardMovement(f, op, pending);
    internal bool IsActualEquipmentOrDiscardProgramDying() => _resolve().IsActualEquipmentOrDiscardProgramDying();
}
