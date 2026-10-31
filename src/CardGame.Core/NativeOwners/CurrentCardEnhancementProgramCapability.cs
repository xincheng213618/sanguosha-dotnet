#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CurrentCardEnhancementProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentCardEnhancementsEntries> getCoreGameEngineCurrentCardEnhancementsRuntime) : ICurrentCardEnhancementProgramHost
{
    public SkillProgramStepOutcome ApplyCurrentCardEnhancements(ProgramSkillFrame frame, int maximum) => getCoreGameEngineCurrentCardEnhancementsRuntime().ApplyProgramCurrentCardEnhancements(frame, maximum);
}
