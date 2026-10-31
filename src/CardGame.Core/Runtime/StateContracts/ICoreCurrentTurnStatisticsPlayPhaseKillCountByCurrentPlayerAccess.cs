#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCurrentTurnStatisticsPlayPhaseKillCountByCurrentPlayerAccess : IProgramRuntimeOperations
{
    ref int _playPhaseKillCountByCurrentPlayer { get; }
}
