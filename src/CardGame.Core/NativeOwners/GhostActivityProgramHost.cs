#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class GhostActivityProgramHost(Func<RuntimeEntries.CoreGhostActivityProgramsEntries> getCoreGhostActivityProgramsRuntime) : IGhostActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteGhostActivity(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGhostActivityProgramsRuntime().ExecuteGhostActivity(frame, effect);
}
