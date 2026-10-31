#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _playPhaseDamageDealtByCurrentPlayer { get; }
}
