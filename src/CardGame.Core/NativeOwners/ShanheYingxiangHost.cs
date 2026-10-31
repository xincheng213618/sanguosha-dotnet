#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheYingxiangHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheYingxiangRuntime>> getCoreGameEngineShanheYingxiangRuntime) : IShanheYingxiangHost
{
    public SkillProgramStepOutcome ResolveYingxiang(ProgramSkillFrame frame, bool used) => getCoreGameEngineShanheYingxiangRuntime().Value.BeginShanheYingxiang(frame, used);
}
