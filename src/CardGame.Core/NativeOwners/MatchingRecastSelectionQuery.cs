#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchingRecastSelectionQuery(Func<RuntimeEntries.CoreGameEngineMatchingRecastsEntries> getCoreGameEngineMatchingRecastsRuntime) : IMatchingRecastSelectionQuery
{
    public bool IsValid(int ownerSeat, IReadOnlyList<int> cards) => getCoreGameEngineMatchingRecastsRuntime().ValidMatchingRecastSelection(ownerSeat, cards);
}
