#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLingTongQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLingTongBoundaryMobileEntries> getCharacterLingTongBoundaryMobileRuntime) : IBoundaryLingTongQueries
{
    public IReadOnlyList<int> MovableSources(int ownerSeat) => Array.AsReadOnly(getCharacterLingTongBoundaryMobileRuntime().BoundaryLingTongEquipmentPairs(ownerSeat).Select(p => p.Source).Distinct().ToArray());
}
