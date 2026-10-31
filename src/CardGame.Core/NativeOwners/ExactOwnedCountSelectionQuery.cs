#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ExactOwnedCountSelectionQuery(Func<RuntimeEntries.CoreGameEngineOwnedExactCountAndNamedTargetsEntries> getCoreGameEngineOwnedExactCountAndNamedTargetsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IExactOwnedCountSelectionQuery
{
    public bool CanStart(int ownerSeat, ProgramExecutionPlan plan) => getCoreGameEngineOwnedExactCountAndNamedTargetsRuntime().CanStartExactOwnedCardCount(matchParticipantsState.ReferenceToPlayers[ownerSeat], plan);
}
