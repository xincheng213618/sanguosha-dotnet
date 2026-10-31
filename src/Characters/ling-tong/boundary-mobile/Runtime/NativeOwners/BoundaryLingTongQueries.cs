#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLingTongQueries(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLingTongBoundaryMobileRuntime>> getCharacterLingTongBoundaryMobileRuntime) : IBoundaryLingTongQueries
{
    public IReadOnlyList<int> MovableSources(int ownerSeat) => Array.AsReadOnly(getCharacterLingTongBoundaryMobileRuntime().Value.BoundaryLingTongEquipmentPairs(ownerSeat).Select(p => p.Source).Distinct().ToArray());
}
