#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5JixianProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheS5ZhulingRuntime>> getCoreGameEngineShanheS5ZhulingRuntime) : IShanheS5JixianProgramHost
{
    public SkillProgramStepOutcome ResolveShanheS5Jixian(ProgramSkillFrame frame, string stage) => getCoreGameEngineShanheS5ZhulingRuntime().Value.ResolveShanheS5Jixian(frame, stage);
}
