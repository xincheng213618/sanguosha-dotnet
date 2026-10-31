#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _fullDiscardPhaseSuitTurn { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.Suit> _fullDiscardPhaseSuits { get; }
}
