#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class GhostActivityProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGhostActivityProgramsEntries> getCoreGhostActivityProgramsRuntime) : IGhostActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteGhostActivity(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGhostActivityProgramsRuntime().ExecuteGhostActivity(frame, effect);
}
