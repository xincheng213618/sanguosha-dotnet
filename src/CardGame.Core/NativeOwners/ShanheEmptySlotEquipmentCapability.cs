#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheEmptySlotEquipmentCapability(Func<RuntimeEntries.CoreGameEngineShanheEmptySlotEquipmentEntries> getCoreGameEngineShanheEmptySlotEquipmentRuntime) : IShanheEmptySlotEquipmentProgramHost
{
    public SkillProgramStepOutcome PlaceRandomDeckEquipment(ProgramSkillFrame frame, bool armorOnly = false) => getCoreGameEngineShanheEmptySlotEquipmentRuntime().PlaceShanheEmptySlotEquipment(frame, armorOnly);
}
