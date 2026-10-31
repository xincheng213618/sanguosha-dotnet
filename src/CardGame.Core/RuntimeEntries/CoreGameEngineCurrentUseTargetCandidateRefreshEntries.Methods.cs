#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentUseTargetCandidateRefreshEntries
{
    internal CardUseFrame NormalizeImplicitDrawTwoUseTarget(CardUseFrame use) => _resolve().NormalizeImplicitDrawTwoUseTarget(use);
    internal void RefreshCurrentUseAddedTargetCandidates(CardUseFrame use, IReadOnlyList<int> addedMainTargets) => _resolve().RefreshCurrentUseAddedTargetCandidates(use, addedMainTargets);
}
