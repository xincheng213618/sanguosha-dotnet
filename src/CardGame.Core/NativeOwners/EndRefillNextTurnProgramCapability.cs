#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class EndRefillNextTurnProgramCapability(Func<RuntimeEntries.CoreGameEngineEndRefillNextTurnEntries> getCoreGameEngineEndRefillNextTurnRuntime) : IEndRefillNextTurnProgramHost
{
    public SkillProgramStepOutcome RefillAndScheduleNextTurnSelfOnly(ProgramSkillFrame f) => getCoreGameEngineEndRefillNextTurnRuntime().BeginEndRefillNextTurn(f);
}
