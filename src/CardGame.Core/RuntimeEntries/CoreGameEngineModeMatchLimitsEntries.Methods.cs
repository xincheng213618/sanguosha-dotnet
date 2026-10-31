#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineModeMatchLimitsEntries
{
    internal void RetirePendingMatchAtModeLimit(string? winningTeamId, Func<long, ResolutionFrameKind, IGameEvent> cancellationFact, string processingReason, string judgmentReason) => _resolve().RetirePendingMatchAtModeLimit(winningTeamId, cancellationFact, processingReason, judgmentReason);
}
