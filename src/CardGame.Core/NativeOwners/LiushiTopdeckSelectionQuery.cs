#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiushiTopdeckSelectionQuery(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineLiushiActivationCardsRuntime>> getCoreGameEngineLiushiActivationCardsRuntime) : ILiushiTopdeckSelectionQuery
{
    public bool CanTopdeck(int ownerSeat, int cardId) => getCoreGameEngineLiushiActivationCardsRuntime().Value.CanTopdeckLiushiCard(ownerSeat, cardId);
}
