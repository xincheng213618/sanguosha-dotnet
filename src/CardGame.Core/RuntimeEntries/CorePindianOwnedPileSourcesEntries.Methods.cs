#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePindianOwnedPileSourcesEntries
{
    internal void AssertOwnedPilePindianSource(PindianFrame frame) => _resolve().AssertOwnedPilePindianSource(frame);
    internal bool HasPaidOwnedPilePindianSource(PindianFrame frame) => _resolve().HasPaidOwnedPilePindianSource(frame);
}
