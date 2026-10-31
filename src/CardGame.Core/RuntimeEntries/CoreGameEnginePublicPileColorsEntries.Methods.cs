#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileColorsEntries
{
    internal bool CanActivatePublicPileColor(CharacterState owner, string skill, ProgramInstructionFeatures features) => _resolve().CanActivatePublicPileColor(owner, skill, features);
    internal bool CanOfferPublicPileColor(CharacterState owner, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferPublicPileColor(owner, candidate, trigger, context);
    internal ActionCardsDiscardedEvent? ColorActionFact(ProgramSkillWindowContext c) => _resolve().ColorActionFact(c);
    internal IReadOnlyList<PromptChoice> ColorDamageChoices(long frameId, int owner, string skill, string instance, SkillProgramEffect e) => _resolve().ColorDamageChoices(frameId, owner, skill, instance, e);
    internal SkillProgramStepOutcome ExecutePublicPileColor(SkillProgramEffect e, ProgramSkillFrame frame) => _resolve().ExecutePublicPileColor(e, frame);
    internal bool? PileColor(PublicPersistentPileSource s) => _resolve().PileColor(s);
    internal void RecordPublicPileColorOffer(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().RecordPublicPileColorOffer(candidate, context);
    internal void ResolvePublicPileColorChoice(PromptChoice choice) => _resolve().ResolvePublicPileColorChoice(choice);
    internal bool ResumePublicPileColorPayment(long id) => _resolve().ResumePublicPileColorPayment(id);
    internal IEnumerable<PublicPersistentPileSource> SameGrantColorPiles(int seat, string sourceSkill, string consumerInstance) => _resolve().SameGrantColorPiles(seat, sourceSkill, consumerInstance);
    internal PromptChoice SelectAiPublicPileColorChoice(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiPublicPileColorChoice(p, f);
}
