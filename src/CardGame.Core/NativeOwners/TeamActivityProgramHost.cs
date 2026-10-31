#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TeamActivityProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTeamActivityProgramsEntries> getCoreTeamActivityProgramsRuntime) : ITeamActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteTeamActivity(ProgramSkillFrame f, SkillProgramEffect e) => getCoreTeamActivityProgramsRuntime().ExecuteTeamActivity(f, e);
}
