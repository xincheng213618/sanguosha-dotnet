#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5YimieProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5YimieEntries> getCoreGameEngineShanheS5YimieRuntime) : IShanheS5YimieHost
{
    public SkillProgramStepOutcome ResolveShanheS5Yimie(ProgramSkillFrame f, string stage) => getCoreGameEngineShanheS5YimieRuntime().ResolveShanheS5Yimie(f, stage);
}
