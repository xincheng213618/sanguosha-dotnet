#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceZirunHost(Func<RuntimeEntries.CoreGameEngineShanheSourceZirunEntries> getCoreGameEngineShanheSourceZirunRuntime) : IShanheSourceZirunHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceZirunRuntime().BeginShanheSourceZirun(frame);
}
