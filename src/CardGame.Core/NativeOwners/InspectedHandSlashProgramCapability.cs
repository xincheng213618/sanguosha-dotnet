#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class InspectedHandSlashProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineInspectedHandSlashesEntries> getCoreGameEngineInspectedHandSlashesRuntime) : IInspectedHandSlashProgramHost
{
    public SkillProgramStepOutcome PayHpInspectHandThenDiscardOrSlash(ProgramSkillFrame f) => getCoreGameEngineInspectedHandSlashesRuntime().BeginInspectedHandSlash(f);
}
