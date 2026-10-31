#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheDeathEquipmentProgramHost(Func<RuntimeEntries.CoreProgramShanheDeathEquipmentEntries> getCoreProgramShanheDeathEquipmentRuntime) : IShanheDeathEquipmentProgramHost
{
    public void AbolishPublicTeamEnemyEquipmentSlots(ProgramSkillFrame frame) => getCoreProgramShanheDeathEquipmentRuntime().AbolishPublicTeamEnemyEquipmentSlots(frame);
}
