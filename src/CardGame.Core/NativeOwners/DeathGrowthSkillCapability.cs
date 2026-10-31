#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DeathGrowthSkillCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDeathGrowthSkillsRuntime>> getCoreGameEngineDeathGrowthSkillsRuntime) : IDeathGrowthSkillHost
{
    public SkillProgramStepOutcome DrawAfterOtherDeathThenChooseMissingSkill(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineDeathGrowthSkillsRuntime().Value.BeginDeathGrowthSkill(f, e);
}
