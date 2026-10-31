#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlacedEquipmentBenefitObserversEntries
{
    internal bool IsPlacedEquipmentBenefitProgramDying() => _resolve().IsPlacedEquipmentBenefitProgramDying();
    internal bool PlacedEquipmentFirstObserver(int index, ProgramSkillFrame root) => _resolve().PlacedEquipmentFirstObserver(index, root);
    internal ProgramSkillFrame? PlacedEquipmentObserverRoot(long? damageWindowId = default) => _resolve().PlacedEquipmentObserverRoot(damageWindowId);
    internal bool ValidPlacedEquipmentBenefit(ProgramSkillFrame f) => _resolve().ValidPlacedEquipmentBenefit(f);
    internal bool ValidPlacedEquipmentLedger(PlacedEquipmentPayment p) => _resolve().ValidPlacedEquipmentLedger(p);
}
