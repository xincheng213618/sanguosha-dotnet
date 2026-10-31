#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DistinctFactionDiscardCapability(Func<RuntimeEntries.CoreGameEngineDistinctFactionDiscardsEntries> getCoreGameEngineDistinctFactionDiscardsRuntime) : IDistinctFactionDiscardHost
{
    public SkillProgramStepOutcome BeginDistinctFactionDiscards(ProgramSkillFrame f) => getCoreGameEngineDistinctFactionDiscardsRuntime().BeginDistinctFactionDiscards(f);
}
