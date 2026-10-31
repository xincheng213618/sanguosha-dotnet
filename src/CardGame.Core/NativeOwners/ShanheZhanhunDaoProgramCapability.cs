#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheZhanhunDaoProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheZhanhunDaoEntries> getCoreGameEngineShanheZhanhunDaoRuntime) : IShanheZhanhunDaoProgramHost
{
    public SkillProgramStepOutcome ResolveShanheZhanhunDao(ProgramSkillFrame frame) => getCoreGameEngineShanheZhanhunDaoRuntime().ResolveShanheZhanhunDao(frame);
}
