#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateCardOffersEntries
{
    internal SkillProgramStepOutcome BeginDeferredPrivateOffer(ProgramSkillFrame supplied) => _resolve().BeginDeferredPrivateOffer(supplied);
    internal bool CanContinueIssuedPrivateOffer(ProgramSkillFrame f) => _resolve().CanContinueIssuedPrivateOffer(f);
    internal bool CanRunDeferredPrivateOffer(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanRunDeferredPrivateOffer(candidate, context);
    internal void CommitPrivateOfferSegment(long id) => _resolve().CommitPrivateOfferSegment(id);
    internal CardLocation? CurrentDeferredPrivateOffer(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CurrentDeferredPrivateOffer(candidate, context);
    internal bool? DecideIssuedTrigger700(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger700(candidate, context);
    internal CardLocation? DeferredPrivateOfferParent(ProgramSkillFrame f) => _resolve().DeferredPrivateOfferParent(f);
    internal IReadOnlyList<CardSnapshot> DeferredPrivateOfferViewedCards(int viewer) => _resolve().DeferredPrivateOfferViewedCards(viewer);
    internal SkillProgramStepOutcome DepositPrivateCardOffer(ProgramSkillFrame supplied, string bind, string resolver) => _resolve().DepositPrivateCardOffer(supplied, bind, resolver);
    internal bool IsDeferredPrivateOfferResolver(ProgramTriggerCandidate candidate) => _resolve().IsDeferredPrivateOfferResolver(candidate);
    internal IReadOnlyList<PromptChoice> PrivateOfferChoices(ProgramSkillFrame f) => _resolve().PrivateOfferChoices(f);
    internal void PublishPrivateOffer(ProgramSkillFrame f) => _resolve().PublishPrivateOffer(f);
    internal void ResolveDeferredPrivateOfferChoice(PromptChoice selected) => _resolve().ResolveDeferredPrivateOfferChoice(selected);
    internal bool TryBeginDeferredPrivateOffers(CharacterState target) => _resolve().TryBeginDeferredPrivateOffers(target);
    internal bool ValidPrivateOfferDeposit(CardLocation location, scoped out int cardId) => _resolve().ValidPrivateOfferDeposit(location, out cardId);
    internal long PrivateOfferSequence { get => _resolve().PrivateOfferSequence; }
}
