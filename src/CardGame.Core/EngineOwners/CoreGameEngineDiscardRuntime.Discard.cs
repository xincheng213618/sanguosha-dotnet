#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDiscardRuntime
{
    internal void RequestHumanDiscard(CharacterState player) => _manualDiscardOperations.Get().RequestHumanDiscard(player);
    internal void ContinueManualDiscard(long frameId) => _manualDiscardOperations.Get().ContinueManualDiscard(frameId);
    internal CommandResult SubmitDiscardCards(DiscardCardsCommand command) => _manualDiscardOperations.Get().SubmitDiscardCards(command);
}
