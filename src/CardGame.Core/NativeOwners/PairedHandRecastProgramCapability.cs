#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PairedHandRecastProgramCapability(Func<RuntimeEntries.CoreGameEnginePairedHandRecastEntries> getCoreGameEnginePairedHandRecastRuntime) : IPairedHandRecastProgramHost
{
    public SkillProgramStepOutcome PairedHandRecast(ProgramSkillFrame f, int partnerSeat) => getCoreGameEnginePairedHandRecastRuntime().BeginPairedHandRecast(f, partnerSeat);
}
