#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRicePileEntries
{
    internal SkillProgramStepOutcome BeginRicePile(SkillProgramEffect effect, ProgramSkillFrame supplied) => _resolve().BeginRicePile(effect, supplied);
    internal bool CanContinueRicePile(ProgramSkillFrame f) => _resolve().CanContinueRicePile(f);
    internal bool CanOfferRicePile(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferRicePile(c, t, context);
    internal bool DrainRiceChildren(ProgramSkillFrame f) => _resolve().DrainRiceChildren(f);
    internal bool ExactRiceParent(ProgramSkillFrame f, SkillProgramEffectOp op) => _resolve().ExactRiceParent(f, op);
    internal PublicPersistentPileSource? ExhaustedRicePile(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().ExhaustedRicePile(c, context);
    internal void FinishRicePile(ProgramSkillFrame f, bool completed) => _resolve().FinishRicePile(f, completed);
    internal void PublishRiceChoice(ProgramSkillFrame f) => _resolve().PublishRiceChoice(f);
    internal void ResolveRiceChoice(PromptChoice choice) => _resolve().ResolveRiceChoice(choice);
    internal bool ResumeRicePile(long id) => _resolve().ResumeRicePile(id);
    internal bool ReturnRiceMovement(ProgramSkillFrame f) => _resolve().ReturnRiceMovement(f);
    internal IReadOnlyList<PromptChoice> RiceChoices(ProgramSkillFrame f) => _resolve().RiceChoices(f);
    internal CardConversionSource RiceIssuer(ProgramSkillFrame f) => _resolve().RiceIssuer(f);
    internal bool RiceProducer(PublicPersistentPileSource p) => _resolve().RiceProducer(p);
    internal IReadOnlyList<PublicPersistentPileSource> RiceSources(int owner, SkillProgramEffect effect, string instance) => _resolve().RiceSources(owner, effect, instance);
    internal IEnumerable<Card> RiceStorageCards(ProgramSkillFrame f) => _resolve().RiceStorageCards(f);
    internal PromptChoice SelectAiRiceChoice(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiRiceChoice(p, f);
    internal bool RiceCapability { get => _resolve().RiceCapability; }
}
