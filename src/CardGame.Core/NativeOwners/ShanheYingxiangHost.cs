#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheYingxiangHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheYingxiangEntries> getCoreGameEngineShanheYingxiangRuntime) : IShanheYingxiangHost
{
    public SkillProgramStepOutcome ResolveYingxiang(ProgramSkillFrame frame, bool used) => getCoreGameEngineShanheYingxiangRuntime().BeginShanheYingxiang(frame, used);
}
