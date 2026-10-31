#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardSubsetViewsEntries
{
    internal CardSnapshot[] GetPrivateSubsetCards(int viewerSeat) => _resolve().GetPrivateSubsetCards(viewerSeat);
}
