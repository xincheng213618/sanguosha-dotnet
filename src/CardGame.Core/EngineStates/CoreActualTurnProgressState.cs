#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreActualTurnProgressState : global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess
{
    private int _currentSeat = default !;
    internal ref int ReferenceToCurrentSeat => ref _currentSeat;

    private global::CardGame.Core.TurnPhase _phase = global::CardGame.Core.TurnPhase.NotStarted;
    internal ref global::CardGame.Core.TurnPhase ReferenceToPhase => ref _phase;

    private bool _started = default !;
    internal ref bool ReferenceToStarted => ref _started;

    private int _turnNumber = default !;
    internal ref int ReferenceToTurnNumber => ref _turnNumber;

    private global::CardGame.Core.ActualTurnProgression _turnProgression = new(global::CardGame.Core.ActualTurnKind.Normal, 0, 0);
    internal ref global::CardGame.Core.ActualTurnProgression ReferenceToTurnProgression => ref _turnProgression;

    ref global::CardGame.Core.TurnPhase global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase => ref _phase;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat => ref _currentSeat;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber => ref _turnNumber;

    ref global::CardGame.Core.ActualTurnProgression global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref global::CardGame.Core.TurnPhase global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess._phase => ref _phase;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess._turnNumber => ref _turnNumber;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat => ref _currentSeat;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber => ref _turnNumber;

    ref global::CardGame.Core.TurnPhase global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase => ref _phase;

    ref global::CardGame.Core.TurnPhase global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess._phase => ref _phase;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess._currentSeat => ref _currentSeat;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber => ref _turnNumber;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat => ref _currentSeat;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess._currentSeat => ref _currentSeat;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber => ref _turnNumber;

    ref global::CardGame.Core.ActualTurnProgression global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref global::CardGame.Core.TurnPhase global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase => ref _phase;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat => ref _currentSeat;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._started => ref _started;

    ref global::CardGame.Core.ActualTurnProgression global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat => ref _currentSeat;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber => ref _turnNumber;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber => ref _turnNumber;

    ref global::CardGame.Core.ActualTurnProgression global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref global::CardGame.Core.ActualTurnProgression global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnProgressionAccess._currentSeat => ref _currentSeat;

    ref global::CardGame.Core.ActualTurnProgression global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedTurnNumberAccess._turnNumber => ref _turnNumber;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedTurnNumberAccess._started => ref _started;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedAccess._started => ref _started;

    ref global::CardGame.Core.TurnPhase global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._phase => ref _phase;

    ref global::CardGame.Core.ActualTurnProgression global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber => ref _turnNumber;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedAccess._started => ref _started;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedAccess._currentSeat => ref _currentSeat;

    ref global::CardGame.Core.TurnPhase global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedAccess._phase => ref _phase;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberAccess._turnNumber => ref _turnNumber;

    ref global::CardGame.Core.TurnPhase global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberAccess._phase => ref _phase;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess._started => ref _started;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess._currentSeat => ref _currentSeat;
}
