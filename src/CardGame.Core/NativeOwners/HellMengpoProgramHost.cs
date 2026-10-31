#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HellMengpoProgramHost(Func<RuntimeEntries.CoreHellMengpoProgramsEntries> getCoreHellMengpoProgramsRuntime, Func<RuntimeEntries.CoreGameEngineHellSoulCardsEntries> getCoreGameEngineHellSoulCardsRuntime) : IHellMengpoProgramHost
{
    public SkillProgramStepOutcome Forget(ProgramSkillFrame frame) => getCoreHellMengpoProgramsRuntime().BeginHellWanghun(frame);
    public SkillProgramStepOutcome BanCategory(ProgramSkillFrame frame) => getCoreHellMengpoProgramsRuntime().IssueHellWangshiBan(frame);
    public SkillProgramStepOutcome Restore(ProgramSkillFrame frame) => getCoreGameEngineHellSoulCardsRuntime().BeginHellSoulRestoration(frame);
}
