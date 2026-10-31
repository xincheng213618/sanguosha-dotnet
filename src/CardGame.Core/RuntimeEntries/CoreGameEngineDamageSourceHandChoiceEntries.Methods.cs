#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageSourceHandChoiceEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDamageSourceHandChoice(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginDamageSourceHandChoice(supplied);
    internal bool CanContinueDamageSourceHandChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueDamageSourceHandChoice(f);
    internal bool CanOfferDamageSourceHandChoice(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferDamageSourceHandChoice(c, t, context);
    internal void CompleteDamageSourceHandChoice(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().CompleteDamageSourceHandChoice(f, completed);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DamageSourceHandChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DamageSourceHandChoices(f);
    internal void PublishDamageSourceHandChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishDamageSourceHandChoice(f);
    internal void RequireDamageSourceHandChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireDamageSourceHandChoice(f);
    internal void ResolveDamageSourceHandChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveDamageSourceHandChoice(choice);
    internal bool ResumeDamageSourceHandChoice(long id) => _resolve().ResumeDamageSourceHandChoice(id);
    internal bool ReturnDamageSourceHandMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnDamageSourceHandMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiDamageSourceHandChoice(global::CardGame.Core.PendingDecision d, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiDamageSourceHandChoice(d, f);
}
