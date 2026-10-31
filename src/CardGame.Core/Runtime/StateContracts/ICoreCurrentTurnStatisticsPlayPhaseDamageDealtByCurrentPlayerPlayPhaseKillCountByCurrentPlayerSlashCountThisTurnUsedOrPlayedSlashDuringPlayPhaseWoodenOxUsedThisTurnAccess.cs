#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _slashCountThisTurn { get; }

    ref bool _usedOrPlayedSlashDuringPlayPhase { get; }

    ref bool _woodenOxUsedThisTurn { get; }

    ref int _playPhaseKillCountByCurrentPlayer { get; }

    ref int _playPhaseDamageDealtByCurrentPlayer { get; }
}
