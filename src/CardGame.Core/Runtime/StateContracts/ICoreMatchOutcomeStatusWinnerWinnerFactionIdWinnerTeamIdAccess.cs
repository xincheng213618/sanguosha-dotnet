#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess : IProgramRuntimeOperations
{
    ref EngineStatus _status { get; }

    ref Winner _winner { get; }

    ref string? _winnerTeamId { get; }

    ref string? _winnerFactionId { get; }
}
