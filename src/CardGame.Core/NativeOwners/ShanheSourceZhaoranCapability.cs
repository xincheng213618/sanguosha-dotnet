#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceZhaoranCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramShanheSourceZhaoranEntries> getCoreProgramShanheSourceZhaoranRuntime) : IShanheSourceZhaoranHost
{
    public void Arm(ProgramSkillFrame frame) => getCoreProgramShanheSourceZhaoranRuntime().ArmShanheSourceZhaoran(frame);
    public void ClaimSuit(ProgramSkillFrame frame) => getCoreProgramShanheSourceZhaoranRuntime().ClaimShanheSourceZhaoranSuit(frame);
}
