#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreMatchOutcomeState : Contracts.ICoreMatchOutcomeStatusWinnerAccess, Contracts.ICoreMatchOutcomeStatusAccess, Contracts.ICoreMatchOutcomeWinnerAccess, Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess, Contracts.ICoreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess, Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess, Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess, Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess
{
    private EngineStatus _status = EngineStatus.NotStarted;
    internal ref EngineStatus ReferenceToStatus => ref _status;

    private Winner _winner = Winner.None;
    internal ref Winner ReferenceToWinner => ref _winner;

    private string? _winnerFactionId = default !;
    internal ref string? ReferenceToWinnerFactionId => ref _winnerFactionId;

    private int? _winnerSeat = default !;
    internal ref int? ReferenceToWinnerSeat => ref _winnerSeat;

    private string? _winnerTeamId = default !;
    internal ref string? ReferenceToWinnerTeamId => ref _winnerTeamId;

    ref EngineStatus Contracts.ICoreMatchOutcomeStatusWinnerAccess._status => ref _status;

    ref Winner Contracts.ICoreMatchOutcomeStatusWinnerAccess._winner => ref _winner;

    ref EngineStatus Contracts.ICoreMatchOutcomeStatusAccess._status => ref _status;

    ref Winner Contracts.ICoreMatchOutcomeWinnerAccess._winner => ref _winner;

    ref EngineStatus Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._status => ref _status;

    ref Winner Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winner => ref _winner;

    ref string? Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winnerTeamId => ref _winnerTeamId;

    ref string? Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winnerFactionId => ref _winnerFactionId;

    ref int? Contracts.ICoreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess._winnerSeat => ref _winnerSeat;

    ref string? Contracts.ICoreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess._winnerTeamId => ref _winnerTeamId;

    ref Winner Contracts.ICoreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess._winner => ref _winner;

    ref Winner Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winner => ref _winner;

    ref string? Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerTeamId => ref _winnerTeamId;

    ref string? Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerFactionId => ref _winnerFactionId;

    ref int? Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerSeat => ref _winnerSeat;

    ref Winner Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winner => ref _winner;

    ref string? Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerTeamId => ref _winnerTeamId;

    ref string? Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerFactionId => ref _winnerFactionId;

    ref EngineStatus Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._status => ref _status;

    ref Winner Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winner => ref _winner;

    ref int? Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerSeat => ref _winnerSeat;

    ref string? Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerTeamId => ref _winnerTeamId;

    ref string? Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess._winnerFactionId => ref _winnerFactionId;
}
