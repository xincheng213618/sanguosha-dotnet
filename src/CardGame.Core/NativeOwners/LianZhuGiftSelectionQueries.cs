#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LianZhuGiftSelectionQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineLianZhuCountsEntries> getCoreGameEngineLianZhuCountsRuntime) : ILianZhuGiftSelectionQueries
{
    public bool CanDeliver(int ownerSeat, int recipientSeat, int cardId) => getCoreGameEngineLianZhuCountsRuntime().CanDeliverLianZhuGift(ownerSeat, recipientSeat, cardId);
}
