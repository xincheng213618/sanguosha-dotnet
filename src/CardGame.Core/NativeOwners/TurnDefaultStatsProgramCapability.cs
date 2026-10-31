#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class TurnDefaultStatsProgramCapability(Func<RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries> getCoreGameEngineTurnDefaultStatsRuntime) : ITurnDefaultStatsProgramHost
{
    public SkillProgramStepOutcome ExecuteTurnDefaultStatOperation(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineTurnDefaultStatsRuntime().ExecuteTurnDefaultStatOperation(f, e);
}
