#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageSourceHandChoiceEntries
{
    internal SkillProgramStepOutcome BeginDamageSourceHandChoice(ProgramSkillFrame supplied) => _resolve().BeginDamageSourceHandChoice(supplied);
    internal bool CanContinueDamageSourceHandChoice(ProgramSkillFrame f) => _resolve().CanContinueDamageSourceHandChoice(f);
    internal bool CanOfferDamageSourceHandChoice(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferDamageSourceHandChoice(c, t, context);
    internal void CompleteDamageSourceHandChoice(ProgramSkillFrame f, bool completed) => _resolve().CompleteDamageSourceHandChoice(f, completed);
    internal IReadOnlyList<PromptChoice> DamageSourceHandChoices(ProgramSkillFrame f) => _resolve().DamageSourceHandChoices(f);
    internal void PublishDamageSourceHandChoice(ProgramSkillFrame f) => _resolve().PublishDamageSourceHandChoice(f);
    internal void RequireDamageSourceHandChoice(ProgramSkillFrame f) => _resolve().RequireDamageSourceHandChoice(f);
    internal void ResolveDamageSourceHandChoice(PromptChoice choice) => _resolve().ResolveDamageSourceHandChoice(choice);
    internal bool ResumeDamageSourceHandChoice(long id) => _resolve().ResumeDamageSourceHandChoice(id);
    internal bool ReturnDamageSourceHandMovement(ProgramSkillFrame f) => _resolve().ReturnDamageSourceHandMovement(f);
    internal PromptChoice SelectAiDamageSourceHandChoice(PendingDecision d, ProgramSkillFrame f) => _resolve().SelectAiDamageSourceHandChoice(d, f);
}
