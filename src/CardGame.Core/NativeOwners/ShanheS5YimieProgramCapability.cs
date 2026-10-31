#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5YimieProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheS5YimieRuntime>> getCoreGameEngineShanheS5YimieRuntime) : IShanheS5YimieHost
{
    public SkillProgramStepOutcome ResolveShanheS5Yimie(ProgramSkillFrame f, string stage) => getCoreGameEngineShanheS5YimieRuntime().Value.ResolveShanheS5Yimie(f, stage);
}
