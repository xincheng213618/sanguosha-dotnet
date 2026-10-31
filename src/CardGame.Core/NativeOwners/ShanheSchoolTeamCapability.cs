#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSchoolTeamCapability(Func<RuntimeEntries.CoreGameEngineShanheSchoolTeamEntries> getCoreGameEngineShanheSchoolTeamRuntime) : IShanheSchoolTeamHost
{
    public SkillProgramStepOutcome ApplyTeamRank(ProgramSkillFrame frame) => getCoreGameEngineShanheSchoolTeamRuntime().BeginShanheSchoolTeamRank(frame);
    public void ApplyAlliedHealth(ProgramSkillFrame frame) => getCoreGameEngineShanheSchoolTeamRuntime().ApplyShanheAlliedHealth(frame);
}
