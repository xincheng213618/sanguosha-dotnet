#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class EndingPairSlashLossCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineEndingPairDrawsRuntime>> getCoreGameEngineEndingPairDrawsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEnginePlaySlashRecastsRuntime>> getCoreGameEnginePlaySlashRecastsRuntime) : IEndingPairSlashLossHost
{
    public SkillProgramStepOutcome DrawEndingPair(ProgramSkillFrame f, string state) => getCoreGameEngineEndingPairDrawsRuntime().Value.BeginEndingPairDraw(f, state);
    public SkillProgramStepOutcome RecastSelectedPhysicalSlash(ProgramSkillFrame f) => getCoreGameEnginePlaySlashRecastsRuntime().Value.BeginActualPlaySlashRecast(f);
}
