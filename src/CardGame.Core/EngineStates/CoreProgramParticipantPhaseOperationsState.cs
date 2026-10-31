#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreProgramParticipantPhaseOperationsState : Contracts.ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess
{
    private int _fullDiscardPhaseSuitTurn = -1;
    internal ref int ReferenceToFullDiscardPhaseSuitTurn => ref _fullDiscardPhaseSuitTurn;

    private List<Suit> _fullDiscardPhaseSuits = [];
    internal ref readonly List<Suit> ReferenceToFullDiscardPhaseSuits => ref _fullDiscardPhaseSuits;

    ref int Contracts.ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuitTurn => ref _fullDiscardPhaseSuitTurn;

    ref readonly List<Suit> Contracts.ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuits => ref _fullDiscardPhaseSuits;
}
