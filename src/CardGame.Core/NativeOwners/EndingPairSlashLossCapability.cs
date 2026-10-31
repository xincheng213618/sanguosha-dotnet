#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class EndingPairSlashLossCapability(Func<RuntimeEntries.CoreGameEngineEndingPairDrawsEntries> getCoreGameEngineEndingPairDrawsRuntime, Func<RuntimeEntries.CoreGameEnginePlaySlashRecastsEntries> getCoreGameEnginePlaySlashRecastsRuntime) : IEndingPairSlashLossHost
{
    public SkillProgramStepOutcome DrawEndingPair(ProgramSkillFrame f, string state) => getCoreGameEngineEndingPairDrawsRuntime().BeginEndingPairDraw(f, state);
    public SkillProgramStepOutcome RecastSelectedPhysicalSlash(ProgramSkillFrame f) => getCoreGameEnginePlaySlashRecastsRuntime().BeginActualPlaySlashRecast(f);
}
