#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineModeMatchLimitsEntries
{
    internal void RetirePendingMatchAtModeLimit(string? winningTeamId, global::System.Func<long, global::CardGame.Core.ResolutionFrameKind, global::CardGame.Core.IGameEvent> cancellationFact, string processingReason, string judgmentReason) => _resolve().RetirePendingMatchAtModeLimit(winningTeamId, cancellationFact, processingReason, judgmentReason);
}
