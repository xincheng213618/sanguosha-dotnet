#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess : IProgramRuntimeOperations
{
    ref Winner _winner { get; }

    ref string? _winnerTeamId { get; }

    ref string? _winnerFactionId { get; }

    ref int? _winnerSeat { get; }
}
