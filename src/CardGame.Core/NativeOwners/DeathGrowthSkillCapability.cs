#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DeathGrowthSkillCapability(Func<RuntimeEntries.CoreGameEngineDeathGrowthSkillsEntries> getCoreGameEngineDeathGrowthSkillsRuntime) : IDeathGrowthSkillHost
{
    public SkillProgramStepOutcome DrawAfterOtherDeathThenChooseMissingSkill(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineDeathGrowthSkillsRuntime().BeginDeathGrowthSkill(f, e);
}
