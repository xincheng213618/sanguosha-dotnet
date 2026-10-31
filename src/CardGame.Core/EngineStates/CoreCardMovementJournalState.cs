#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreCardMovementJournalState : global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.CardMovementRecord> _cardMovements = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementRecord> ReferenceToCardMovements => ref _cardMovements;

    private int _movementSequence = default !;
    internal ref int ReferenceToMovementSequence => ref _movementSequence;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence => ref _movementSequence;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementRecord> global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements => ref _cardMovements;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementRecord> global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess._cardMovements => ref _cardMovements;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess._movementSequence => ref _movementSequence;
}
