#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HulaoActivityProgramHost(Func<RuntimeEntries.CoreHulaoActivityProgramsEntries> getCoreHulaoActivityProgramsRuntime) : IHulaoActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteHulaoActivity(ProgramSkillFrame f, SkillProgramEffect e) => getCoreHulaoActivityProgramsRuntime().ExecuteHulaoActivity(f, e);
}
