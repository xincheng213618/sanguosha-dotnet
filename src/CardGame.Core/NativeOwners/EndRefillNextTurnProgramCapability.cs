#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class EndRefillNextTurnProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEndRefillNextTurnEntries> getCoreGameEngineEndRefillNextTurnRuntime) : IEndRefillNextTurnProgramHost
{
    public SkillProgramStepOutcome RefillAndScheduleNextTurnSelfOnly(ProgramSkillFrame f) => getCoreGameEngineEndRefillNextTurnRuntime().BeginEndRefillNextTurn(f);
}
