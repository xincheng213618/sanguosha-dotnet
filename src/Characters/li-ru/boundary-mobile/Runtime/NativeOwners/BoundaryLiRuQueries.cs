#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLiRuQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiRuBoundaryMobileEntries> getCharacterLiRuBoundaryMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime) : IBoundaryLiRuQueries
{
    public IReadOnlyList<int> LostCardTargets(int owner) => getCharacterLiRuBoundaryMobileRuntime().BoundaryLiRuLostCardTargets(owner);
    public bool HasHeCards(int target) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(target) && matchParticipantsState.ReferenceToPlayers[target].IsAlive && getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[target]).Count + getCorePhysicalOwnedCardQueriesRuntime().GetEquipment2(target).Count > 0;
}
