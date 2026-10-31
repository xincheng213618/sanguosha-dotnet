#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class NianRevivalCapability(Func<RuntimeEntries.CoreGameEngineNianRevivalEntries> getCoreGameEngineNianRevivalRuntime) : INianRevivalHost, INianRevivalSelectionQuery
{
    public SkillProgramStepOutcome ReviveNianAlly(ProgramSkillFrame frame) => getCoreGameEngineNianRevivalRuntime().BeginNianRevival(frame);
    public IReadOnlyList<int> DeadAllies(int ownerSeat) => getCoreGameEngineNianRevivalRuntime().NianDeadAllies(ownerSeat);
}
