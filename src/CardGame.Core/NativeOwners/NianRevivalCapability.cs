#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class NianRevivalCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNianRevivalEntries> getCoreGameEngineNianRevivalRuntime) : INianRevivalHost, INianRevivalSelectionQuery
{
    public SkillProgramStepOutcome ReviveNianAlly(ProgramSkillFrame frame) => getCoreGameEngineNianRevivalRuntime().BeginNianRevival(frame);
    public IReadOnlyList<int> DeadAllies(int ownerSeat) => getCoreGameEngineNianRevivalRuntime().NianDeadAllies(ownerSeat);
}
