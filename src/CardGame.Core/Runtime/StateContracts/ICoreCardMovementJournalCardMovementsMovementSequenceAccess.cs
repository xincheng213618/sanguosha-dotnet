#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCardMovementJournalCardMovementsMovementSequenceAccess : IProgramRuntimeOperations
{
    ref int _movementSequence { get; }

    ref readonly List<CardMovementRecord> _cardMovements { get; }
}
