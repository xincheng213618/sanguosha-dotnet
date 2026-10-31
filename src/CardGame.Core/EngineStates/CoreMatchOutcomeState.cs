#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreMatchOutcomeState : global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess
{
    private global::CardGame.Core.EngineStatus _status = global::CardGame.Core.EngineStatus.NotStarted;
    internal ref global::CardGame.Core.EngineStatus ReferenceToStatus => ref _status;

    private global::CardGame.Core.Winner _winner = global::CardGame.Core.Winner.None;
    internal ref global::CardGame.Core.Winner ReferenceToWinner => ref _winner;

    private string? _winnerFactionId = default !;
    internal ref string? ReferenceToWinnerFactionId => ref _winnerFactionId;

    private int? _winnerSeat = default !;
    internal ref int? ReferenceToWinnerSeat => ref _winnerSeat;

    private string? _winnerTeamId = default !;
    internal ref string? ReferenceToWinnerTeamId => ref _winnerTeamId;

    ref global::CardGame.Core.EngineStatus global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess._status => ref _status;

    ref global::CardGame.Core.Winner global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess._winner => ref _winner;

    ref global::CardGame.Core.EngineStatus global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess._status => ref _status;

    ref global::CardGame.Core.Winner global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess._winner => ref _winner;

    ref global::CardGame.Core.EngineStatus global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._status => ref _status;

    ref global::CardGame.Core.Winner global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winner => ref _winner;

    ref string? global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winnerTeamId => ref _winnerTeamId;

    ref string? global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winnerFactionId => ref _winnerFactionId;

    ref int? global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess._winnerSeat => ref _winnerSeat;

    ref string? global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess._winnerTeamId => ref _winnerTeamId;

    ref global::CardGame.Core.Winner global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess._winner => ref _winner;

    ref global::CardGame.Core.Winner global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winner => ref _winner;

    ref string? global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerTeamId => ref _winnerTeamId;

    ref string? global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerFactionId => ref _winnerFactionId;

    ref int? global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerSeat => ref _winnerSeat;

    ref global::CardGame.Core.Winner global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winner => ref _winner;

    ref string? global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerTeamId => ref _winnerTeamId;

    ref string? global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerFactionId => ref _winnerFactionId;

    ref global::CardGame.Core.EngineStatus global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._status => ref _status;

    ref global::CardGame.Core.Winner global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winner => ref _winner;

    ref int? global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerSeat => ref _winnerSeat;

    ref string? global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerTeamId => ref _winnerTeamId;

    ref string? global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerFactionId => ref _winnerFactionId;
}
