#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDiscardEntries
{
    internal void AssertDiscardPromptInvariant() => _resolve().AssertDiscardPromptInvariant();
    internal void ContinueManualDiscard(long frameId) => _resolve().ContinueManualDiscard(frameId);
    internal bool HasManualDiscardBoundaryFrame() => _resolve().HasManualDiscardBoundaryFrame();
    internal void RequestHumanDiscard(global::CardGame.Core.CharacterState player) => _resolve().RequestHumanDiscard(player);
    internal global::CardGame.Core.CommandResult SubmitDiscardCards(global::CardGame.Core.DiscardCardsCommand command) => _resolve().SubmitDiscardCards(command);
}
