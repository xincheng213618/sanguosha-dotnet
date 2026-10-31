#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseEntityExchangeEntries
{
    internal global::CardGame.Core.CardActionContext? ActualRespondedAction(global::CardGame.Core.CardActionContext response) => _resolve().ActualRespondedAction(response);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPublicSuitEscalatingDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginPublicSuitEscalatingDiscard(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginResponseEntityExchange(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginResponseEntityExchange(frame, effect);
    internal bool CanExchangeResponseEntities(int owner, string skill, string instance, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.CardActionContext response) => _resolve().CanExchangeResponseEntities(owner, skill, instance, effect, response);
    internal bool CanOfferResponseExchange(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferResponseExchange(candidate, trigger, context);
    internal void ClaimResponseEntities(global::CardGame.Core.ProgramSkillFrame frame, long action, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, bool restricted) => _resolve().ClaimResponseEntities(frame, action, recipient, ids, restricted);
    internal global::CardGame.Core.Card EntityAtCurrentLocation(int id) => _resolve().EntityAtCurrentLocation(id);
    internal void FinishPublicSuitDiscard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishPublicSuitDiscard(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnProhibitedPhysicalCardsSnapshot>? GetResponseEntityRestrictionSnapshots() => _resolve().GetResponseEntityRestrictionSnapshots();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramResponseExchangeStateSnapshot>? GetResponseExchangeStateSnapshots() => _resolve().GetResponseExchangeStateSnapshots();
    internal bool HasResponseEntityExchangeObservers() => _resolve().HasResponseEntityExchangeObservers();
    internal bool IsExchangedCardClaim(long actionId, int cardId) => _resolve().IsExchangedCardClaim(actionId, cardId);
    internal bool IsExchangedUseCardClaim(long frameId, int cardId) => _resolve().IsExchangedUseCardClaim(frameId, cardId);
    internal bool IsResponseEntityRestricted(int seat, int id) => _resolve().IsResponseEntityRestricted(seat, id);
    internal void PublishPublicSuitChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishPublicSuitChoice(frame);
    internal void PublishPublicSuitDiscard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishPublicSuitDiscard(frame);
    internal void ResolvePublicSuitChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePublicSuitChoice(choice);
    internal void ResolvePublicSuitDiscard(global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePublicSuitDiscard(choice);
    internal bool ResponseEntityExchangeFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().ResponseEntityExchangeFinalizedAfterCategoryTargetWindow(query);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> ResponseExchangeHistory() => _resolve().ResponseExchangeHistory();
    internal bool ResponseExchangeUpgraded(int owner, string skill, string instance, string state) => _resolve().ResponseExchangeUpgraded(owner, skill, instance, state);
    internal global::CardGame.Core.SkillProgramStepOutcome StartPublicSuitDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.Suit suit, int previous, int required) => _resolve().StartPublicSuitDraw(frame, suit, previous, required);
    internal bool TryResumeResponseEntityExchange(long id) => _resolve().TryResumeResponseEntityExchange(id);
}
