#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedUndamagedTargetRevealEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCompletedUndamagedTargetReveal(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginCompletedUndamagedTargetReveal(supplied);
    internal bool CanContinueCompletedUndamagedTargetReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueCompletedUndamagedTargetReveal(f);
    internal bool CanOfferCompletedUndamagedTargetReveal(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferCompletedUndamagedTargetReveal(gate);
    internal void CompleteCompletedUndamagedTargetReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteCompletedUndamagedTargetReveal(f);
    internal int[] CompletedUndamagedEligibleTargets(global::CardGame.Core.CardUseFrame use) => _resolve().CompletedUndamagedEligibleTargets(use);
    internal bool CompletedUndamagedFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().CompletedUndamagedFinalizedAfterCategoryTargetWindow(query);
    internal bool CompletedUndamagedNativeUseActorMatches(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CompletedUndamagedNativeUseActorMatches(use, attack);
    internal bool CompletedUndamagedSourceUseQualifies(string skillId, global::CardGame.Core.CardUseFrame use) => _resolve().CompletedUndamagedSourceUseQualifies(skillId, use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CompletedUndamagedTargetRevealChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompletedUndamagedTargetRevealChoices(f);
    internal global::CardGame.Core.CardUseFrame? CompletedUndamagedUse(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CompletedUndamagedUse(context);
    internal void ContinueCompletedTargetPublicReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueCompletedTargetPublicReveal(f);
    internal bool HasExactAcceptedCompletedUndamagedUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action) => _resolve().HasExactAcceptedCompletedUndamagedUse(use, action);
    internal bool HasExactCompletedUndamagedPublicPileCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action) => _resolve().HasExactCompletedUndamagedPublicPileCost(use, action);
    internal bool HasExactCompletedUndamagedTunanVirtualAttack(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action, global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().HasExactCompletedUndamagedTunanVirtualAttack(use, action, attack);
    internal bool IsCompletedUndamagedTargetRevealMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsCompletedUndamagedTargetRevealMovement(f, effect, pending);
    internal void IssueCompletedUndamagedTargetReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueCompletedUndamagedTargetReveal(f);
    internal void PublishCompletedUndamagedTargetRevealChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCompletedUndamagedTargetRevealChoice(f);
    internal void RecordCompletedUndamagedTargetDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, int amount) => _resolve().RecordCompletedUndamagedTargetDamage(attack, damageFrameId, amount);
    internal void ResolveCompletedUndamagedTargetRevealChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveCompletedUndamagedTargetRevealChoice(choice);
    internal bool ResumeCompletedUndamagedTargetReveal(long id) => _resolve().ResumeCompletedUndamagedTargetReveal(id);
    internal bool ReturnCompletedUndamagedTargetRevealMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnCompletedUndamagedTargetRevealMovement(f);
    internal long CompletedUndamagedMovementSequence { get => _resolve().CompletedUndamagedMovementSequence; }
    internal bool TracksCompletedUndamagedUseDamage { get => _resolve().TracksCompletedUndamagedUseDamage; }
}
