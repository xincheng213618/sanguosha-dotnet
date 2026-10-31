#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class InspectedHandSlashProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineInspectedHandSlashesRuntime>> getCoreGameEngineInspectedHandSlashesRuntime) : IInspectedHandSlashProgramHost
{
    public SkillProgramStepOutcome PayHpInspectHandThenDiscardOrSlash(ProgramSkillFrame f) => getCoreGameEngineInspectedHandSlashesRuntime().Value.BeginInspectedHandSlash(f);
}
