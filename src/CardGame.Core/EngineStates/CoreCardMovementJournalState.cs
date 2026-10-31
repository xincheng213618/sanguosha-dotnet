#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreCardMovementJournalState : Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess, Contracts.ICoreCardMovementJournalCardMovementsAccess, Contracts.ICoreCardMovementJournalMovementSequenceAccess
{
    private List<CardMovementRecord> _cardMovements = [];
    internal ref readonly List<CardMovementRecord> ReferenceToCardMovements => ref _cardMovements;

    private int _movementSequence = default !;
    internal ref int ReferenceToMovementSequence => ref _movementSequence;

    ref int Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence => ref _movementSequence;

    ref readonly List<CardMovementRecord> Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements => ref _cardMovements;

    ref readonly List<CardMovementRecord> Contracts.ICoreCardMovementJournalCardMovementsAccess._cardMovements => ref _cardMovements;

    ref int Contracts.ICoreCardMovementJournalMovementSequenceAccess._movementSequence => ref _movementSequence;
}
