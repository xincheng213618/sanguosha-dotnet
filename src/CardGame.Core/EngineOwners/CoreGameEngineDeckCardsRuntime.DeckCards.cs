#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDeckCardsRuntime
{
    bool global::CardGame.Core.IProgramDeckCardsFacts.Alive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    long global::CardGame.Core.IProgramDeckCardsFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    IReadOnlyList<ProgramDeckCard> global::CardGame.Core.IProgramDeckCardsFacts.DrawPile => Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(card => new ProgramDeckCard(card.Id, card.Kind)).ToArray());

    IReadOnlyList<CardMovementRecord> global::CardGame.Core.IProgramDeckCardsFacts.MovementRange(long before, long after) => Array.AsReadOnly(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > before && move.Sequence <= after).ToArray());
    IEnumerable<DeckCardsObtainedEvent> global::CardGame.Core.IProgramDeckCardsFacts.ObtainedEvents => _coreProgramEventHistoryOperations.ProgramEventHistory<DeckCardsObtainedEvent>();

    bool global::CardGame.Core.IProgramDeckCardsFacts.IsRedirectedGain(CardMovementRecord movement, CardLocation requested) => _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(movement, requested);
    bool global::CardGame.Core.IProgramDeckCardsFacts.IsDestroyedMaterial(CardMovementRecord movement) => _characterZuoCiHappyGodOperations.IsHappyZuoCiDestroyedMaterial(movement);
    bool global::CardGame.Core.IProgramDeckCardsOperations.EnsureDrawPile() => _corePhysicalDrawFacadesOperations.EnsureDrawPile();
    void global::CardGame.Core.IProgramDeckCardsOperations.MoveCards(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason, Action<long, IReadOnlyList<CardMovementRecord>> afterPhysicalRecords) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords);
    SkillProgramStepOutcome global::CardGame.Core.IProgramDeckCardsOperations.AwaitMovements(long frameId, int ownerSeat) => _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frameId, ownerSeat);
}
