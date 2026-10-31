#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramTargetCommitCapability(Func<RuntimeEntries.CoreGameEngineProgramTargetCommitEntries> getCoreGameEngineProgramTargetCommitRuntime) : IProgramTargetCommitHost
{
    public SkillProgramStepOutcome SelectRelativeZoneDemandTarget(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineProgramTargetCommitRuntime().BeginRelativeZoneDemand(effect, frame);
    public SkillProgramStepOutcome DrawOnFirstProgramTargetEncounter(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineProgramTargetCommitRuntime().DrawFirstProgramTargetReward(effect, frame);
}
