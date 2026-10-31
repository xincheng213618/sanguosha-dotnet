#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class BoundarySunXiuSelection(global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore) : IBoundarySunXiuSelectionQuery
{
    public bool HasOwnedRegionCards(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count + getCorePhysicalOwnedCardQueriesRuntime().GetEquipment2(seat).Count + getCardZoneStore().CardsAt(CardLocation.Judgment(seat)).Count > 0;
}
