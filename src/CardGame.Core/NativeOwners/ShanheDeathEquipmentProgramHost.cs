#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheDeathEquipmentProgramHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreProgramShanheDeathEquipmentRuntime>> getCoreProgramShanheDeathEquipmentRuntime) : IShanheDeathEquipmentProgramHost
{
    public void AbolishPublicTeamEnemyEquipmentSlots(ProgramSkillFrame frame) => getCoreProgramShanheDeathEquipmentRuntime().Value.AbolishPublicTeamEnemyEquipmentSlots(frame);
}
