#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HulaoActivityProgramHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreHulaoActivityProgramsRuntime>> getCoreHulaoActivityProgramsRuntime) : IHulaoActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteHulaoActivity(ProgramSkillFrame f, SkillProgramEffect e) => getCoreHulaoActivityProgramsRuntime().Value.ExecuteHulaoActivity(f, e);
}
