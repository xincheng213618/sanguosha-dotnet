#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess : IProgramRuntimeOperations
{
    ref readonly List<GameEngine.ProgramSuppression> _programSuppressions { get; }

    ref readonly Dictionary<int, int> _strategicHandLimitPenaltyTurns { get; }

    ref int _strategicEndPlayTurn { get; }
}
