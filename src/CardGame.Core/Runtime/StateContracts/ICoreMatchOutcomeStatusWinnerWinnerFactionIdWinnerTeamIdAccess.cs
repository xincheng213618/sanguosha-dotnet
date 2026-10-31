#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.EngineStatus _status { get; }

    ref global::CardGame.Core.Winner _winner { get; }

    ref string? _winnerTeamId { get; }

    ref string? _winnerFactionId { get; }
}
