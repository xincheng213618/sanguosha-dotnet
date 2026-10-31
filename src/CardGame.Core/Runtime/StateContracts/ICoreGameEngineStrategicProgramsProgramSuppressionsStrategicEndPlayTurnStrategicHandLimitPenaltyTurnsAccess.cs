#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.GameEngine.ProgramSuppression> _programSuppressions { get; }

    ref readonly global::System.Collections.Generic.Dictionary<int, int> _strategicHandLimitPenaltyTurns { get; }

    ref int _strategicEndPlayTurn { get; }
}
