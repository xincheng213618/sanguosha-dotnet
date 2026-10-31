#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualDiscardRecoveryObserversEntries
{
    internal bool ActualEquipmentOrDiscardFirstObserver(int index, global::CardGame.Core.ProgramSkillFrame root) => _resolve().ActualEquipmentOrDiscardFirstObserver(index, root);
    internal global::CardGame.Core.ProgramSkillFrame? ActualEquipmentOrDiscardObserverRoot(long id) => _resolve().ActualEquipmentOrDiscardObserverRoot(id);
    internal bool IsActualEquipmentOrDiscardProgramDying() => _resolve().IsActualEquipmentOrDiscardProgramDying();
}
