#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramPrivateDamageOfferOperationsEntries
{
    internal void AssertPrivateOfferDrafts(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertPrivateOfferDrafts(frame, paused);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DamageCardOfferChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DamageCardOfferChoices(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome DiscardProgramSelectedParticipantCards(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DiscardProgramSelectedParticipantCards(frame);
    internal global::CardGame.Core.CardSnapshot[] GetOfferedPrivatelyViewedCards(int viewerSeat) => _resolve().GetOfferedPrivatelyViewedCards(viewerSeat);
    internal bool IsDamageOfferPairBlocked(int owner, string skill, int source) => _resolve().IsDamageOfferPairBlocked(owner, skill, source);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferProgramBoundCardsForDamagePrevention(global::CardGame.Core.ProgramSkillFrame frame, string bind) => _resolve().OfferProgramBoundCardsForDamagePrevention(frame, bind);
    internal void PublishDamageCardOffer(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishDamageCardOffer(frame);
    internal void PublishSelectedParticipantDiscard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishSelectedParticipantDiscard(frame);
    internal void ResolveDamageCardOffer(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveDamageCardOffer(choice);
    internal void ResolveSelectedParticipantDiscard(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveSelectedParticipantDiscard(choice);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SelectedParticipantDiscardChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectedParticipantDiscardChoices(frame);
}
