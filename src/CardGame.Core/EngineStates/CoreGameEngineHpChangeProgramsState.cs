#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineHpChangeProgramsState : Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess
{
    private List<HpChangeContext> _pendingHpChanges = [];
    internal ref readonly List<HpChangeContext> ReferenceToPendingHpChanges => ref _pendingHpChanges;

    ref readonly List<HpChangeContext> Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges => ref _pendingHpChanges;
}
