#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlacedEquipmentBenefitObserversEntries
{
    internal bool IsPlacedEquipmentBenefitProgramDying() => _resolve().IsPlacedEquipmentBenefitProgramDying();
    internal bool PlacedEquipmentFirstObserver(int index, global::CardGame.Core.ProgramSkillFrame root) => _resolve().PlacedEquipmentFirstObserver(index, root);
    internal global::CardGame.Core.ProgramSkillFrame? PlacedEquipmentObserverRoot(long? damageWindowId = default) => _resolve().PlacedEquipmentObserverRoot(damageWindowId);
    internal bool ValidPlacedEquipmentBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPlacedEquipmentBenefit(f);
    internal bool ValidPlacedEquipmentLedger(global::CardGame.Core.PlacedEquipmentPayment p) => _resolve().ValidPlacedEquipmentLedger(p);
}
