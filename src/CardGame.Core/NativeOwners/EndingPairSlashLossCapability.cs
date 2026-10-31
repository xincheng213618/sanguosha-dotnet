#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class EndingPairSlashLossCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEndingPairDrawsEntries> getCoreGameEngineEndingPairDrawsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePlaySlashRecastsEntries> getCoreGameEnginePlaySlashRecastsRuntime) : IEndingPairSlashLossHost
{
    public SkillProgramStepOutcome DrawEndingPair(ProgramSkillFrame f, string state) => getCoreGameEngineEndingPairDrawsRuntime().BeginEndingPairDraw(f, state);
    public SkillProgramStepOutcome RecastSelectedPhysicalSlash(ProgramSkillFrame f) => getCoreGameEnginePlaySlashRecastsRuntime().BeginActualPlaySlashRecast(f);
}
