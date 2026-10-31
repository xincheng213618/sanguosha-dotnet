#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ArmorProgramCapability(Func<RuntimeEntries.CoreProgramArmorOperationsEntries> getCoreProgramArmorOperationsRuntime) : IArmorProgramHost
{
    public void GrantArmor(ProgramSkillFrame frame) => getCoreProgramArmorOperationsRuntime().GrantProgramArmor(frame);
}
