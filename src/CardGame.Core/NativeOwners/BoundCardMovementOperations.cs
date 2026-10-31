#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundCardMovementOperations(EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CorePhysicalDrawFacadesEntries> getCorePhysicalDrawFacadesRuntime, Func<RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<CardZoneStore> getCardZoneStore) : IProgramBoundCardMovementOperations
{
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;

    public Card? DrawToProcessing(CardMoveReason reason) => getCorePhysicalDrawFacadesRuntime().DrawOneToProcessing(reason);
    public void Move(Card card, CardLocation source, CardLocation destination, CardMoveReason reason, ProgramBoundCardGive? give = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified) => getCorePhysicalMovementFacadesRuntime().MoveCard(card, source, destination, reason, give is { } bill ? getCharacterHuangQuanMouRuntime().NativeGiveCompletion(bill.FrameId, bill.Giver, bill.Recipient, bill.CardIds, bill.SequenceBefore) : null, disclosure: disclosure);
    public void MoveCards(IReadOnlyList<Card> cards, CardLocation source, CardLocation destination, CardMoveReason reason, ProgramBoundCardGive? give = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified) => getCorePhysicalMovementFacadesRuntime().MoveCards(cards, source, destination, reason, give is { } bill ? getCharacterHuangQuanMouRuntime().NativeGiveCompletion(bill.FrameId, bill.Giver, bill.Recipient, bill.CardIds, bill.SequenceBefore) : null, disclosure: disclosure);
    public void MoveMultipleSources(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason, CardMovementDisclosure disclosure, Action<long, IReadOnlyList<CardMovementRecord>>? afterPhysicalRecords = null) => getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords, disclosure: disclosure);
    public void PlaceAtBottom(IReadOnlyList<int> ids) => getCardZoneStore().PlaceDrawPileCardsAtBottom(ids);
    public void PlaceAtTop(IReadOnlyList<int> ids) => getCardZoneStore().PlaceDrawPileCardsAtTop(ids);
}
