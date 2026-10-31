#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceZirunHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceZirunEntries> getCoreGameEngineShanheSourceZirunRuntime) : IShanheSourceZirunHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceZirunRuntime().BeginShanheSourceZirun(frame);
}
