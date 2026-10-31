#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SourceFactionPreventionProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSourceFactionPreventionsRuntime>> getCoreGameEngineSourceFactionPreventionsRuntime) : ISourceFactionPreventionProgramHost
{
    public SkillProgramStepOutcome PreventDamageAndConsumeSourceFaction(ProgramSkillFrame f, string stateId) => getCoreGameEngineSourceFactionPreventionsRuntime().Value.PreventDamageAndConsumeSourceFaction(f, stateId);
}
