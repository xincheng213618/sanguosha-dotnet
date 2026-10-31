#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DeckCardsFacts(EngineStates.CoreMatchParticipantsState matchParticipantsState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries> getCoreHappyWeiYanGainDestinationsRuntime, Func<RuntimeEntries.CharacterZuoCiHappyGodEntries> getCharacterZuoCiHappyGodRuntime) : IProgramDeckCardsFacts
{
    public bool Alive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public IReadOnlyList<ProgramDeckCard> DrawPile => Array.AsReadOnly(getCardZoneStore().CardsAt(CardLocation.DrawPile).Select(card => new ProgramDeckCard(card.Id, card.Kind)).ToArray());

    public IReadOnlyList<CardMovementRecord> MovementRange(long before, long after) => Array.AsReadOnly(cardMovementJournalState.ReferenceToCardMovements.Where(move => move.Sequence > before && move.Sequence <= after).ToArray());
    public IEnumerable<DeckCardsObtainedEvent> ObtainedEvents => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<DeckCardsObtainedEvent>();

    public bool IsRedirectedGain(CardMovementRecord movement, CardLocation requested) => getCoreHappyWeiYanGainDestinationsRuntime().IsHappyWeiYanRedirectedGain(movement, requested);
    public bool IsDestroyedMaterial(CardMovementRecord movement) => getCharacterZuoCiHappyGodRuntime().IsHappyZuoCiDestroyedMaterial(movement);
}
