#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class PairedHandRecastProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePairedHandRecastEntries> getCoreGameEnginePairedHandRecastRuntime) : IPairedHandRecastProgramHost
{
    public SkillProgramStepOutcome PairedHandRecast(ProgramSkillFrame f, int partnerSeat) => getCoreGameEnginePairedHandRecastRuntime().BeginPairedHandRecast(f, partnerSeat);
}
