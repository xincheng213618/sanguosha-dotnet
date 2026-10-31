#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheGuideSkillsHost(Func<RuntimeEntries.CoreGameEngineShanheGuideSkillsEntries> getCoreGameEngineShanheGuideSkillsRuntime) : IShanheGuideSkillsHost
{
    public SkillProgramStepOutcome ResolveGuideSkill(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineShanheGuideSkillsRuntime().ResolveShanheGuideSkill(frame, effect);
}
