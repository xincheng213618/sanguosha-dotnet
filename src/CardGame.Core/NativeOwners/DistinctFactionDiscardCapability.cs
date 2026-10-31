#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DistinctFactionDiscardCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDistinctFactionDiscardsEntries> getCoreGameEngineDistinctFactionDiscardsRuntime) : IDistinctFactionDiscardHost
{
    public SkillProgramStepOutcome BeginDistinctFactionDiscards(ProgramSkillFrame f) => getCoreGameEngineDistinctFactionDiscardsRuntime().BeginDistinctFactionDiscards(f);
}
