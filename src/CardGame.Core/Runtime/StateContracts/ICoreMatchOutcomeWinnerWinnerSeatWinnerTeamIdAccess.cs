#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess : IProgramRuntimeOperations
{
    ref int? _winnerSeat { get; }

    ref string? _winnerTeamId { get; }

    ref Winner _winner { get; }
}
