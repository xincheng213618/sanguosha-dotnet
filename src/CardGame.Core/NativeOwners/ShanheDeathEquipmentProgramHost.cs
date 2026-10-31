#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheDeathEquipmentProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramShanheDeathEquipmentEntries> getCoreProgramShanheDeathEquipmentRuntime) : IShanheDeathEquipmentProgramHost
{
    public void AbolishPublicTeamEnemyEquipmentSlots(ProgramSkillFrame frame) => getCoreProgramShanheDeathEquipmentRuntime().AbolishPublicTeamEnemyEquipmentSlots(frame);
}
