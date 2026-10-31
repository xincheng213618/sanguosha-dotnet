#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchingRecastSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineMatchingRecastsEntries> getCoreGameEngineMatchingRecastsRuntime) : IMatchingRecastSelectionQuery
{
    public bool IsValid(int ownerSeat, IReadOnlyList<int> cards) => getCoreGameEngineMatchingRecastsRuntime().ValidMatchingRecastSelection(ownerSeat, cards);
}
