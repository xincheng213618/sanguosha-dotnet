#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundCardFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFieldEquipmentEntries> getCoreGameEngineFieldEquipmentRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries> getCoreNativeCardAppearanceRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime) : IProgramBoundCardFacts
{
    public bool IsValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public bool IsHuman(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public bool HasHpValue(int seat) => matchParticipantsState.ReferenceToPlayers[seat].HasHpValue;
    public int Hp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Hp;
    public int MaxHp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].MaxHp;
    public IReadOnlyList<Card> Hand(int seat) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).ToArray());
    public IReadOnlyList<Card> Equipment(int seat) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[seat]).ToArray());
    public IReadOnlyList<Card> CardsAt(CardLocation location) => Array.AsReadOnly(getCardZoneStore().CardsAt(location).ToArray());
    public CardLocation Location(int cardId) => getCardZoneStore().GetLocation(cardId);
    public CardMovementRecord? LastMovement(int cardId) => cardMovementJournalState.ReferenceToCardMovements.LastOrDefault(move => move.CardId == cardId);
    public EquipmentSlot FieldEquipmentSlot(Card card) => getCoreGameEngineFieldEquipmentRuntime().FieldEquipmentSlot(card);
    public Suit EffectiveSuit(int seat, Card card) => getCoreNativeCardAppearanceRuntime().EffectiveSuit(matchParticipantsState.ReferenceToPlayers[seat], card);
    public bool ForeignDiscardPrevented(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent) => getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(chooser, card, source, intent);
}
