#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceZirunHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceZirunRuntime>> getCoreGameEngineShanheSourceZirunRuntime) : IShanheSourceZirunHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceZirunRuntime().Value.BeginShanheSourceZirun(frame);
}
