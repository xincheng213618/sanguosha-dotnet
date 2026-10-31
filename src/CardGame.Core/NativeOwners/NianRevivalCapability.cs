#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class NianRevivalCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineNianRevivalRuntime>> getCoreGameEngineNianRevivalRuntime) : INianRevivalHost, INianRevivalSelectionQuery
{
    public SkillProgramStepOutcome ReviveNianAlly(ProgramSkillFrame frame) => getCoreGameEngineNianRevivalRuntime().Value.BeginNianRevival(frame);
    public IReadOnlyList<int> DeadAllies(int ownerSeat) => getCoreGameEngineNianRevivalRuntime().Value.NianDeadAllies(ownerSeat);
}
