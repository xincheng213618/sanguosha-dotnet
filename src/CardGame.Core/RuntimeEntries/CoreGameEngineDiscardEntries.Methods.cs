#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDiscardEntries
{
    internal void ContinueManualDiscard(long frameId) => _resolve().ContinueManualDiscard(frameId);
    internal void RequestHumanDiscard(CharacterState player) => _resolve().RequestHumanDiscard(player);
    internal CommandResult SubmitDiscardCards(DiscardCardsCommand command) => _resolve().SubmitDiscardCards(command);
}
