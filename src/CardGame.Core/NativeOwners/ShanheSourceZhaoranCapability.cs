#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceZhaoranCapability(Func<RuntimeEntries.CoreProgramShanheSourceZhaoranEntries> getCoreProgramShanheSourceZhaoranRuntime) : IShanheSourceZhaoranHost
{
    public void Arm(ProgramSkillFrame frame) => getCoreProgramShanheSourceZhaoranRuntime().ArmShanheSourceZhaoran(frame);
    public void ClaimSuit(ProgramSkillFrame frame) => getCoreProgramShanheSourceZhaoranRuntime().ClaimShanheSourceZhaoranSuit(frame);
}
