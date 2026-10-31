#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramTargetCommitCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramTargetCommitRuntime>> getCoreGameEngineProgramTargetCommitRuntime) : IProgramTargetCommitHost
{
    public SkillProgramStepOutcome SelectRelativeZoneDemandTarget(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineProgramTargetCommitRuntime().Value.BeginRelativeZoneDemand(effect, frame);
    public SkillProgramStepOutcome DrawOnFirstProgramTargetEncounter(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineProgramTargetCommitRuntime().Value.DrawFirstProgramTargetReward(effect, frame);
}
