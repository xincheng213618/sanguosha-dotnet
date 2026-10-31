#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NianActivityProgramHost(Func<RuntimeEntries.CoreNianActivityProgramsEntries> getCoreNianActivityProgramsRuntime) : INianActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteNianActivity(ProgramSkillFrame f, SkillProgramEffect e) => getCoreNianActivityProgramsRuntime().ExecuteNianActivity(f, e);
}
