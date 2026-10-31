#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRicePileEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginRicePile(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginRicePile(effect, supplied);
    internal bool CanContinueRicePile(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueRicePile(f);
    internal bool CanOfferRicePile(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferRicePile(c, t, context);
    internal bool DrainRiceChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainRiceChildren(f);
    internal bool ExactRiceParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExactRiceParent(f, op);
    internal global::CardGame.Core.PublicPersistentPileSource? ExhaustedRicePile(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ExhaustedRicePile(c, context);
    internal void FinishRicePile(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().FinishRicePile(f, completed);
    internal void PublishRiceChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishRiceChoice(f);
    internal void ResolveRiceChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveRiceChoice(choice);
    internal bool ResumeRicePile(long id) => _resolve().ResumeRicePile(id);
    internal bool ReturnRiceMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnRiceMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RiceChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RiceChoices(f);
    internal global::CardGame.Core.CardConversionSource RiceIssuer(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RiceIssuer(f);
    internal bool RiceProducer(global::CardGame.Core.PublicPersistentPileSource p) => _resolve().RiceProducer(p);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PublicPersistentPileSource> RiceSources(int owner, global::CardGame.Core.SkillProgramEffect effect, string instance) => _resolve().RiceSources(owner, effect, instance);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> RiceStorageCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RiceStorageCards(f);
    internal global::CardGame.Core.PromptChoice SelectAiRiceChoice(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiRiceChoice(p, f);
    internal bool RiceCapability { get => _resolve().RiceCapability; }
}
