#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCardMovementJournalCardMovementsMovementSequenceAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _movementSequence { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementRecord> _cardMovements { get; }
}
