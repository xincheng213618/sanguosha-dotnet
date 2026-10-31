#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheEmptySlotEquipmentCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheEmptySlotEquipmentRuntime>> getCoreGameEngineShanheEmptySlotEquipmentRuntime) : IShanheEmptySlotEquipmentProgramHost
{
    public SkillProgramStepOutcome PlaceRandomDeckEquipment(ProgramSkillFrame frame, bool armorOnly = false) => getCoreGameEngineShanheEmptySlotEquipmentRuntime().Value.PlaceShanheEmptySlotEquipment(frame, armorOnly);
}
