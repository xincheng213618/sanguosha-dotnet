#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineCharacterStateProgramsState : Contracts.ICoreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess
{
    private List<CharacterStateChangeContext> _pendingCharacterStateChanges = [];
    internal ref readonly List<CharacterStateChangeContext> ReferenceToPendingCharacterStateChanges => ref _pendingCharacterStateChanges;

    ref readonly List<CharacterStateChangeContext> Contracts.ICoreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess._pendingCharacterStateChanges => ref _pendingCharacterStateChanges;
}
