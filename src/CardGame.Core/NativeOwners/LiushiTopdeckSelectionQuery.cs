#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiushiTopdeckSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineLiushiActivationCardsEntries> getCoreGameEngineLiushiActivationCardsRuntime) : ILiushiTopdeckSelectionQuery
{
    public bool CanTopdeck(int ownerSeat, int cardId) => getCoreGameEngineLiushiActivationCardsRuntime().CanTopdeckLiushiCard(ownerSeat, cardId);
}
