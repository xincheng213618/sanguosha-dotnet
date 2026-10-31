#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ArmorProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramArmorOperationsEntries> getCoreProgramArmorOperationsRuntime) : IArmorProgramHost
{
    public void GrantArmor(ProgramSkillFrame frame) => getCoreProgramArmorOperationsRuntime().GrantProgramArmor(frame);
}
