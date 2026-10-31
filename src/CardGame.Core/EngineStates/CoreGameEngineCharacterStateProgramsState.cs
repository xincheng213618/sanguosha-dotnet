#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineCharacterStateProgramsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.CharacterStateChangeContext> _pendingCharacterStateChanges = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CharacterStateChangeContext> ReferenceToPendingCharacterStateChanges => ref _pendingCharacterStateChanges;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CharacterStateChangeContext> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess._pendingCharacterStateChanges => ref _pendingCharacterStateChanges;
}
