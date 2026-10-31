#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineStrategicProgramsStrategicEndPlayTurnAccess : IProgramRuntimeOperations
{
    ref int _strategicEndPlayTurn { get; }
}
