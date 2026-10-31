#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramPrivateDamageOfferOperationsEntries
{
    internal IReadOnlyList<PromptChoice> DamageCardOfferChoices(ProgramSkillFrame frame) => _resolve().DamageCardOfferChoices(frame);
    internal SkillProgramStepOutcome DiscardProgramSelectedParticipantCards(ProgramSkillFrame frame) => _resolve().DiscardProgramSelectedParticipantCards(frame);
    internal CardSnapshot[] GetOfferedPrivatelyViewedCards(int viewerSeat) => _resolve().GetOfferedPrivatelyViewedCards(viewerSeat);
    internal bool IsDamageOfferPairBlocked(int owner, string skill, int source) => _resolve().IsDamageOfferPairBlocked(owner, skill, source);
    internal SkillProgramStepOutcome OfferProgramBoundCardsForDamagePrevention(ProgramSkillFrame frame, string bind) => _resolve().OfferProgramBoundCardsForDamagePrevention(frame, bind);
    internal void PublishDamageCardOffer(ProgramSkillFrame frame) => _resolve().PublishDamageCardOffer(frame);
    internal void PublishSelectedParticipantDiscard(ProgramSkillFrame frame) => _resolve().PublishSelectedParticipantDiscard(frame);
    internal void ResolveDamageCardOffer(PromptChoice choice) => _resolve().ResolveDamageCardOffer(choice);
    internal void ResolveSelectedParticipantDiscard(PromptChoice choice) => _resolve().ResolveSelectedParticipantDiscard(choice);
    internal IReadOnlyList<PromptChoice> SelectedParticipantDiscardChoices(ProgramSkillFrame frame) => _resolve().SelectedParticipantDiscardChoices(frame);
}
