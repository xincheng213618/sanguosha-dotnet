#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class CappedHandRefreshProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineCappedHandRefreshProgramsRuntime>> getCoreGameEngineCappedHandRefreshProgramsRuntime) : ICappedHandRefreshProgramHost
{
    public SkillProgramStepOutcome DrawThenDiscardHandToMaximumHp(ProgramSkillFrame frame, int targetSeat, int cap) => getCoreGameEngineCappedHandRefreshProgramsRuntime().Value.BeginCappedHandRefresh(frame, targetSeat, cap);
}
