#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedUndamagedTargetRevealEntries
{
    internal SkillProgramStepOutcome BeginCompletedUndamagedTargetReveal(ProgramSkillFrame supplied) => _resolve().BeginCompletedUndamagedTargetReveal(supplied);
    internal bool CanContinueCompletedUndamagedTargetReveal(ProgramSkillFrame f) => _resolve().CanContinueCompletedUndamagedTargetReveal(f);
    internal bool CanOfferCompletedUndamagedTargetReveal(ProgramTriggerLegalityContext gate) => _resolve().CanOfferCompletedUndamagedTargetReveal(gate);
    internal void CompleteCompletedUndamagedTargetReveal(ProgramSkillFrame f) => _resolve().CompleteCompletedUndamagedTargetReveal(f);
    internal int[] CompletedUndamagedEligibleTargets(CardUseFrame use) => _resolve().CompletedUndamagedEligibleTargets(use);
    internal bool CompletedUndamagedFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().CompletedUndamagedFinalizedAfterCategoryTargetWindow(query);
    internal bool CompletedUndamagedNativeUseActorMatches(CardUseFrame use, NativeOwners.CardAttackHandle attack) => _resolve().CompletedUndamagedNativeUseActorMatches(use, attack);
    internal bool CompletedUndamagedSourceUseQualifies(string skillId, CardUseFrame use) => _resolve().CompletedUndamagedSourceUseQualifies(skillId, use);
    internal IReadOnlyList<PromptChoice> CompletedUndamagedTargetRevealChoices(ProgramSkillFrame f) => _resolve().CompletedUndamagedTargetRevealChoices(f);
    internal CardUseFrame? CompletedUndamagedUse(ProgramSkillWindowContext context) => _resolve().CompletedUndamagedUse(context);
    internal void ContinueCompletedTargetPublicReveal(ProgramSkillFrame f) => _resolve().ContinueCompletedTargetPublicReveal(f);
    internal bool HasExactAcceptedCompletedUndamagedUse(CardUseFrame use, CardActionContext action) => _resolve().HasExactAcceptedCompletedUndamagedUse(use, action);
    internal bool HasExactCompletedUndamagedPublicPileCost(CardUseFrame use, CardActionContext action) => _resolve().HasExactCompletedUndamagedPublicPileCost(use, action);
    internal bool HasExactCompletedUndamagedTunanVirtualAttack(CardUseFrame use, CardActionContext action, NativeOwners.CardAttackHandle attack) => _resolve().HasExactCompletedUndamagedTunanVirtualAttack(use, action, attack);
    internal void IssueCompletedUndamagedTargetReveal(ProgramSkillFrame f) => _resolve().IssueCompletedUndamagedTargetReveal(f);
    internal void PublishCompletedUndamagedTargetRevealChoice(ProgramSkillFrame f) => _resolve().PublishCompletedUndamagedTargetRevealChoice(f);
    internal void RecordCompletedUndamagedTargetDamage(GameEngine.IDamageAttempt attack, long damageFrameId, int amount) => _resolve().RecordCompletedUndamagedTargetDamage(attack, damageFrameId, amount);
    internal void ResolveCompletedUndamagedTargetRevealChoice(PromptChoice choice) => _resolve().ResolveCompletedUndamagedTargetRevealChoice(choice);
    internal bool ResumeCompletedUndamagedTargetReveal(long id) => _resolve().ResumeCompletedUndamagedTargetReveal(id);
    internal bool ReturnCompletedUndamagedTargetRevealMovement(ProgramSkillFrame f) => _resolve().ReturnCompletedUndamagedTargetRevealMovement(f);
    internal long CompletedUndamagedMovementSequence { get => _resolve().CompletedUndamagedMovementSequence; }
    internal bool TracksCompletedUndamagedUseDamage { get => _resolve().TracksCompletedUndamagedUseDamage; }
}
