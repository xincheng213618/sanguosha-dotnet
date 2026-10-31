#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseEntityExchangeEntries
{
    internal CardActionContext? ActualRespondedAction(CardActionContext response) => _resolve().ActualRespondedAction(response);
    internal SkillProgramStepOutcome BeginPublicSuitEscalatingDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().BeginPublicSuitEscalatingDiscard(frame, effect);
    internal SkillProgramStepOutcome BeginResponseEntityExchange(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().BeginResponseEntityExchange(frame, effect);
    internal bool CanExchangeResponseEntities(int owner, string skill, string instance, SkillProgramEffect effect, CardActionContext response) => _resolve().CanExchangeResponseEntities(owner, skill, instance, effect, response);
    internal bool CanOfferResponseExchange(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferResponseExchange(candidate, trigger, context);
    internal void ClaimResponseEntities(ProgramSkillFrame frame, long action, int recipient, IReadOnlyList<int> ids, bool restricted) => _resolve().ClaimResponseEntities(frame, action, recipient, ids, restricted);
    internal Card EntityAtCurrentLocation(int id) => _resolve().EntityAtCurrentLocation(id);
    internal void FinishPublicSuitDiscard(ProgramSkillFrame frame) => _resolve().FinishPublicSuitDiscard(frame);
    internal IReadOnlyList<TurnProhibitedPhysicalCardsSnapshot>? GetResponseEntityRestrictionSnapshots() => _resolve().GetResponseEntityRestrictionSnapshots();
    internal IReadOnlyList<ProgramResponseExchangeStateSnapshot>? GetResponseExchangeStateSnapshots() => _resolve().GetResponseExchangeStateSnapshots();
    internal bool HasResponseEntityExchangeObservers() => _resolve().HasResponseEntityExchangeObservers();
    internal bool IsExchangedCardClaim(long actionId, int cardId) => _resolve().IsExchangedCardClaim(actionId, cardId);
    internal bool IsExchangedUseCardClaim(long frameId, int cardId) => _resolve().IsExchangedUseCardClaim(frameId, cardId);
    internal bool IsResponseEntityRestricted(int seat, int id) => _resolve().IsResponseEntityRestricted(seat, id);
    internal void PublishPublicSuitChoice(ProgramSkillFrame frame) => _resolve().PublishPublicSuitChoice(frame);
    internal void PublishPublicSuitDiscard(ProgramSkillFrame frame) => _resolve().PublishPublicSuitDiscard(frame);
    internal void ResolvePublicSuitChoice(PromptChoice choice) => _resolve().ResolvePublicSuitChoice(choice);
    internal void ResolvePublicSuitDiscard(PromptChoice choice) => _resolve().ResolvePublicSuitDiscard(choice);
    internal bool ResponseEntityExchangeFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().ResponseEntityExchangeFinalizedAfterCategoryTargetWindow(query);
    internal IEnumerable<IGameEvent> ResponseExchangeHistory() => _resolve().ResponseExchangeHistory();
    internal bool ResponseExchangeUpgraded(int owner, string skill, string instance, string state) => _resolve().ResponseExchangeUpgraded(owner, skill, instance, state);
    internal SkillProgramStepOutcome StartPublicSuitDraw(ProgramSkillFrame frame, Suit suit, int previous, int required) => _resolve().StartPublicSuitDraw(frame, suit, previous, required);
    internal bool TryResumeResponseEntityExchange(long id) => _resolve().TryResumeResponseEntityExchange(id);
}
