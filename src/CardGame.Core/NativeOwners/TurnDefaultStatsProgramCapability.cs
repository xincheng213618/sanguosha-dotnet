#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class TurnDefaultStatsProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries> getCoreGameEngineTurnDefaultStatsRuntime) : ITurnDefaultStatsProgramHost
{
    public SkillProgramStepOutcome ExecuteTurnDefaultStatOperation(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineTurnDefaultStatsRuntime().ExecuteTurnDefaultStatOperation(f, e);
}
