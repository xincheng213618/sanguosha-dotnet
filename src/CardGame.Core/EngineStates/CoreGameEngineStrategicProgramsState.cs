#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineStrategicProgramsState : Contracts.ICoreGameEngineStrategicProgramsStrategicEndPlayTurnAccess, Contracts.ICoreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess
{
    private List<GameEngine.ProgramSuppression> _programSuppressions = [];
    internal ref readonly List<GameEngine.ProgramSuppression> ReferenceToProgramSuppressions => ref _programSuppressions;

    private int _strategicEndPlayTurn = -1;
    internal ref int ReferenceToStrategicEndPlayTurn => ref _strategicEndPlayTurn;

    private Dictionary<int, int> _strategicHandLimitPenaltyTurns = [];
    internal ref readonly Dictionary<int, int> ReferenceToStrategicHandLimitPenaltyTurns => ref _strategicHandLimitPenaltyTurns;

    ref int Contracts.ICoreGameEngineStrategicProgramsStrategicEndPlayTurnAccess._strategicEndPlayTurn => ref _strategicEndPlayTurn;

    ref readonly List<GameEngine.ProgramSuppression> Contracts.ICoreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._programSuppressions => ref _programSuppressions;

    ref readonly Dictionary<int, int> Contracts.ICoreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._strategicHandLimitPenaltyTurns => ref _strategicHandLimitPenaltyTurns;

    ref int Contracts.ICoreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._strategicEndPlayTurn => ref _strategicEndPlayTurn;
}
