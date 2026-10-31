#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheMobilizationCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheMobilizationRuntime>> getCoreGameEngineShanheMobilizationRuntime) : IShanheMobilizationHost
{
    public SkillProgramStepOutcome ReviveRoundAllies(ProgramSkillFrame frame) => getCoreGameEngineShanheMobilizationRuntime().Value.BeginShanheMobilization(frame);
}
