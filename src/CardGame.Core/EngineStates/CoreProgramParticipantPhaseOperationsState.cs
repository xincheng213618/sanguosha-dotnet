#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreProgramParticipantPhaseOperationsState : global::CardGame.Core.EngineStates.Contracts.ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess
{
    private int _fullDiscardPhaseSuitTurn = -1;
    internal ref int ReferenceToFullDiscardPhaseSuitTurn => ref _fullDiscardPhaseSuitTurn;

    private global::System.Collections.Generic.List<global::CardGame.Core.Suit> _fullDiscardPhaseSuits = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.Suit> ReferenceToFullDiscardPhaseSuits => ref _fullDiscardPhaseSuits;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuitTurn => ref _fullDiscardPhaseSuitTurn;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.Suit> global::CardGame.Core.EngineStates.Contracts.ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuits => ref _fullDiscardPhaseSuits;
}
