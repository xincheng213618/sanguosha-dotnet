#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SourceFactionPreventionProgramCapability(Func<RuntimeEntries.CoreGameEngineSourceFactionPreventionsEntries> getCoreGameEngineSourceFactionPreventionsRuntime) : ISourceFactionPreventionProgramHost
{
    public SkillProgramStepOutcome PreventDamageAndConsumeSourceFaction(ProgramSkillFrame f, string stateId) => getCoreGameEngineSourceFactionPreventionsRuntime().PreventDamageAndConsumeSourceFaction(f, stateId);
}
