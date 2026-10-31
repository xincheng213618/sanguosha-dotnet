#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5LiJueCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheS5LiJueRuntime>> getCoreGameEngineShanheS5LiJueRuntime) : IShanheS5LiJueProgramHost
{
    public SkillProgramStepOutcome BeginPositiveRandomDamage(ProgramSkillFrame frame) => getCoreGameEngineShanheS5LiJueRuntime().Value.BeginShanheS5Langxi(frame);
}
