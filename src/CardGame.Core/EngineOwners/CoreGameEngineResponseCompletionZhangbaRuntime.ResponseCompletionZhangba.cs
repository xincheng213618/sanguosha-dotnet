#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineResponseCompletionZhangbaRuntime
{
    internal IReadOnlyList<CardActionCost>? CaptureZhangbaResponseCompletionCosts(CharacterState responder, IReadOnlyList<Card> pair) => !_characterZhaoYunMobileShiOperations.HasMobileShiCuifengCapability && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerWindow(SkillProgramTriggerWindow.CardResponseCompleted) ? null : Array.AsReadOnly(pair.Select(card => new CardActionCost(card.Id, card.Kind, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(responder, card), _coreActionDiscardColorsOperations.CapturePhysicalCardColor(responder.Seat, card))).ToArray());
}
