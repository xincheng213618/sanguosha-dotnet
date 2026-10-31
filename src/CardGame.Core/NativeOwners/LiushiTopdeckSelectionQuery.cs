#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiushiTopdeckSelectionQuery(Func<RuntimeEntries.CoreGameEngineLiushiActivationCardsEntries> getCoreGameEngineLiushiActivationCardsRuntime) : ILiushiTopdeckSelectionQuery
{
    public bool CanTopdeck(int ownerSeat, int cardId) => getCoreGameEngineLiushiActivationCardsRuntime().CanTopdeckLiushiCard(ownerSeat, cardId);
}
