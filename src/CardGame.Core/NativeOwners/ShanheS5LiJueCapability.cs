#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5LiJueCapability(Func<RuntimeEntries.CoreGameEngineShanheS5LiJueEntries> getCoreGameEngineShanheS5LiJueRuntime) : IShanheS5LiJueProgramHost
{
    public SkillProgramStepOutcome BeginPositiveRandomDamage(ProgramSkillFrame frame) => getCoreGameEngineShanheS5LiJueRuntime().BeginShanheS5Langxi(frame);
}
