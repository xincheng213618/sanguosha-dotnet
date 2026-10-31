#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess : IProgramRuntimeOperations
{
    ref int _fullDiscardPhaseSuitTurn { get; }

    ref readonly List<Suit> _fullDiscardPhaseSuits { get; }
}
