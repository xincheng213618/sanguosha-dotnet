#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OwnedRankSumSelectionQuery(Func<RuntimeEntries.CoreGameEngineOwnedHandRankSumProgramsEntries> getCoreGameEngineOwnedHandRankSumProgramsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IOwnedRankSumSelectionQuery
{
    public bool CanStart(int ownerSeat, ProgramExecutionPlan plan) => getCoreGameEngineOwnedHandRankSumProgramsRuntime().CanStartOwnedHandRankSum(matchParticipantsState.ReferenceToPlayers[ownerSeat], plan);
}
