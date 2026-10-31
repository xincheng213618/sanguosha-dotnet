#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class CappedHandRefreshProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCappedHandRefreshProgramsEntries> getCoreGameEngineCappedHandRefreshProgramsRuntime) : ICappedHandRefreshProgramHost
{
    public SkillProgramStepOutcome DrawThenDiscardHandToMaximumHp(ProgramSkillFrame frame, int targetSeat, int cap) => getCoreGameEngineCappedHandRefreshProgramsRuntime().BeginCappedHandRefresh(frame, targetSeat, cap);
}
