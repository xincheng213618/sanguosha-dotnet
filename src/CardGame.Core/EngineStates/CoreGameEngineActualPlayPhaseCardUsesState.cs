#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineActualPlayPhaseCardUsesState : Contracts.ICoreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess
{
    private HashSet<long> _actualPlayPhaseUseActions = [];
    internal ref readonly HashSet<long> ReferenceToActualPlayPhaseUseActions => ref _actualPlayPhaseUseActions;

    ref readonly HashSet<long> Contracts.ICoreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess._actualPlayPhaseUseActions => ref _actualPlayPhaseUseActions;
}
