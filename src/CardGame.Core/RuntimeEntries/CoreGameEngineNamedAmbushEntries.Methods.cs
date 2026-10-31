#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedAmbushEntries
{
    internal SkillProgramStepOutcome BeginNamedAmbush(SkillProgramEffect effect, ProgramSkillFrame supplied) => _resolve().BeginNamedAmbush(effect, supplied);
    internal bool CanContinueNamedAmbush(ProgramSkillFrame f) => _resolve().CanContinueNamedAmbush(f);
    internal bool CanOfferNamedAmbush(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferNamedAmbush(candidate, trigger, context);
    internal void CleanupLostNamedAmbushPiles() => _resolve().CleanupLostNamedAmbushPiles();
    internal void CompleteNamedAmbush(ProgramSkillFrame f, bool completed) => _resolve().CompleteNamedAmbush(f, completed);
    internal bool DrainNamedHandChoiceChildren(ProgramSkillFrame f) => _resolve().DrainNamedHandChoiceChildren(f);
    internal NamedAmbushDeposit[] LiveAmbushDeposits(int owner, string? skill = default, string? instance = default) => _resolve().LiveAmbushDeposits(owner, skill, instance);
    internal IReadOnlyList<PromptChoice> NamedAmbushChoices(ProgramSkillFrame f) => _resolve().NamedAmbushChoices(f);
    internal IReadOnlyList<NamedAmbushPileSnapshot>? ProjectNamedAmbushPiles(int owner, int viewer, bool revealAll) => _resolve().ProjectNamedAmbushPiles(owner, viewer, revealAll);
    internal void PublishNamedAmbushChoice(ProgramSkillFrame f) => _resolve().PublishNamedAmbushChoice(f);
    internal void RequireNamedAmbushReceipt(ProgramSkillFrame f) => _resolve().RequireNamedAmbushReceipt(f);
    internal void ResolveNamedAmbushChoice(PromptChoice choice) => _resolve().ResolveNamedAmbushChoice(choice);
    internal bool ResumeNamedAmbush(long id) => _resolve().ResumeNamedAmbush(id);
    internal bool ReturnNamedAmbushMovement(ProgramSkillFrame f) => _resolve().ReturnNamedAmbushMovement(f);
    internal PromptChoice SelectAiNamedAmbush(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiNamedAmbush(decision, f);
    internal bool TryGetAmbushUse(ProgramSkillWindowContext context, scoped out NamedAmbushUseIdentity identity, scoped out CardActionContext action) => _resolve().TryGetAmbushUse(context, out identity, out action);
    internal bool HasNamedAmbushCapability { get => _resolve().HasNamedAmbushCapability; }
}
