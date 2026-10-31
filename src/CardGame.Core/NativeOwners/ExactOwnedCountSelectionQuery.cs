#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ExactOwnedCountSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedExactCountAndNamedTargetsEntries> getCoreGameEngineOwnedExactCountAndNamedTargetsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : IExactOwnedCountSelectionQuery
{
    public bool CanStart(int ownerSeat, ProgramExecutionPlan plan) => getCoreGameEngineOwnedExactCountAndNamedTargetsRuntime().CanStartExactOwnedCardCount(matchParticipantsState.ReferenceToPlayers[ownerSeat], plan);
}
