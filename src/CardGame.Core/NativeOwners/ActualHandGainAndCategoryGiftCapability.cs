#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ActualHandGainAndCategoryGiftCapability(Func<RuntimeEntries.CoreGameEngineActualHandGainProgramsEntries> getCoreGameEngineActualHandGainProgramsRuntime) : IActualHandGainAndCategoryGiftHost
{
    public SkillProgramStepOutcome ExecuteActualHandGain(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineActualHandGainProgramsRuntime().ExecuteActualHandGain(f, e);
}
