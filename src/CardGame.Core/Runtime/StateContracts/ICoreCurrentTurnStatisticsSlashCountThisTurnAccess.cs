#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCurrentTurnStatisticsSlashCountThisTurnAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _slashCountThisTurn { get; }
}
