#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int? _winnerSeat { get; }

    ref string? _winnerTeamId { get; }

    ref global::CardGame.Core.Winner _winner { get; }
}
