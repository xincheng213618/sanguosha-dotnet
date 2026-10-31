#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCurrentTurnStatisticsSlashCountThisTurnAccess : IProgramRuntimeOperations
{
    ref int _slashCountThisTurn { get; }
}
