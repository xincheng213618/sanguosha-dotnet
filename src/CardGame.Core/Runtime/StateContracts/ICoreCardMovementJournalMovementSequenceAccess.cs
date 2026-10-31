#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCardMovementJournalMovementSequenceAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _movementSequence { get; }
}
