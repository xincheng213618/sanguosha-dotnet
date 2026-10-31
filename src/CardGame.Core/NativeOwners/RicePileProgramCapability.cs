#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class RicePileProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRicePileEntries> getCoreGameEngineRicePileRuntime) : IRicePileProgramHost
{
    public SkillProgramStepOutcome ExecuteRicePile(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEngineRicePileRuntime().BeginRicePile(e, f);
}
