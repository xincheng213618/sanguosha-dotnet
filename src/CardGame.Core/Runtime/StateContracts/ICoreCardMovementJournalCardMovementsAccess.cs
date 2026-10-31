#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCardMovementJournalCardMovementsAccess : IProgramRuntimeOperations
{
    ref readonly List<CardMovementRecord> _cardMovements { get; }
}
