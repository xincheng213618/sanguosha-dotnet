#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCardMovementJournalMovementSequenceAccess : IProgramRuntimeOperations
{
    ref int _movementSequence { get; }
}
