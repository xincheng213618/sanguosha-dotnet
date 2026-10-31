#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSameNameHandDiscardOrDamageEntries
{
    internal SkillProgramStepOutcome BeginSameNameHandDiscardOrDamage(ProgramSkillFrame supplied) => _resolve().BeginSameNameHandDiscardOrDamage(supplied);
    internal bool CanContinueSameNameHandDiscardOrDamage(ProgramSkillFrame f) => _resolve().CanContinueSameNameHandDiscardOrDamage(f);
    internal bool CanOfferSameNameHandDiscardOrDamage(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferSameNameHandDiscardOrDamage(candidate, trigger, context);
    internal void FinishSameNameHand(ProgramSkillFrame f) => _resolve().FinishSameNameHand(f);
    internal void PublishSameNameHandChoice(ProgramSkillFrame f) => _resolve().PublishSameNameHandChoice(f);
    internal void ResolveSameNameHandDiscardOrDamageChoice(PromptChoice choice) => _resolve().ResolveSameNameHandDiscardOrDamageChoice(choice);
    internal bool ResumeSameNameHandDiscardOrDamage(long id) => _resolve().ResumeSameNameHandDiscardOrDamage(id);
    internal bool ReturnSameNameHandDiscardOrDamageMovement(ProgramSkillFrame f) => _resolve().ReturnSameNameHandDiscardOrDamageMovement(f);
    internal bool SameNameHandActionParent(int owner, ProgramSkillWindowContext context, scoped out ProgramCardTriggerWindowFrame window) => _resolve().SameNameHandActionParent(owner, context, out window);
    internal PromptChoice SameNameHandChoice(ProgramSkillFrame f, string step, string label, int? card = default, int? target = default) => _resolve().SameNameHandChoice(f, step, label, card, target);
    internal IReadOnlyList<PromptChoice> SameNameHandChoices(ProgramSkillFrame f) => _resolve().SameNameHandChoices(f);
    internal string SameNameHandDiscardReason(ProgramSkillFrame f) => _resolve().SameNameHandDiscardReason(f);
    internal SameNameHandMaterial[] SameNameHandMaterials(int seat, CardKind name) => _resolve().SameNameHandMaterials(seat, name);
    internal int[] SameNameHandTargets(int owner) => _resolve().SameNameHandTargets(owner);
    internal PromptChoice SelectAiSameNameHandDiscardOrDamage(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiSameNameHandDiscardOrDamage(decision, f);
}
