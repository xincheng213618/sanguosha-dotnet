#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLiRuQueries(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiRuBoundaryMobileRuntime>> getCharacterLiRuBoundaryMobileRuntime, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime) : IBoundaryLiRuQueries
{
    public IReadOnlyList<int> LostCardTargets(int owner) => getCharacterLiRuBoundaryMobileRuntime().Value.BoundaryLiRuLostCardTargets(owner);
    public bool HasHeCards(int target) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(target) && matchParticipantsState.ReferenceToPlayers[target].IsAlive && getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[target]).Count + getCorePhysicalOwnedCardQueriesRuntime().GetEquipment2(target).Count > 0;
}
