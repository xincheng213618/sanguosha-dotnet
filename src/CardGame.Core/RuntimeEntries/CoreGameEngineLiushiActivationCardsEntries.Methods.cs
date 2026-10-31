#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLiushiActivationCardsEntries
{
    internal bool CanTopdeckLiushiCard(int ownerSeat, int cardId) => _resolve().CanTopdeckLiushiCard(ownerSeat, cardId);
}
