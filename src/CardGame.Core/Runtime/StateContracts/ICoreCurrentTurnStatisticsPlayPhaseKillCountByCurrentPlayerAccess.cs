#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCurrentTurnStatisticsPlayPhaseKillCountByCurrentPlayerAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _playPhaseKillCountByCurrentPlayer { get; }
}
