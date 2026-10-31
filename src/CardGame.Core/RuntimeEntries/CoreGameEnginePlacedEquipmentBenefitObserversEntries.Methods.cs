#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlacedEquipmentBenefitObserversEntries
{
    internal void AssertPlacedEquipmentBenefits() => _resolve().AssertPlacedEquipmentBenefits();
    internal bool HasPlacedEquipmentBenefitDamageObserver(long damageWindowId) => _resolve().HasPlacedEquipmentBenefitDamageObserver(damageWindowId);
    internal bool IsPlacedEquipmentBenefitMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsPlacedEquipmentBenefitMovement(f, effect, pending);
    internal bool IsPlacedEquipmentBenefitProgramDying() => _resolve().IsPlacedEquipmentBenefitProgramDying();
    internal bool PlacedEquipmentFirstObserver(int index, global::CardGame.Core.ProgramSkillFrame root) => _resolve().PlacedEquipmentFirstObserver(index, root);
    internal global::CardGame.Core.ProgramSkillFrame? PlacedEquipmentObserverRoot(long? damageWindowId = default) => _resolve().PlacedEquipmentObserverRoot(damageWindowId);
    internal bool ValidPlacedEquipmentBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPlacedEquipmentBenefit(f);
    internal bool ValidPlacedEquipmentLedger(global::CardGame.Core.PlacedEquipmentPayment p) => _resolve().ValidPlacedEquipmentLedger(p);
}
