#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess : IProgramRuntimeOperations
{
    ref readonly List<CharacterStateChangeContext> _pendingCharacterStateChanges { get; }
}
