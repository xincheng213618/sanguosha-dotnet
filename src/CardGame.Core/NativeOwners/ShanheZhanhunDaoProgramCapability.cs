#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheZhanhunDaoProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheZhanhunDaoRuntime>> getCoreGameEngineShanheZhanhunDaoRuntime) : IShanheZhanhunDaoProgramHost
{
    public SkillProgramStepOutcome ResolveShanheZhanhunDao(ProgramSkillFrame frame) => getCoreGameEngineShanheZhanhunDaoRuntime().Value.ResolveShanheZhanhunDao(frame);
}
