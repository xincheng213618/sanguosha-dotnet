#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchingRecastSelectionQueries(Func<RuntimeEntries.CoreGameEngineMatchingRecastsEntries> getCoreGameEngineMatchingRecastsRuntime) : IMatchingRecastSelectionQueries
{
    public IReadOnlyList<MatchingRecastMaterial> Materials(int ownerSeat) => Array.AsReadOnly(getCoreGameEngineMatchingRecastsRuntime().MatchingRecastMaterials(ownerSeat));
}
