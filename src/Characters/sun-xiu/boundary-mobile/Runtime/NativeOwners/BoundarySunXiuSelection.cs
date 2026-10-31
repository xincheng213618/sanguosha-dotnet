#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundarySunXiuSelection(Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<CardZoneStore> getCardZoneStore) : IBoundarySunXiuSelectionQuery
{
    public bool HasOwnedRegionCards(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count + getCorePhysicalOwnedCardQueriesRuntime().GetEquipment2(seat).Count + getCardZoneStore().CardsAt(CardLocation.Judgment(seat)).Count > 0;
}
