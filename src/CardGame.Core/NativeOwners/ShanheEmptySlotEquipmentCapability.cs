#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheEmptySlotEquipmentCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheEmptySlotEquipmentEntries> getCoreGameEngineShanheEmptySlotEquipmentRuntime) : IShanheEmptySlotEquipmentProgramHost
{
    public SkillProgramStepOutcome PlaceRandomDeckEquipment(ProgramSkillFrame frame, bool armorOnly = false) => getCoreGameEngineShanheEmptySlotEquipmentRuntime().PlaceShanheEmptySlotEquipment(frame, armorOnly);
}
