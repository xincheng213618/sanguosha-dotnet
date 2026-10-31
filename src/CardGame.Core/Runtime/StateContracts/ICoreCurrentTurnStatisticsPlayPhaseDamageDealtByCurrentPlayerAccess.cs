#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerAccess : IProgramRuntimeOperations
{
    ref int _playPhaseDamageDealtByCurrentPlayer { get; }
}
