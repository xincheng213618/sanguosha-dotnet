#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineActualPlayPhaseCardUsesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess
{
    private global::System.Collections.Generic.HashSet<long> _actualPlayPhaseUseActions = [];
    internal ref readonly global::System.Collections.Generic.HashSet<long> ReferenceToActualPlayPhaseUseActions => ref _actualPlayPhaseUseActions;

    ref readonly global::System.Collections.Generic.HashSet<long> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess._actualPlayPhaseUseActions => ref _actualPlayPhaseUseActions;
}
