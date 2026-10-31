#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateCardOffersEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDeferredPrivateOffer(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginDeferredPrivateOffer(supplied);
    internal bool CanContinueIssuedPrivateOffer(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedPrivateOffer(f);
    internal bool CanRunDeferredPrivateOffer(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunDeferredPrivateOffer(candidate, context);
    internal void CommitPrivateOfferSegment(long id) => _resolve().CommitPrivateOfferSegment(id);
    internal global::CardGame.Core.CardLocation? CurrentDeferredPrivateOffer(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CurrentDeferredPrivateOffer(candidate, context);
    internal bool? DecideIssuedTrigger700(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger700(candidate, context);
    internal global::CardGame.Core.CardLocation? DeferredPrivateOfferParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DeferredPrivateOfferParent(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> DeferredPrivateOfferViewedCards(int viewer) => _resolve().DeferredPrivateOfferViewedCards(viewer);
    internal global::CardGame.Core.SkillProgramStepOutcome DepositPrivateCardOffer(global::CardGame.Core.ProgramSkillFrame supplied, string bind, string resolver) => _resolve().DepositPrivateCardOffer(supplied, bind, resolver);
    internal bool IsDeferredPrivateOfferResolver(global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().IsDeferredPrivateOfferResolver(candidate);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PrivateOfferChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PrivateOfferChoices(f);
    internal void PublishPrivateOffer(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishPrivateOffer(f);
    internal void ResolveDeferredPrivateOfferChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveDeferredPrivateOfferChoice(selected);
    internal bool TryBeginDeferredPrivateOffers(global::CardGame.Core.CharacterState target) => _resolve().TryBeginDeferredPrivateOffers(target);
    internal bool ValidPrivateOfferDeposit(global::CardGame.Core.CardLocation location, scoped out int cardId) => _resolve().ValidPrivateOfferDeposit(location, out cardId);
    internal long PrivateOfferSequence { get => _resolve().PrivateOfferSequence; }
}
