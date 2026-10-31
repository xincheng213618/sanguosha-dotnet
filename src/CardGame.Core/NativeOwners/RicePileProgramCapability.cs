#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class RicePileProgramCapability(Func<RuntimeEntries.CoreGameEngineRicePileEntries> getCoreGameEngineRicePileRuntime) : IRicePileProgramHost
{
    public SkillProgramStepOutcome ExecuteRicePile(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEngineRicePileRuntime().BeginRicePile(e, f);
}
