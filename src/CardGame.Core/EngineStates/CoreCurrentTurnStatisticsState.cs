#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreCurrentTurnStatisticsState : global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnWoodenOxUsedThisTurnAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseKillCountByCurrentPlayerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsWoodenOxUsedThisTurnAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess
{
    private int _playPhaseDamageDealtByCurrentPlayer = default !;
    internal ref int ReferenceToPlayPhaseDamageDealtByCurrentPlayer => ref _playPhaseDamageDealtByCurrentPlayer;

    private int _playPhaseKillCountByCurrentPlayer = default !;
    internal ref int ReferenceToPlayPhaseKillCountByCurrentPlayer => ref _playPhaseKillCountByCurrentPlayer;

    private int _slashCountThisTurn = default !;
    internal ref int ReferenceToSlashCountThisTurn => ref _slashCountThisTurn;

    private bool _usedOrPlayedSlashDuringPlayPhase = default !;
    internal ref bool ReferenceToUsedOrPlayedSlashDuringPlayPhase => ref _usedOrPlayedSlashDuringPlayPhase;

    private bool _woodenOxUsedThisTurn = default !;
    internal ref bool ReferenceToWoodenOxUsedThisTurn => ref _woodenOxUsedThisTurn;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerAccess._playPhaseDamageDealtByCurrentPlayer => ref _playPhaseDamageDealtByCurrentPlayer;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnAccess._slashCountThisTurn => ref _slashCountThisTurn;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnWoodenOxUsedThisTurnAccess._slashCountThisTurn => ref _slashCountThisTurn;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnWoodenOxUsedThisTurnAccess._woodenOxUsedThisTurn => ref _woodenOxUsedThisTurn;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess._usedOrPlayedSlashDuringPlayPhase => ref _usedOrPlayedSlashDuringPlayPhase;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseKillCountByCurrentPlayerAccess._playPhaseKillCountByCurrentPlayer => ref _playPhaseKillCountByCurrentPlayer;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsWoodenOxUsedThisTurnAccess._woodenOxUsedThisTurn => ref _woodenOxUsedThisTurn;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess._slashCountThisTurn => ref _slashCountThisTurn;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess._usedOrPlayedSlashDuringPlayPhase => ref _usedOrPlayedSlashDuringPlayPhase;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess._woodenOxUsedThisTurn => ref _woodenOxUsedThisTurn;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess._playPhaseKillCountByCurrentPlayer => ref _playPhaseKillCountByCurrentPlayer;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess._playPhaseDamageDealtByCurrentPlayer => ref _playPhaseDamageDealtByCurrentPlayer;
}
