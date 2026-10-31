#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreActualTurnProgressState : Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess, Contracts.ICoreActualTurnProgressPhaseAccess, Contracts.ICoreActualTurnProgressTurnNumberAccess, Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess, Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess, Contracts.ICoreActualTurnProgressCurrentSeatAccess, Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess, Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess, Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess, Contracts.ICoreActualTurnProgressCurrentSeatTurnProgressionAccess, Contracts.ICoreActualTurnProgressTurnProgressionAccess, Contracts.ICoreActualTurnProgressStartedTurnNumberAccess, Contracts.ICoreActualTurnProgressStartedAccess, Contracts.ICoreActualTurnProgressPhaseTurnNumberTurnProgressionAccess, Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedAccess, Contracts.ICoreActualTurnProgressPhaseTurnNumberAccess, Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess
{
    private int _currentSeat = default !;
    internal ref int ReferenceToCurrentSeat => ref _currentSeat;

    private TurnPhase _phase = TurnPhase.NotStarted;
    internal ref TurnPhase ReferenceToPhase => ref _phase;

    private bool _started = default !;
    internal ref bool ReferenceToStarted => ref _started;

    private int _turnNumber = default !;
    internal ref int ReferenceToTurnNumber => ref _turnNumber;

    private ActualTurnProgression _turnProgression = new(ActualTurnKind.Normal, 0, 0);
    internal ref ActualTurnProgression ReferenceToTurnProgression => ref _turnProgression;

    ref TurnPhase Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase => ref _phase;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat => ref _currentSeat;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber => ref _turnNumber;

    ref ActualTurnProgression Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref TurnPhase Contracts.ICoreActualTurnProgressPhaseAccess._phase => ref _phase;

    ref int Contracts.ICoreActualTurnProgressTurnNumberAccess._turnNumber => ref _turnNumber;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat => ref _currentSeat;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber => ref _turnNumber;

    ref TurnPhase Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase => ref _phase;

    ref TurnPhase Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess._phase => ref _phase;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess._currentSeat => ref _currentSeat;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber => ref _turnNumber;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat => ref _currentSeat;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatAccess._currentSeat => ref _currentSeat;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber => ref _turnNumber;

    ref ActualTurnProgression Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref TurnPhase Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase => ref _phase;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat => ref _currentSeat;

    ref bool Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._started => ref _started;

    ref ActualTurnProgression Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat => ref _currentSeat;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber => ref _turnNumber;

    ref int Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber => ref _turnNumber;

    ref ActualTurnProgression Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref ActualTurnProgression Contracts.ICoreActualTurnProgressCurrentSeatTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatTurnProgressionAccess._currentSeat => ref _currentSeat;

    ref ActualTurnProgression Contracts.ICoreActualTurnProgressTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref int Contracts.ICoreActualTurnProgressStartedTurnNumberAccess._turnNumber => ref _turnNumber;

    ref bool Contracts.ICoreActualTurnProgressStartedTurnNumberAccess._started => ref _started;

    ref bool Contracts.ICoreActualTurnProgressStartedAccess._started => ref _started;

    ref TurnPhase Contracts.ICoreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._phase => ref _phase;

    ref ActualTurnProgression Contracts.ICoreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression => ref _turnProgression;

    ref int Contracts.ICoreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber => ref _turnNumber;

    ref bool Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedAccess._started => ref _started;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedAccess._currentSeat => ref _currentSeat;

    ref TurnPhase Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedAccess._phase => ref _phase;

    ref int Contracts.ICoreActualTurnProgressPhaseTurnNumberAccess._turnNumber => ref _turnNumber;

    ref TurnPhase Contracts.ICoreActualTurnProgressPhaseTurnNumberAccess._phase => ref _phase;

    ref bool Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess._started => ref _started;

    ref int Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess._currentSeat => ref _currentSeat;
}
