#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class GhostActivityProgramHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGhostActivityProgramsRuntime>> getCoreGhostActivityProgramsRuntime) : IGhostActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteGhostActivity(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGhostActivityProgramsRuntime().Value.ExecuteGhostActivity(frame, effect);
}
