#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchingRecastSelectionQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineMatchingRecastsEntries> getCoreGameEngineMatchingRecastsRuntime) : IMatchingRecastSelectionQueries
{
    public IReadOnlyList<MatchingRecastMaterial> Materials(int ownerSeat) => Array.AsReadOnly(getCoreGameEngineMatchingRecastsRuntime().MatchingRecastMaterials(ownerSeat));
}
