#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDeckCardsRuntime
{
    bool IProgramDeckCardsFacts.Alive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    long IProgramDeckCardsFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    IReadOnlyList<ProgramDeckCard> IProgramDeckCardsFacts.DrawPile => Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(card => new ProgramDeckCard(card.Id, card.Kind)).ToArray());

    IReadOnlyList<CardMovementRecord> IProgramDeckCardsFacts.MovementRange(long before, long after) => Array.AsReadOnly(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > before && move.Sequence <= after).ToArray());
    IEnumerable<DeckCardsObtainedEvent> IProgramDeckCardsFacts.ObtainedEvents => _coreProgramEventHistoryOperations.ProgramEventHistory<DeckCardsObtainedEvent>();

    bool IProgramDeckCardsFacts.IsRedirectedGain(CardMovementRecord movement, CardLocation requested) => _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(movement, requested);
    bool IProgramDeckCardsFacts.IsDestroyedMaterial(CardMovementRecord movement) => _characterZuoCiHappyGodOperations.IsHappyZuoCiDestroyedMaterial(movement);
    bool IProgramDeckCardsOperations.EnsureDrawPile() => _corePhysicalDrawFacadesOperations.EnsureDrawPile();
    void IProgramDeckCardsOperations.MoveCards(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason, Action<long, IReadOnlyList<CardMovementRecord>> afterPhysicalRecords) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords);
    SkillProgramStepOutcome IProgramDeckCardsOperations.AwaitMovements(long frameId, int ownerSeat) => _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frameId, ownerSeat);
}
