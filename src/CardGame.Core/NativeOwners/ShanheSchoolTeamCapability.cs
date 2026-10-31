#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSchoolTeamCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolTeamEntries> getCoreGameEngineShanheSchoolTeamRuntime) : IShanheSchoolTeamHost
{
    public SkillProgramStepOutcome ApplyTeamRank(ProgramSkillFrame frame) => getCoreGameEngineShanheSchoolTeamRuntime().BeginShanheSchoolTeamRank(frame);
    public void ApplyAlliedHealth(ProgramSkillFrame frame) => getCoreGameEngineShanheSchoolTeamRuntime().ApplyShanheAlliedHealth(frame);
}
