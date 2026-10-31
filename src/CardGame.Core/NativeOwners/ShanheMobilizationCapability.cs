#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheMobilizationCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheMobilizationEntries> getCoreGameEngineShanheMobilizationRuntime) : IShanheMobilizationHost
{
    public SkillProgramStepOutcome ReviveRoundAllies(ProgramSkillFrame frame) => getCoreGameEngineShanheMobilizationRuntime().BeginShanheMobilization(frame);
}
