#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ActualHandGainAndCategoryGiftCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualHandGainProgramsEntries> getCoreGameEngineActualHandGainProgramsRuntime) : IActualHandGainAndCategoryGiftHost
{
    public SkillProgramStepOutcome ExecuteActualHandGain(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineActualHandGainProgramsRuntime().ExecuteActualHandGain(f, e);
}
