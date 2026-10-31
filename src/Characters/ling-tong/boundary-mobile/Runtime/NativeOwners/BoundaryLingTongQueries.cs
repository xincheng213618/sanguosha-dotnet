#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLingTongQueries(Func<RuntimeEntries.CharacterLingTongBoundaryMobileEntries> getCharacterLingTongBoundaryMobileRuntime) : IBoundaryLingTongQueries
{
    public IReadOnlyList<int> MovableSources(int ownerSeat) => Array.AsReadOnly(getCharacterLingTongBoundaryMobileRuntime().BoundaryLingTongEquipmentPairs(ownerSeat).Select(p => p.Source).Distinct().ToArray());
}
