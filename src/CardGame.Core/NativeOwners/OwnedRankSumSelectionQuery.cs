#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OwnedRankSumSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedHandRankSumProgramsEntries> getCoreGameEngineOwnedHandRankSumProgramsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : IOwnedRankSumSelectionQuery
{
    public bool CanStart(int ownerSeat, ProgramExecutionPlan plan) => getCoreGameEngineOwnedHandRankSumProgramsRuntime().CanStartOwnedHandRankSum(matchParticipantsState.ReferenceToPlayers[ownerSeat], plan);
}
