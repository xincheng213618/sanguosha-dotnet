#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardSubsetViewsEntries
{
    internal global::CardGame.Core.CardSnapshot[] GetPrivateSubsetCards(int viewerSeat) => _resolve().GetPrivateSubsetCards(viewerSeat);
}
