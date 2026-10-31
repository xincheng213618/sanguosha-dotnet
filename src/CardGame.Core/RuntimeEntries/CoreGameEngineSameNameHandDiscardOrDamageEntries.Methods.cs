#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSameNameHandDiscardOrDamageEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSameNameHandDiscardOrDamage(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginSameNameHandDiscardOrDamage(supplied);
    internal bool CanContinueSameNameHandDiscardOrDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueSameNameHandDiscardOrDamage(f);
    internal bool CanOfferSameNameHandDiscardOrDamage(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferSameNameHandDiscardOrDamage(candidate, trigger, context);
    internal void FinishSameNameHand(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishSameNameHand(f);
    internal bool IsSameNameHandDiscardOrDamageMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsSameNameHandDiscardOrDamageMovement(f, e, pending);
    internal void PublishSameNameHandChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSameNameHandChoice(f);
    internal void ResolveSameNameHandDiscardOrDamageChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveSameNameHandDiscardOrDamageChoice(choice);
    internal bool ResumeSameNameHandDiscardOrDamage(long id) => _resolve().ResumeSameNameHandDiscardOrDamage(id);
    internal bool ReturnSameNameHandDiscardOrDamageMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSameNameHandDiscardOrDamageMovement(f);
    internal bool SameNameHandActionParent(int owner, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().SameNameHandActionParent(owner, context, out window);
    internal global::CardGame.Core.PromptChoice SameNameHandChoice(global::CardGame.Core.ProgramSkillFrame f, string step, string label, int? card = default, int? target = default) => _resolve().SameNameHandChoice(f, step, label, card, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SameNameHandChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SameNameHandChoices(f);
    internal string SameNameHandDiscardReason(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SameNameHandDiscardReason(f);
    internal global::CardGame.Core.SameNameHandMaterial[] SameNameHandMaterials(int seat, global::CardGame.Core.CardKind name) => _resolve().SameNameHandMaterials(seat, name);
    internal int[] SameNameHandTargets(int owner) => _resolve().SameNameHandTargets(owner);
    internal global::CardGame.Core.PromptChoice SelectAiSameNameHandDiscardOrDamage(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiSameNameHandDiscardOrDamage(decision, f);
}
