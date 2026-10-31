#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineStrategicProgramsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineStrategicProgramsStrategicEndPlayTurnAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramSuppression> _programSuppressions = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramSuppression> ReferenceToProgramSuppressions => ref _programSuppressions;

    private int _strategicEndPlayTurn = -1;
    internal ref int ReferenceToStrategicEndPlayTurn => ref _strategicEndPlayTurn;

    private global::System.Collections.Generic.Dictionary<int, int> _strategicHandLimitPenaltyTurns = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, int> ReferenceToStrategicHandLimitPenaltyTurns => ref _strategicHandLimitPenaltyTurns;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineStrategicProgramsStrategicEndPlayTurnAccess._strategicEndPlayTurn => ref _strategicEndPlayTurn;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramSuppression> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._programSuppressions => ref _programSuppressions;

    ref readonly global::System.Collections.Generic.Dictionary<int, int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._strategicHandLimitPenaltyTurns => ref _strategicHandLimitPenaltyTurns;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess._strategicEndPlayTurn => ref _strategicEndPlayTurn;
}
