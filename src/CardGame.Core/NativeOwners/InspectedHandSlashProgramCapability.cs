#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class InspectedHandSlashProgramCapability(Func<RuntimeEntries.CoreGameEngineInspectedHandSlashesEntries> getCoreGameEngineInspectedHandSlashesRuntime) : IInspectedHandSlashProgramHost
{
    public SkillProgramStepOutcome PayHpInspectHandThenDiscardOrSlash(ProgramSkillFrame f) => getCoreGameEngineInspectedHandSlashesRuntime().BeginInspectedHandSlash(f);
}
