#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineHpChangeProgramsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.HpChangeContext> _pendingHpChanges = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.HpChangeContext> ReferenceToPendingHpChanges => ref _pendingHpChanges;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.HpChangeContext> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges => ref _pendingHpChanges;
}
