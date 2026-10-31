#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class EquipmentSlotGroupProgramCapability(Func<RuntimeEntries.CoreGameEngineEquipmentSlotGroupProgramsEntries> getCoreGameEngineEquipmentSlotGroupProgramsRuntime) : IEquipmentSlotGroupProgramHost
{
    public void AbolishEquipmentSlotGroup(ProgramSkillFrame frame, IReadOnlyList<EquipmentSlot> slots) => getCoreGameEngineEquipmentSlotGroupProgramsRuntime().AbolishProgramEquipmentSlotGroup(frame, slots);
    public void RecastSelectedEquipment(ProgramSkillFrame frame) => getCoreGameEngineEquipmentSlotGroupProgramsRuntime().RecastProgramSelectedEquipment(frame);
    public void ReplaceSkillsOnPreparation(ProgramSkillFrame frame, IReadOnlyList<string> lost, string acquired) => getCoreGameEngineEquipmentSlotGroupProgramsRuntime().ReplaceProgramPreparationSkills(frame, lost, acquired);
}
