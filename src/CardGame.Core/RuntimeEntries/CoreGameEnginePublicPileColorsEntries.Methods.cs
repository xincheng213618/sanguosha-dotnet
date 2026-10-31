#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileColorsEntries
{
    internal bool CanActivatePublicPileColor(global::CardGame.Core.CharacterState owner, string skill, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().CanActivatePublicPileColor(owner, skill, features);
    internal bool CanOfferPublicPileColor(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferPublicPileColor(owner, candidate, trigger, context);
    internal global::CardGame.Core.ActionCardsDiscardedEvent? ColorActionFact(global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().ColorActionFact(c);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ColorDamageChoices(long frameId, int owner, string skill, string instance, global::CardGame.Core.SkillProgramEffect e) => _resolve().ColorDamageChoices(frameId, owner, skill, instance, e);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecutePublicPileColor(global::CardGame.Core.SkillProgramEffect e, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExecutePublicPileColor(e, frame);
    internal bool? PileColor(global::CardGame.Core.PublicPersistentPileSource s) => _resolve().PileColor(s);
    internal void RecordPublicPileColorOffer(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().RecordPublicPileColorOffer(candidate, context);
    internal void ResolvePublicPileColorChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePublicPileColorChoice(choice);
    internal bool ResumePublicPileColorPayment(long id) => _resolve().ResumePublicPileColorPayment(id);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PublicPersistentPileSource> SameGrantColorPiles(int seat, string sourceSkill, string consumerInstance) => _resolve().SameGrantColorPiles(seat, sourceSkill, consumerInstance);
    internal global::CardGame.Core.PromptChoice SelectAiPublicPileColorChoice(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiPublicPileColorChoice(p, f);
}
