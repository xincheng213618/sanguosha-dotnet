#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCardMovementJournalCardMovementsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementRecord> _cardMovements { get; }
}
