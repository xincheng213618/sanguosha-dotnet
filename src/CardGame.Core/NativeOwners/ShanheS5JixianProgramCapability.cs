#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5JixianProgramCapability(Func<RuntimeEntries.CoreGameEngineShanheS5ZhulingEntries> getCoreGameEngineShanheS5ZhulingRuntime) : IShanheS5JixianProgramHost
{
    public SkillProgramStepOutcome ResolveShanheS5Jixian(ProgramSkillFrame frame, string stage) => getCoreGameEngineShanheS5ZhulingRuntime().ResolveShanheS5Jixian(frame, stage);
}
