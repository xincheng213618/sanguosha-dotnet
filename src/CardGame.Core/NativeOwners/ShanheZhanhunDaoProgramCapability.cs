#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheZhanhunDaoProgramCapability(Func<RuntimeEntries.CoreGameEngineShanheZhanhunDaoEntries> getCoreGameEngineShanheZhanhunDaoRuntime) : IShanheZhanhunDaoProgramHost
{
    public SkillProgramStepOutcome ResolveShanheZhanhunDao(ProgramSkillFrame frame) => getCoreGameEngineShanheZhanhunDaoRuntime().ResolveShanheZhanhunDao(frame);
}
