#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentUseTargetCandidateRefreshEntries
{
    internal global::CardGame.Core.CardUseFrame NormalizeImplicitDrawTwoUseTarget(global::CardGame.Core.CardUseFrame use) => _resolve().NormalizeImplicitDrawTwoUseTarget(use);
    internal void RefreshCurrentUseAddedTargetCandidates(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> addedMainTargets) => _resolve().RefreshCurrentUseAddedTargetCandidates(use, addedMainTargets);
}
