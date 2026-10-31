#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheMobilizationCapability(Func<RuntimeEntries.CoreGameEngineShanheMobilizationEntries> getCoreGameEngineShanheMobilizationRuntime) : IShanheMobilizationHost
{
    public SkillProgramStepOutcome ReviveRoundAllies(ProgramSkillFrame frame) => getCoreGameEngineShanheMobilizationRuntime().BeginShanheMobilization(frame);
}
