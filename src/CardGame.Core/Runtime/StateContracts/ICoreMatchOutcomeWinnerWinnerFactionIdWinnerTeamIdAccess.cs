#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess : IProgramRuntimeOperations
{
    ref Winner _winner { get; }

    ref string? _winnerTeamId { get; }

    ref string? _winnerFactionId { get; }
}
