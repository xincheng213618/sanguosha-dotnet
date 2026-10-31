#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class CappedHandRefreshProgramCapability(Func<RuntimeEntries.CoreGameEngineCappedHandRefreshProgramsEntries> getCoreGameEngineCappedHandRefreshProgramsRuntime) : ICappedHandRefreshProgramHost
{
    public SkillProgramStepOutcome DrawThenDiscardHandToMaximumHp(ProgramSkillFrame frame, int targetSeat, int cap) => getCoreGameEngineCappedHandRefreshProgramsRuntime().BeginCappedHandRefresh(frame, targetSeat, cap);
}
