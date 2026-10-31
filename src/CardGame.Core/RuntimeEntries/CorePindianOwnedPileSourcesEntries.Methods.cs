#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePindianOwnedPileSourcesEntries
{
    internal void AssertOwnedPilePindianSource(global::CardGame.Core.PindianFrame frame) => _resolve().AssertOwnedPilePindianSource(frame);
    internal bool HasPaidOwnedPilePindianSource(global::CardGame.Core.PindianFrame frame) => _resolve().HasPaidOwnedPilePindianSource(frame);
}
