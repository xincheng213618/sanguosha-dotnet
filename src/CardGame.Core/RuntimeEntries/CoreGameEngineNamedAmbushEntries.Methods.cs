#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedAmbushEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginNamedAmbush(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginNamedAmbush(effect, supplied);
    internal bool CanContinueNamedAmbush(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueNamedAmbush(f);
    internal bool CanOfferNamedAmbush(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferNamedAmbush(candidate, trigger, context);
    internal void CleanupLostNamedAmbushPiles() => _resolve().CleanupLostNamedAmbushPiles();
    internal void CompleteNamedAmbush(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().CompleteNamedAmbush(f, completed);
    internal bool DrainNamedHandChoiceChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainNamedHandChoiceChildren(f);
    internal global::CardGame.Core.NamedAmbushDeposit[] LiveAmbushDeposits(int owner, string? skill = default, string? instance = default) => _resolve().LiveAmbushDeposits(owner, skill, instance);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> NamedAmbushChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NamedAmbushChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NamedAmbushPileSnapshot>? ProjectNamedAmbushPiles(int owner, int viewer, bool revealAll) => _resolve().ProjectNamedAmbushPiles(owner, viewer, revealAll);
    internal void PublishNamedAmbushChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishNamedAmbushChoice(f);
    internal void RequireNamedAmbushReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireNamedAmbushReceipt(f);
    internal void ResolveNamedAmbushChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveNamedAmbushChoice(choice);
    internal bool ResumeNamedAmbush(long id) => _resolve().ResumeNamedAmbush(id);
    internal bool ReturnNamedAmbushMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnNamedAmbushMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiNamedAmbush(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiNamedAmbush(decision, f);
    internal bool TryGetAmbushUse(global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.NamedAmbushUseIdentity identity, scoped out global::CardGame.Core.CardActionContext action) => _resolve().TryGetAmbushUse(context, out identity, out action);
    internal bool HasNamedAmbushCapability { get => _resolve().HasNamedAmbushCapability; }
}
