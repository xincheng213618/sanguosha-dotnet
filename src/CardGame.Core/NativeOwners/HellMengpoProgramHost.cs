#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class HellMengpoProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries> getCoreHellMengpoProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHellSoulCardsEntries> getCoreGameEngineHellSoulCardsRuntime) : IHellMengpoProgramHost
{
    public SkillProgramStepOutcome Forget(ProgramSkillFrame frame) => getCoreHellMengpoProgramsRuntime().BeginHellWanghun(frame);
    public SkillProgramStepOutcome BanCategory(ProgramSkillFrame frame) => getCoreHellMengpoProgramsRuntime().IssueHellWangshiBan(frame);
    public SkillProgramStepOutcome Restore(ProgramSkillFrame frame) => getCoreGameEngineHellSoulCardsRuntime().BeginHellSoulRestoration(frame);
}
